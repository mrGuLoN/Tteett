using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CanvasSingleTone : MonoBehaviour
{
    public static CanvasSingleTone instance =null;

    public Joystick movementJoyStick => _movementJoyStick;
    public Joystick fireJoyStick => _fireJoyStick;

    [SerializeField] private Joystick _movementJoyStick, _fireJoyStick;
    [SerializeField] private Transform _cameraTransform;

    [SerializeField] private Transform _target;
    private Vector3 _distance;
    void Awake()
    { 
        if (instance == null)
        {
            instance = this; 
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetTarget();
    }

    public void SetTarget()
    {
        _distance = _cameraTransform.position - _target.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
      
            _cameraTransform.position = _target.position + _distance;
       
    }
}
