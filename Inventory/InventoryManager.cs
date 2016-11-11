using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public GameObject inventoryPanel;
    public GameObject slotPanel;
    public GameObject slotPrefab;
    public GameObject itemPrefab;

    public int numSlots;
    [HideInInspector]
    public List<GameObject> slots = new List<GameObject>();

    ItemDatabase database;

    public ItemData dragging;

    void Start()
    {
        database = GetComponent<ItemDatabase>();
        for (int i = 0; i < numSlots; i++)
        {
            slots.Add(Instantiate(slotPrefab));
            slots[i].transform.SetParent(slotPanel.transform);
            slots[i].GetComponent<ItemSlot>().slotNumber = i;
            slots[i].GetComponent<ItemSlot>().itemDataObject = null;
        }

        addItem(1);//Debug
        addItem(0);
    }

    int debug;
    bool dumbHack = true;
    void Update()
    {
        if (Time.time > 1 && debug == 0)
        {
            addItem(3);
            debug++;
        }
        else if (Time.time > 4 && debug == 1)
        {
            addItem(3, 50);
            addItem(4, 3);
            debug++;
        }
        else if (Time.time > 7 && debug == 2)
        {
            addItem(3, 500);
            addItem(5, 3);
            debug++;
        }
        else if (Time.time > 10 && debug == 3)
        {
            addItem(3, 2000);
            addItem(6, 3);

            Item specialEdition = new ComponentItem(database.FetchItemWithID(5));
            addItem(specialEdition);
            specialEdition.Value = 10;
            addItem(specialEdition);

            debug++;
        }
        else if (Time.time > 13 && debug == 4)
        {
            addItem(3, 5000);
            addItem(5);
            addItem(6);
            addItem(4);
            debug++;
        }
        else if (Time.time > 16 && debug == 5)
        {
            addItem(3, 10000);
            addItem(1);
            addItem(3);
            addItem(0);
            debug++;
        }

        if (dumbHack && Time.time > 1)
        {
            GameObject test = GameObject.FindGameObjectWithTag("InventoryPanel").transform.GetChild(0).gameObject;
            Destroy(test.GetComponent<GridLayoutGroup>());
            dumbHack = false;
        }
    }

    int firstEmptySlot()
    {
        for (int i = 0; i < numSlots; i++)
        {
            if (itemInSlot(i) == null)
            {
                return i;
            }
        }

        Functions.ErrorMessage("Inventory is full!");
        return -1;
    }

    int firstSlotWithItem(Item thisItem)
    {
        for (int i = 0; i < numSlots; i++)
        {
            if (itemInSlot(i) != null)
            {
                if (itemInSlot(i).thisItem.Equals(thisItem))
                {
                    return i;
                }
            }
        }

        return -1;
    }

    public void addItem(Item addingItem)
    {
        addItem(addingItem, 1);
    }

    public void addItem(int id)
    {
        addItem(database.FetchItemWithID(id));
    }

    public void addItem(int id, int amount)
    {
        addItem(database.FetchItemWithID(id), amount);
    }

    public void addItem(Item addingItem, int amount)
    {
        if (addingItem.Stackable)
        {
            int possibleSlot = firstSlotWithItem(addingItem);
            if (possibleSlot != -1)
            {
                slots[possibleSlot].GetComponent<ItemSlot>().itemDataObject.Count += amount;
                return;
            }

            int emptySlot = firstEmptySlot();
            if (emptySlot != -1)
            {
                itemSetup(addingItem, amount, emptySlot);
                return;
            }
        }

        for (int i = 0; i < amount; i++)
        {
            int emptySlot = firstEmptySlot();
            if (emptySlot != -1)
            {
                itemSetup(addingItem, 1, emptySlot);
            }
        }
    }

    void itemSetup(Item addingItem, int amount, int slot)
    {
        GameObject itemVisual = Instantiate(itemPrefab);
        itemVisual.GetComponent<ItemData>().Count = amount;
        itemVisual.GetComponent<ItemData>().thisItem = addingItem;
        itemVisual.GetComponent<Image>().sprite = addingItem.Sprite;
        itemVisual.transform.SetParent(slots[slot].transform);
        itemVisual.transform.position = itemVisual.transform.parent.position;
        placeItemInSlot(itemVisual.GetComponent<ItemData>(), slot);
    }

    public void placeItemInSlot(ItemData item, int slotNumber)
    {
        slots[slotNumber].GetComponent<ItemSlot>().itemDataObject = item;
        item.slotNumber = slotNumber;
        item.transform.SetParent(slots[slotNumber].transform);
        if (Time.time > 2)
        {
            slots[slotNumber].transform.SetAsLastSibling();
        }
    }

    public ItemData itemInSlot(int slotNumber)
    {
        return slots[slotNumber].GetComponent<ItemSlot>().itemDataObject;
    }

    public void emptySlot(int slotNumber)
    {
        slots[slotNumber].GetComponent<ItemSlot>().itemDataObject = null;
    }

}
