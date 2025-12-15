using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    bool Checked;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!Checked && collision.CompareTag("Player"))
        {
            RespawnManager.Instance.UpdateRespawnPoint(transform.position);
            Checked = true;
        }
    }
}
