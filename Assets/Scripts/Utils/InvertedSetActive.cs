using UnityEngine;

public class InvertedSetActive : MonoBehaviour
{
    public void SetActiveInverted(bool value)
    {
        gameObject.SetActive(!value);
    }
}
