using UnityEngine;

public class BreakableFloor : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerMovement>().GetState() == PlayerState.Slam)
        {
            Destroy(gameObject);
        }
    }
}
