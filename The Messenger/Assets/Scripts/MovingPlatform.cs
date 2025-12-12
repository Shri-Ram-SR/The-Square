using DG.Tweening;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    GameObject player;
    Vector3 v;
    private void Start()
    {
        v = transform.position;
    }
    private void Update()
    {
        if(player)
            player.transform.position += transform.position - v;
        v = transform.position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            player = collision.gameObject;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            player = null;
    }
}
