using System.Collections.Generic;
using UnityEngine;

public class BeizierCurve : MonoBehaviour
{
    [SerializeField] List<Transform> Points;
    Vector2 GizmosPos;

    private void OnDrawGizmos()
    {
        for(float t = 0; t <= 1; t += 0.05f)
        {
            GizmosPos = Mathf.Pow(1 - t, 3) * Points[0].position +
                3 * Mathf.Pow(1 - t, 2) * t * Points[1].position +
                3 * (1 - t) * Mathf.Pow(t, 2) * Points[2].position +
                Mathf.Pow(t, 3) * Points[3].position;

            Gizmos.DrawSphere(GizmosPos, .25f);
        }

        Gizmos.DrawLine(new Vector2(Points[0].position.x, Points[0].position.y), new Vector2(Points[1].position.x, Points[1].position.y));

        Gizmos.DrawLine(new Vector2(Points[2].position.x, Points[2].position.y), new Vector2(Points[3].position.x, Points[3].position.y));

    }
}
