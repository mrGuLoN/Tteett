using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CanvasSingleTone : MonoBehaviour
{
    public static CanvasSingleTone instance =null;

    public Joystick movementJoyStick => _movementJoyStick;
    public Joystick fireJoyStick => _fireJoyStick;

    [SerializeField]private Joystick _movementJoyStick, _fireJoyStick;

    private Transform _target, _cameraTransform;
    private float _distance;
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
        
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        _cameraTransform = Camera.main.transform;
        _distance = _cameraTransform.position.y - _target.transform.position.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_target != null)
        {
            _cameraTransform.position = _target.position + Vector3.up * _distance;
        }
    }
}
