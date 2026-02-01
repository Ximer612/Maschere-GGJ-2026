using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    void IInteractable.Interact()
    {
        Inventory.Singleton.SetMaskStatus(MaskEnum.Arlecchino, MaskStatusEnum.FULL);
        GameplayManager.Singleton.UpdateMajorSpawnPoint(gameObject.transform.position);
        GameplayManager.Singleton.StartWaterSection();
        Destroy(gameObject);
    }
}
