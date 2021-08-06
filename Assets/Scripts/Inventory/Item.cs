using UnityEngine;
using System;

// Make items a scriptable object that stores the sprite and other values
// The switch statments are not simple to scale up

[Serializable]
public class Item
{
    public enum ItemType
    {
        XPipe,
        IPipe,
        LPipe,
        TPipe
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.XPipe: return ItemAssets.Instance.XPipeSprite;
            case ItemType.TPipe: return ItemAssets.Instance.TPipeSprite;
            case ItemType.LPipe: return ItemAssets.Instance.LPipeSprite;
            case ItemType.IPipe: return ItemAssets.Instance.IPipeSprite;
        }
    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            case ItemType.XPipe:
            case ItemType.LPipe:
                return true;
            default:
            case ItemType.IPipe:
            case ItemType.TPipe:
                return false;
        }
    }
}
