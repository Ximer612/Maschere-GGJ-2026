using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct Mask
{
    public Mask(MaskEnum iMask, MaskStatusEnum iStatus)
    {
        mask = iMask;
        status = iStatus;
    }

    public MaskEnum mask;
    public MaskStatusEnum status;
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

    public void SetMaskStatus(MaskEnum mask, MaskStatusEnum status)
    {
        masks[mask] = status;
        var arrayMask = readonlyMasksArray.Find((aMask) => aMask.mask == mask);
        arrayMask.mask = mask;
        arrayMask.status = status;
    }
}

public enum MaskEnum
{
    Arlecchino,
}

public enum MaskStatusEnum
{
    MISSING,
    BROKEN,
    FULL,
}
