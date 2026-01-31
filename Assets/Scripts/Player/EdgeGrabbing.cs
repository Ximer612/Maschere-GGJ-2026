using UnityEngine;

public delegate void OnHitted();
public class EdgeGrabbing : MonoBehaviour
{
    [SerializeField] float forwardOffset, upOffset = 0.5f, downOffset;
    [SerializeField] Animator animator;
    [SerializeField] LayerMask layer;
    public RaycastHit2D[] hits;
    public bool Hitting { get; private set; }

    public event OnHitted OnHittedEvent;

    private void Update()
    {
        Vector2 startPos = new Vector2(transform.position.x, transform.position.y + upOffset) + ((animator.GetBool("FlipX") ? -1 : 1) * Vector2.right * forwardOffset);
        Vector2 endPos = startPos + Vector2.down * downOffset;

        var hitted = Physics2D.LinecastAll(startPos,endPos, layer);

        if(hitted.Length > 0)
        {
            Hitting = true;
            hits = hitted;
            OnHittedEvent?.Invoke();
        }
        else
        {
            Hitting = false;
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 startPos = new Vector2(transform.position.x, transform.position.y + upOffset) + (1 * Vector2.right * forwardOffset);
        Vector2 startPos2 = new Vector2(transform.position.x, transform.position.y + upOffset) + (-1 * Vector2.right * forwardOffset);
        Vector2 endPos = startPos + Vector2.down * downOffset;
        Vector2 endPos2 = startPos2 + Vector2.down * downOffset;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPos, endPos);
        Gizmos.DrawLine(startPos2, endPos2);
    }
}
