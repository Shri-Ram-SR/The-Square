using UnityEngine;

public class Spike : MonoBehaviour
{
    void PlayerDamaged()
    {
        PlayerHealthSystem.Instance.SubHealth(1);
        RespawnManager.Instance.Respawn();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            PlayerDamaged();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            PlayerDamaged();
    }
}
