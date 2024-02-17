using UnityEngine;

public class Foot : MonoBehaviour
{
    private float _speed;

    public void Initialize(float speed)
    { 
        _speed = speed;
    }
    private void FixedUpdate()
    {
        transform.position += transform.right * _speed * Time.fixedDeltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FootFinish"))
        { 
            Destroy(gameObject);
        }
    }
}
