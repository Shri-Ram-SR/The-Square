using UnityEngine;

public class JumpSpring : MonoBehaviour
{
    [SerializeField] float Power;
    [SerializeField] Vector2 Dir;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement pm = collision.gameObject.GetComponent<PlayerMovement>();
            if (pm.GetState() == PlayerState.Dash || pm.GetState() == PlayerState.Slam)
                pm.SetVelocity(Dir * Power * 1.5f);
            pm.SetVelocity(Dir * Power * 1.5f);
            if(Dir.x != 0)
            {
                pm.Ignoremovement(.25f);
            }
        }
    }
}
