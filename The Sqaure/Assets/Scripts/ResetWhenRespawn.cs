using UnityEngine;

public class ResetWhenRespawn : MonoBehaviour
{
    Vector3 pos;
    Quaternion rot;
    void Start()
    {
        pos = transform.position;
        rot = transform.rotation;

        RespawnManager.Respawned.AddListener(PlayerRespawn);
    }
    void PlayerRespawn()
    {
        transform.position = pos;
        transform.rotation = rot;
    }
}
