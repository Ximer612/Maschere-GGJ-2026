using UnityEngine;

public class SubtitleTrigger : SubtitleTriggerer
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasPlayedOnce)
            return;

        TriggerSubtitle();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.36f, 0.52f, 0.52f, 0.34f);

        // Convert the local coordinate values into world
        // coordinates for the matrix transformation.
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
#endif

}
