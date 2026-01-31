using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField]
    UnityEvent OnInteract;

    [SerializeField]
    string character;

    void IInteractable.Interact()
    {
        Debug.Log("INTERACTION");
        OnInteract.Invoke();
    }
}
