using UnityEngine;
using DG.Tweening;
public class SpinningLauncher : MonoBehaviour
{
    [SerializeField] Transform SpinPoint;
    [SerializeField] float SpinDuration;
    [SerializeField] Vector3 SpinPosition;
    [SerializeField] Vector2 LaunchDirection;
    GameObject Player;
    void Spin()
    {
        Player.transform.position = SpinPoint.position;
        Player.transform.SetParent(SpinPoint);
        Player.GetComponent<SpriteRenderer>().enabled = false;
        transform.DORotate(SpinPosition, SpinDuration);
        Invoke("Launch", SpinDuration);
    }
    void Launch()
    {
        Player.transform.SetParent(null);
        PlayerMovement.Instance.SetState(PlayerState.Zero);
        Player.GetComponent<SpriteRenderer>().enabled = true;
        PlayerMovement.Instance.SetVelocity(LaunchDirection);
        PlayerMovement.Instance.Ignoremovement(.2f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player = collision.gameObject;
            Spin();
        }
    }
}
