using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSystem : MonoBehaviour
{
    [SerializeField] List<RawImage> HealthImages = new List<RawImage>();
    [SerializeField] int TotalHealth;
    [SerializeField] int CurHealth;
    [SerializeField] LayerMask EnemyMask;
    public static PlayerHealthSystem Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        CurHealth = TotalHealth;
        UpdateUI();

        for (int i = TotalHealth; i < HealthImages.Count; i++)
            HealthImages[i].gameObject.SetActive(false);
    }
    void UpdateUI()
    {
        for (int i = 0; i < CurHealth; i++)
            HealthImages[i].gameObject.SetActive(true); 
        for (int i = CurHealth; i < TotalHealth; i++)
            HealthImages[i].gameObject.SetActive(false);
    }
    public void AddHealth(int i)
    {
        CurHealth += i;
        UpdateUI();
    }
    public void SubHealth(int i)
    {
        CurHealth -= i;
        UpdateUI();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            SubHealth(1);
    }
}
