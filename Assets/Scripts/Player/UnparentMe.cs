using UnityEngine;

public class UnparentMe : MonoBehaviour
{
    private void Awake()
    {
        transform.SetParent(null);
    }
}
