using UnityEngine;

public class WaterDeactivator : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.transform.root.CompareTag("Player"))
            return;

        GameplayManager.Singleton.StopWaterSection();
    }
}
