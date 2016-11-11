using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerExitHandler, 
    IPointerEnterHandler, IPointerDownHandler{

    GameObject inventoryPanel;
    ToolTip toolTip;
    InventoryManager inventoryManager;
    
    Text countText;
    private int count;
    public int Count
    {
        get
        {
            return count;
        }
        set
        {
            count = value;
            if (count > 1)
            {
                if (countText != null)
                {
                    countText.text = count.ToString();
                }
            }
            else
            {
                if (countText != null)
                {
                    countText.text = "";
                }
            }
        }
    }

    //todo public int slot;
    public Item thisItem;
    public int slotNumber;

    private Vector2 offset;
    bool dragging;

    //Constuctor
    public ItemData()
    {
        thisItem = new Item();
        slotNumber = 0;
    }

    void Start()
    {
        countText = transform.GetChild(0).GetComponent<Text>();
        inventoryPanel = GameObject.FindGameObjectWithTag("InventoryPanel");
        inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
        toolTip = GameObject.FindGameObjectWithTag("ToolTip").GetComponent<ToolTip>();
        Count = Count;
    }

    void Update()
    {
        if (!dragging)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.parent.position, 500 * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, transform.parent.position) < 1)
        {
            transform.position = transform.parent.position;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(thisItem.ID != -1){
            transform.SetParent(inventoryPanel.transform);
            transform.position = (Vector2)Input.mousePosition + offset;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            dragging = true;
            inventoryManager.dragging = this;
            transform.position = eventData.position + offset;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position + offset;
        if (offset.magnitude > 25)
        {
            Debug.Log(offset.magnitude);
            offset /= 1.1F;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(thisItem.ID != -1 && slotNumber != -1)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        dragging = false;
        inventoryManager.dragging = null;

        if (slotNumber != -1)
        {
            inventoryManager.placeItemInSlot(this, slotNumber);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!inventoryManager.dragging)
        {
            toolTip.Activate(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.Deactivate();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        offset = transform.position - Input.mousePosition;
    }
}
