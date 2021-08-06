using System;
using System.Collections.Generic;

public class Inventory
{
    public event EventHandler OnItemListChanged;

    private List<Item> itemList;

    public Inventory()
    {
        itemList = new List<Item>();
        AddItem(new Item { itemType = Item.ItemType.XPipe, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.IPipe, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.TPipe, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.LPipe, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.LPipe, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.LPipe, amount = 1 });


    }
    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if (item.itemType == inventoryItem.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory)
            {
                itemList.Add(item);
            }
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}
