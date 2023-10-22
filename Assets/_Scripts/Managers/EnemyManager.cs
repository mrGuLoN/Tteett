using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : NetworkBehaviour
{
    public static EnemyManager instanse;
    public UnityEvent<Transform> newPlayer;
    public UnityEvent<EnemyStateMachineController> newEnemy;
    public UnityEvent<EnemyStateMachineController> deadEnemy;

    [SerializeField] private Transform _target;
    [SerializeField] private List<EnemyStateMachineController> _enemys = new();
    void Awake()
    {
        instanse = this;
        newPlayer.AddListener(AddTargetTransform);
        newEnemy.AddListener(NewEnemy);
        deadEnemy.AddListener(DeadEnemy);
    }

    private void DeadEnemy(EnemyStateMachineController arg0)
    {
        _enemys.Remove(arg0);
    }

    private void NewEnemy(EnemyStateMachineController arg0)
    {
        _enemys.Add(arg0);
    }

    private void AddTargetTransform(Transform target)
    {
        _target = target;
    }
   
    void FixedUpdate()
    {
        if (_target != null)
        {
            foreach (var enemy in _enemys)
            {
                enemy.EveryFrameUpdate();
                enemy.EndEveryFrameUpdateLateUpdate();
                enemy.Update60FixedUpdate(_target);
            }
        }
    }
}
