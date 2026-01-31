using UnityEngine;

public class Grata : MonoBehaviour
{
    private MovementControl movementControl;
    public Collider2D collision;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.transform.root.CompareTag("Player"))
            return;

        var maybeControl = collider.transform.root.GetComponent<MovementControl>();
        if (maybeControl == null) return;

        movementControl = maybeControl;
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (!collider.transform.root.CompareTag("Player"))
            return;

        var maybeControl = collider.transform.root.GetComponent<MovementControl>();
        if (maybeControl != movementControl) return;

        movementControl = null;
    }

    public void Update()
    {
        EvaluateCollisionStatus();
    }

    private void EvaluateCollisionStatus()
    {
        collision.enabled = movementControl == null || !movementControl.IsDashing;
    }
}
