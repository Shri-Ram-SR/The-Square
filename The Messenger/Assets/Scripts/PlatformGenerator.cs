using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] float WaitTime;
    [SerializeField] float Speed;
    [SerializeField] float LifeTime;
    [SerializeField] List<Transform> Routes;
    [SerializeField] GameObject Platform;
    void Start()
    {
        StartCoroutine(Generate());
    }

    IEnumerator Generate()
    {
        GameObject p = Instantiate(Platform);
        p.GetComponent<BeizerFollow>().Setup(Routes, Speed);
        Destroy(p, LifeTime);
        yield return new WaitForSeconds(WaitTime);
        StartCoroutine(Generate());
    }
}
