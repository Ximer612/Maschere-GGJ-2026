using UnityEngine;

public class RenderCollisionBox : MonoBehaviour
{
#if UNITY_EDITOR

    [SerializeField] Color boxColor = new Color(1f, 1f, 1f, 0.34f);

    private void OnDrawGizmos()
    {
        Gizmos.color = boxColor;

        // Convert the local coordinate values into world
        // coordinates for the matrix transformation.
        Gizmos.matrix = transform.localToWorldMatrix;

        var col = GetComponent<BoxCollider2D>();
        if (col)
        {
            Gizmos.DrawCube(col.offset, col.size);
        }
        else
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
#endif
}
