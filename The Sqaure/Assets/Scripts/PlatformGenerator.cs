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
    [SerializeField] bool Beizer;
    void Start()
    {
        StartCoroutine(Generate());
    }

    IEnumerator Generate()
    {
        GameObject p = Instantiate(Platform);
        if (Beizer)
            p.AddComponent<BeizerFollow>().Setup(Routes,Speed);
        else
            p.AddComponent<LinearFollow>().Setup(Routes,Speed);

        Destroy(p, LifeTime);
        yield return new WaitForSeconds(WaitTime);
        StartCoroutine(Generate());
    }
}
