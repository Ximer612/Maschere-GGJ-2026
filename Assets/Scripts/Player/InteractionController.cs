using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    IInteractable currentInteractable;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        var maybeInteractable = collider.gameObject.GetComponent<IInteractable>();
        if (maybeInteractable != null)
        {
            currentInteractable = maybeInteractable;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        var maybeInteractable = collider.gameObject.GetComponent<IInteractable>();
        if (maybeInteractable == currentInteractable)
        {
            currentInteractable = null;
        }
    }

    public void CheckInteraction(InputAction.CallbackContext ctx)
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }
}
