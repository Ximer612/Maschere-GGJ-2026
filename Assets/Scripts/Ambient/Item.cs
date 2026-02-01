using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    void IInteractable.Interact()
    {
        Inventory.Singleton.SetMaskStatus(MaskEnum.Arlecchino, MaskStatusEnum.FULL);
        Destroy(gameObject);
    }
}
