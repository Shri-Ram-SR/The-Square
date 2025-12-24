using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class LinearFollow : Follow
{
    public override void Setup(List<Transform> r, float s)
    {
        base.Setup(r, s);
        transform.position = r[0].position;
        transform.DOMove(r[1].position, Vector2.Distance(r[0].position, r[1].position) / s).SetEase(Ease.Linear);
    }
}
