using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using Mirror;

namespace Underground_Laboratory_Generator.Scripts
{
    public class LaboratoryGenerator : NetworkBehaviour
    {
        public UnityEvent done;
        [SerializeField] private bool generateOnStart = true;
        [Range(2, 100)]
        [SerializeField] private int roomCount = 9;
        [SerializeField] private LayerMask cellLayer;

        [SerializeField] private GameObject insteadDoor;
        [SerializeField] private GameObject[] doorPrefabs;
        [SerializeField] private Cell[] cellPrefabs;
        public static LaboratoryGenerator Instance;

   
        void Awake() 
        {
            Instance = this;
        }
        private void Start()
        {
            if (generateOnStart) StartCoroutine(StartGeneration());
        }

       public IEnumerator StartGeneration()
        {
            List<Transform> createdExits = new List<Transform>();
            Cell startRoom = Instantiate(cellPrefabs[Random.Range(0, cellPrefabs.Length)], Vector3.zero, Quaternion.identity) ?? throw new ArgumentNullException(nameof(Instantiate) + "(cellPrefabs[Random.Range(0, cellPrefabs.Length)], Vector3.zero, Quaternion.identity)");
            NetworkServer.Spawn(startRoom.gameObject);
            for (int i = 0; i < startRoom.Exits.Length; i++) createdExits.Add(startRoom.Exits[i].transform);
            startRoom.TriggerBox.enabled = true;

            int limit = 1000, roomsLeft = roomCount - 1;
            while (limit > 0 && roomsLeft > 0)
            {
                limit--;

                Cell selectedPrefab = Instantiate(cellPrefabs[Random.Range(0, cellPrefabs.Length)], Vector3.zero, Quaternion.identity);
                NetworkServer.Spawn(selectedPrefab.gameObject);
                
                int lim = 100;
                bool collided;
                Transform selectedExit;
                Transform createdExit; // из списка созданных входов

                selectedPrefab.TriggerBox.enabled = false; // чтобы сам себя не проверял на наличие нахлеста ВЫКЛЮЧИЛ

                do
                {
                    lim--;

                    createdExit = createdExits[Random.Range(0, createdExits.Count)];
                    selectedExit = selectedPrefab.Exits[Random.Range(0, selectedPrefab.Exits.Length)].transform;

                    // rotation
                    float shiftAngle = createdExit.eulerAngles.z + 180 - selectedExit.eulerAngles.z;
                    Transform selectedPrefabTransform;
                    (selectedPrefabTransform = selectedPrefab.transform).Rotate(new Vector3(0, 0, shiftAngle)); // выходы повернуты друг напротив друга

                    // position
                    Vector3 shiftPosition = createdExit.position - selectedExit.position;
                    var position = selectedPrefabTransform.position;
                    position += shiftPosition; // выходы состыковались
                    selectedPrefabTransform.position = position;

                    // check
                    var triggerBoxCenter = selectedPrefab.TriggerBox.center;
                    Vector3 center = position + triggerBoxCenter.z * selectedPrefabTransform.forward
                                              + triggerBoxCenter.y * selectedPrefabTransform.up
                                              + triggerBoxCenter.x * selectedPrefabTransform.right; // selectedPrefab.TriggerBox.center
                    Vector3 size = selectedPrefab.TriggerBox.size / 2f; // half size
                    Quaternion rot = selectedPrefabTransform.localRotation;
                    collided = Physics.CheckBox(center, size, rot, cellLayer, QueryTriggerInteraction.Collide);

                    yield return new WaitForEndOfFrame();

                } while (collided && lim > 0);

                selectedPrefab.TriggerBox.enabled = true; // ВКЛЮЧИЛ

                if (lim > 0)
                {
                    roomsLeft--;

                    for (int j = 0; j < selectedPrefab.Exits.Length; j++) createdExits.Add(selectedPrefab.Exits[j].transform);

                    createdExits.Remove(createdExit);
                    createdExits.Remove(selectedExit);

                    Transform exitTransform;
                    var door = Instantiate(doorPrefabs[Random.Range(0, doorPrefabs.Length)], (exitTransform = createdExit.transform).position, exitTransform.rotation);
                    NetworkServer.Spawn(door.gameObject);
                    DestroyImmediate(createdExit.gameObject);
                    DestroyImmediate(selectedExit.gameObject);
                }
                else DestroyImmediate(selectedPrefab.gameObject);

                yield return new WaitForEndOfFrame();
            }

            // instead doors
            for (int i = 0; i < createdExits.Count; i++)
            {
                var door = Instantiate(insteadDoor, createdExits[i].position, createdExits[i].rotation);
                NetworkServer.Spawn(door.gameObject);
                DestroyImmediate(createdExits[i].gameObject);
            }

            Debug.Log("Finished " + Time.time);
            done.Invoke();
        }
    }
}