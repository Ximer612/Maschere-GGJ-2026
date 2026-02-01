using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] bool IsActive;
    [SerializeField] SpawnPoint NextPoint;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!IsActive) return;

        GameplayManager.Singleton.UpdateMajorSpawnPoint(transform.position);

        NextPoint.IsActive = true;
        IsActive = false;

        // GameplayManager.Singleton.SetWaterLevel(Vec2);
    }
}
