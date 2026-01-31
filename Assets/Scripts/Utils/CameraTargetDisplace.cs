using UnityEngine;

public class CameraTargetDisplace : MonoBehaviour
{
    [SerializeField] Vector3 Delta;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("LocalPlayer"))
        {
            Transform CameraTarget = collision.gameObject.transform.Find("CameraTarget");
            if (CameraTarget)
            {
                CameraTarget.localPosition += Delta;
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("LocalPlayer"))
        {
            Transform CameraTarget = collision.gameObject.transform.Find("CameraTarget");
            if (CameraTarget)
            {
                CameraTarget.localPosition -= Delta;
            }

        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.4f, 0f, 0.34f);

        // Convert the local coordinate values into world
        // coordinates for the matrix transformation.
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
#endif

}
