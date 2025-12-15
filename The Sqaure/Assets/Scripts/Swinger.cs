using UnityEngine;
using DG.Tweening;
using System.Collections;
public class Swinger : MonoBehaviour
{
    [SerializeField] Transform SpinPoint;
    [SerializeField] float SpinDuration;
    [SerializeField] Vector3 SpinPosition;
    [SerializeField] float LaunchPower;
    [SerializeField] float FreezeTime;
    GameObject Player;
    IEnumerator Spin()
    {
        Player.transform.position = SpinPoint.position;
        Player.transform.SetParent(SpinPoint);
        PlayerMovement.Instance.StartSpin();

        transform.DORotate(transform.rotation.eulerAngles + SpinPosition, SpinDuration).SetEase(Ease.Linear);

        yield return new WaitForSeconds(SpinDuration);

        Player.transform.SetParent(null);
        StartCoroutine(PlayerMovement.Instance.EndSpin(transform.right * LaunchPower, FreezeTime));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player = collision.gameObject;
            StartCoroutine(Spin());
        }
    }
}
