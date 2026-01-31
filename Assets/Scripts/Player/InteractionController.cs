using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    [SerializeField] Camera playerCamera;

    [SerializeField] float interactionDistance = 0.5f;

    IInteractable currentInteractable;

    public void Update()
    {
        UpdateCurrentInteractable();

        CheckInteraction();
    }

    void UpdateCurrentInteractable()
    {
        var ray = playerCamera.ViewportPointToRay(new Vector2(0.5f, 0.5f));

        Physics.Raycast(ray, out var hit, interactionDistance);

        currentInteractable = hit.collider?.GetComponent<IInteractable>();
    }

    void CheckInteraction()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

}
