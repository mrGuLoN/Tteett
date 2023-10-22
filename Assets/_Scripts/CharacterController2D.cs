using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    private Rigidbody2D _rb2D;

    private void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }



    public void Move(Vector2 Direction)
    {
        _rb2D.velocity = Direction*100;
    }
}
