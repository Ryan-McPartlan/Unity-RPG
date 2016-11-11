using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour {

    ItemData item;
    Text text;
    string data;

    bool active;

    void Start()
    {
        text = transform.GetChild(0).GetComponent<Text>();
    }

    void Update()
    {
        if(active){
            UpdatePosition();
        }
    }

    public void Activate(ItemData itemIn){
        item = itemIn;
        CreateString();
        UpdatePosition();
        active = true;
    }

    public void Activate(string textIn)
    {
        text.text = textIn;
        UpdatePosition();
        active = true;
    }

    public void Deactivate()
    {
        transform.position = new Vector2(9999, 9999);
        active = false;
        item = null;
    }

    void UpdatePosition()
    {
        if (Input.mousePosition.x > Screen.width / 2)
        {
            GetComponent<RectTransform>().pivot = new Vector2(1.1F, GetComponent<RectTransform>().pivot.y);
        }
        else
        {
            GetComponent<RectTransform>().pivot = new Vector2(-0.15F, GetComponent<RectTransform>().pivot.y);
        }

        if (Input.mousePosition.y > Screen.height / 2)
        {
            GetComponent<RectTransform>().pivot = new Vector2(GetComponent<RectTransform>().pivot.x, .9F);
        }
        else
        {
            GetComponent<RectTransform>().pivot = new Vector2(GetComponent<RectTransform>().pivot.x, .3F);
        }

        transform.position = Input.mousePosition;
    }

    public void CreateString()
    {
        data = "";

        if (item.thisItem.Type == "Gold")
        {
            data = SetGoldString();
        }
        else
        {
            string rarity;

            switch (item.thisItem.Rarity)
            {
                case 0: data += "<color=#9d9d9d>";
                    rarity = "Worthless";
                    break;
                case 1: data += "<color=#ffffff>";
                    rarity = "Common";
                    break;
                case 2: data += "<color=#1eff00>";
                    rarity = "Uncommon";
                    break;
                case 3: data += "<color=#0070dd>";
                    rarity = "Rare";
                    break;
                case 4: data += "<color=#a335ee>";
                    rarity = "Epic";
                    break;
                case 5: data += "<color=#ff8000>";
                    rarity = "Legendary";
                    break;
                default: data += "<color=#000000>";
                    rarity = "Glitched";
                    break;
            }
            data += "<b>" + item.thisItem.Title + "</b>\n";
            data += rarity + " " + item.thisItem.Type + "</color>\n\n";
            if(item.thisItem.Type == "Weapon"){
                data += addWeaponData((WeaponItem)item.thisItem);
            }
            else if (item.thisItem.Type == "Component")
            {
                data += addComponentData((ComponentItem)item.thisItem);
            }
            data += item.thisItem.Descritption;
        }

        text.text = data;
    }

    string addWeaponData(WeaponItem item){
        string newString = "<color=#ff0066>";
        newString += "Base Damage: " + item.damage + "\n";
        newString += "Maximum Range: " + item.maxRange + "\n";

        if (item.moveModifier > 0)
        {
            newString += "Movement Modifier: +" + item.moveModifier + "%\n";
        }
        if (item.moveModifier < 0)
        {
            newString += "Movement Modifier: " + item.moveModifier + "%\n";
        }

        newString += "</color>\n";
        return newString;
    }

    string addComponentData(ComponentItem item)
    {
        string newString = "<color=#ff0066>";

        for (int i = 0; i < item.componentTypes.Count; i++)
        {
            newString += item.componentTypes[i].slug + "\n";
        }

            newString += "</color>\n";
        return newString;
    }

    string SetGoldString()
    {
        if(item.Count > 10000){
            data += "<color=#ff8000><b>" + item.thisItem.Title + "</b></color>\n\n";
            data += "Saving up for a party hat?";
        }
        else if(item.Count > 5000){
            data += "<color=#a335ee><b>" + item.thisItem.Title + "</b></color>\n\n";
            data += "A towering mountain of coins";
        }
        else if (item.Count > 2000)
        {
            data += "<color=#0070dd><b>" + item.thisItem.Title + "</b></color>\n\n";
            data += "A heaving sack of coins";
        }
        else if (item.Count > 500)
        {
            data += "<color=#1eff00><b>" + item.thisItem.Title + "</b></color>\n\n";
            data += "A respectable pouch of coins";
        }
        else if (item.Count > 50)
        {
            data += "<color=#ffffff><b>" + item.thisItem.Title + "</b></color>\n\n";
            data += "A small pouch of coins";
        }
        else
        {
            data += "<color=#9d9d9d><b>" + item.thisItem.Title + "</b></color>\n\n";
            data += "Some loose change";
        }

        return data;
    }
}
