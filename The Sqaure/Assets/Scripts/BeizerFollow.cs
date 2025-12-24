using System.Collections;
using UnityEngine;

public class BeizerFollow : Follow
{
    [SerializeField] bool Loop = false;
    int RouteToGo;
    float Tparam;
    Vector2 catpos;
    float SpeedModifier;
    bool CoroutineAllowed;

    private void Start()
    {
        RouteToGo = 0;
        Tparam = 0;
        SpeedModifier = 0.05f;
        CoroutineAllowed = true;
    }
    private void Update()
    {
        if (CoroutineAllowed)
            StartCoroutine(GoByTheRoute(RouteToGo));
    }
    IEnumerator GoByTheRoute(int n)
    {
        CoroutineAllowed = false;

        Vector2 p0 = Routes[n].GetChild(0).transform.position;
        Vector2 p1 = Routes[n].GetChild(1).transform.position;
        Vector2 p2 = Routes[n].GetChild(2).transform.position;
        Vector2 p3 = Routes[n].GetChild(3).transform.position;

        while(Tparam < 1)
        {
            Tparam += Time.deltaTime * SpeedModifier * Speed;

            catpos = Mathf.Pow(1 - Tparam, 3) * p0 + 3 * Mathf.Pow(1 - Tparam, 2) * Tparam * p1 + 3 * (1 - Tparam) * Mathf.Pow(Tparam, 2) * p2 + Mathf.Pow(Tparam, 3) * p3;

            transform.position = catpos;
            yield return new WaitForEndOfFrame();
        }

        Tparam = 0;
        RouteToGo++;

        CoroutineAllowed = true;
        if (RouteToGo == Routes.Count)
        {
            if (Loop)
            {
                RouteToGo = 0;
            }
            else
            {
                CoroutineAllowed = false;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}