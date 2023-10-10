using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private AbstractHealth _heathController;
    private Rigidbody _rb;

    public void SetHealthController(AbstractHealth healthcontroller)
    {
        _rb = GetComponent<Rigidbody>();
        _heathController = healthcontroller;
    }

    public void Damage(float damage, Vector3 direction, Vector3 point)
    {
        _heathController.Damage(damage, _rb, direction, point);
    }
}
