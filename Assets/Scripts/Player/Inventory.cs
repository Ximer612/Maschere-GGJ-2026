using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct Mask
{
    public MaskEnum mask;
    public MaskStatusEnum maskStatusEnum;
}

public class Inventory : MonoBehaviour
{
    public Dictionary<MaskEnum, MaskStatusEnum> masks = new Dictionary<MaskEnum, MaskStatusEnum>();
    public List<Mask> readonlyMasksArray;

    public void Awake()
    {
        InitializeMasks();
    }

    void InitializeMasks()
    {
        masks.Add(MaskEnum.Arlecchino, MaskStatusEnum.MISSING);
    }

    public void SetMaskStatus(MaskEnum mask, MaskStatusEnum status)
    {
        masks[mask] = status;
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
