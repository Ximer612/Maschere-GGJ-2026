using UnityEngine;

public class EdgeGrabbing : MonoBehaviour
{
    [SerializeField] float forwardOffset, upOffset = 0.5f, downOffset;
    [SerializeField] Animator animator;
    [SerializeField] LayerMask layer;
    [SerializeField] float standOffsetY = 0.05f;
    [SerializeField] Collider2D playerCollider;
    [SerializeField] GameObject prefabTest;

    public bool Hitting { get; private set; }
    public Vector2 standPoint { get; private set; }


    private void Update()
    {
        Vector2 dir = animator.GetBool("FlipX") ? Vector2.left : Vector2.right;

        Vector2 startPos = new Vector2(transform.position.x, transform.position.y + upOffset) + (dir * Vector2.right * forwardOffset);
        Vector2 endPos = startPos + Vector2.down * downOffset;

        var hitted = Physics2D.LinecastAll(startPos, endPos, layer);

        if (hitted.Length > 0)
        {
            standPoint = hitted[0].point;

            if (CanStandOnPoint())
            {
                //Instantiate(prefabTest, standPoint, Quaternion.identity);

                Hitting = true;
                return;
            }

            //Hitting = true;
            //hits = hitted;
            //OnHittedEvent?.Invoke();
        }
        else
        {
            Hitting = false;
        }
    }

    bool CanStandOnPoint()
    {
        Bounds b = playerCollider.bounds;

        Vector2 targetCenter = standPoint + Vector2.up * (b.extents.y + standOffsetY);

        var hit = Physics2D.OverlapBox(targetCenter, b.size, 0f, layer);

        standPoint = targetCenter;

        return hit == null;
    }

    void OnDrawGizmos()
    {
        if (!playerCollider) return;

        Gizmos.color = Color.yellow;

        Bounds b = playerCollider.bounds;
        Vector2 p = transform.position;

        Gizmos.DrawWireCube(p, b.size);

        Vector2 startPos = new Vector2(transform.position.x, transform.position.y + upOffset) + Vector2.right * forwardOffset;
        Vector2 startPos2 = new Vector2(transform.position.x, transform.position.y + upOffset) + Vector2.left * forwardOffset;

        Vector2 endPos = startPos + Vector2.down * downOffset;
        Vector2 endPos2 = startPos2 + Vector2.down * downOffset;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPos, endPos);
        Gizmos.DrawLine(startPos2, endPos2);
    }
}
