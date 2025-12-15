using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] GameObject Rock;
    [SerializeField] float Speed;
    [SerializeField] float CooldownTime;
    [SerializeField] float Timer;

    [Header("Damage")]
    [SerializeField] int ThrowDamage;
    [SerializeField] float AOEDamage;

    [Header("Misc")]
    [SerializeField] Transform ChakraPoint;
    [SerializeField] LayerMask InterruptLayer;

    public static PlayerCombat Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    void Update()
    {
        if (Timer > 0)
            Timer -= Time.deltaTime;
    }
    public void ThrowAttack()
    {
        if(Timer <= 0)
        {
            GameObject P = Instantiate(Rock, ChakraPoint.position, ChakraPoint.rotation);
            P.GetComponent<Projectile>().SetUp(ThrowDamage);
            P.transform.right = transform.right;
            Timer = CooldownTime;
        }
    }
    public void Slam()
    {
        //AOE Attack
    }
}
