using System;
using UnityEngine;

public class BoxGroundController : MonoBehaviour
{
    public GameObject GoundObject { get; private set; }
    public bool IsGrounded { get; private set; }
    bool wasGroundedLastFrame = false;

    [SerializeField] Vector2 boxSize, offset;
    [SerializeField] float castDistance = 0.5f;
    [SerializeField] LayerMask mask;

    public Action OnNewLand;

    void Update()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(mask);
        filter.useTriggers = false;

        RaycastHit2D[] hits = new RaycastHit2D[10];
        int results = Physics2D.BoxCast(transform.position + new Vector3(offset.x, offset.y, 0f), boxSize, 0, Vector2.down, filter, hits, castDistance);

        //Debug.DrawLine(transform.position + new Vector3(offset.x, offset.y, 0f), transform.position + new Vector3(0, -0.5f, 0), Color.yellow);

        IsGrounded = results > 0;

        if (IsGrounded)
        {
            GoundObject = hits[0].collider.gameObject;
        }

        if (wasGroundedLastFrame != IsGrounded && IsGrounded)
        {
            //dust?
            OnNewLand?.Invoke();
        }

        wasGroundedLastFrame = IsGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(offset.x, offset.y, 0f) + transform.position + Vector3.down * castDistance, boxSize);
    }
}
