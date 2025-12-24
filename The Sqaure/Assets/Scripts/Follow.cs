using System.Collections.Generic;
using UnityEngine;

public abstract class Follow : MonoBehaviour
{
    public List<Transform> Routes;
    public float Speed;
    public virtual void Setup(List<Transform> r, float s)
    {
        Routes = r;
        Speed = s;
    }
}
