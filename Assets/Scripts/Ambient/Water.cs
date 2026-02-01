using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] float Speed = 0.2f;
    [SerializeField] Vector2 Direction = new Vector2(0, 1);
    [SerializeField] bool IsActive = false;

    void Update()
    {
        if (!IsActive) return;
        transform.Translate(Direction * Speed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.transform.root.CompareTag("Player"))
            return;

        GameplayManager.Singleton.Die();
    }

    public void Activate()
    {
        IsActive = true;
    }
}
