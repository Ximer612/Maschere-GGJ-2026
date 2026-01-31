using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct Mask
{
    public Mask(MaskEnum iMask, MaskStatusEnum iStatus)
    {
        mask = iMask;
        Status = iStatus;
    }

    public MaskEnum mask;
    public MaskStatusEnum Status;
}

public class Inventory : MonoBehaviour
{
    private static Inventory Singleton;

    public Dictionary<MaskEnum, MaskStatusEnum> masks = new Dictionary<MaskEnum, MaskStatusEnum>();
    public List<Mask> readonlyMasksArray;

    public void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            InitializeMasks();
        }

    }

    void InitializeMasks()
    {
        Debug.Log("Initializing Masks");
        foreach (MaskEnum mask in (MaskEnum[])System.Enum.GetValues(typeof(MaskEnum)))
        {
            masks.Add(mask, MaskStatusEnum.MISSING);
            readonlyMasksArray.Add(new Mask(mask, MaskStatusEnum.MISSING));
        }
    }

    public void SetArlecchinoBroken() { setMaskStatus(MaskEnum.Arlecchino, MaskStatusEnum.BROKEN); }

    private void setMaskStatus(MaskEnum mask, MaskStatusEnum status)
    {
        masks[mask] = status;
        Debug.Log($"Set {mask} to {status}");
    }
}

public enum MaskEnum : int
{
    Arlecchino,
}

public enum MaskStatusEnum : int
{
    MISSING,
    BROKEN,
    FULL,
}
