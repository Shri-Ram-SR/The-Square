using UnityEngine;

public class InteractiveBgObject : MonoBehaviour
{
    [SerializeField] string TriggerState;
    Animator Ani;
    void Start()
    {
        Ani = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Ani && collision.CompareTag("Player"))
            Ani.SetTrigger(TriggerState);
    }
}
