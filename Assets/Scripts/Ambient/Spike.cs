using UnityEngine;

public class Spike : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.transform.root.CompareTag("Player"))
            return;

        GameplayManager.Singleton.Die();
    }
}
