using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static ItemSlot highlighted = null;

    public int slotNumber;
    public ItemData itemDataObject;
    InventoryManager inventory;

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemData itemDropped = eventData.pointerDrag.GetComponent<ItemData>();

        if (itemDropped != null)
        {
            if (slotNumber == itemDropped.slotNumber)
            {
            }
            else if (inventory.itemInSlot(slotNumber) == null)
            {
                inventory.emptySlot(itemDropped.slotNumber);
                inventory.placeItemInSlot(itemDropped, slotNumber);
            }
            else
            {
                ItemData itemCurrentlyHere = itemDataObject;
                int otherSlot = itemDropped.slotNumber;

                inventory.placeItemInSlot(itemDropped, slotNumber);
                inventory.placeItemInSlot(itemCurrentlyHere, otherSlot);
            }
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        float normailzedColor = 233F / 255F;
        if (highlighted != null)
        {
            highlighted.GetComponent<Image>().color = new Color(normailzedColor, normailzedColor, normailzedColor);
        }
        highlighted = this;

        if(itemDataObject != null){
            GetComponent<Image>().color = new Color(1, 1, 1);
        }
        else
        {
            GetComponent<Image>().color = new Color(220 / 255F, 220 / 255F, 220 / 255F);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        float normailzedColor = 233F / 255F;
        GetComponent<Image>().color = new Color(normailzedColor, normailzedColor, normailzedColor);
        highlighted = null;
    }
}
