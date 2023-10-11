using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private AbstractHealth _heathController;
    private int _intRigidbody;

    public void SetHealthController(AbstractHealth healthcontroller, int intRigidbody)
    {
        _heathController = healthcontroller;
        _intRigidbody = intRigidbody;
    }

    public void Damage(float damage, Vector3 direction, Vector3 point)
    {
        _heathController.Damage(damage, _intRigidbody, direction, point);
    }
}
