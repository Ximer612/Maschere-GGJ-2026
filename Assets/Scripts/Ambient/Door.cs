using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    string IInteractable.InteractMessage => $"Parla con {character}";

    [SerializeField]
    string character;

    void IInteractable.Interact()
    {
        Debug.Log($"Ciao sono {character}");
    }
}
