using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    int Damage;
    [SerializeField] float Speed;
    [SerializeField] Rigidbody2D rb;

    private void Update()
    {
        rb.linearVelocityX = Speed;
    }
    public void SetUp(int D)
    {
        Speed *= transform.right.x;
        Damage = D;
        Invoke("DestroyItself", 5f);
    }
    void DestroyItself()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            Debug.Log("Hit");
        }
        if (collision.gameObject.layer == 3)
        {
            DestroyItself();
        }
    }
}
