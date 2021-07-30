using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
