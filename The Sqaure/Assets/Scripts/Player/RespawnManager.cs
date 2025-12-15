using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] Vector3 Spawn;
    public static RespawnManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public void Respawn()
    {
        Player.transform.position = Spawn;
    }
    public void UpdateRespawnPoint(Vector3 point)
    {
        Spawn = point;
    }
}
