using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;

public class ItemDatabase : MonoBehaviour {
    private List<Item> dataBase = new List<Item>();
    private JsonData itemData;

    void Start()
    {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        ConstructItemDatabase();
    }

    void ConstructItemDatabase()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            dataBase.Add(new Item((int)itemData[i]["id"], itemData[i]["title"].ToString(), itemData[i]["type"].ToString(),
                (int)itemData[i]["value"], itemData[i]["description"].ToString(), (int)itemData[i]["rarity"], 
                (bool)itemData[i]["stackable"], itemData[i]["slug"].ToString()));
            if (dataBase[i].Type == "Weapon")
            {
                WeaponItem newWeapon = new WeaponItem(dataBase[i], itemData[i]["components"]["1"].ToString(), 
                    itemData[i]["components"]["2"].ToString(), itemData[i]["components"]["3"].ToString(), 
                    (int)itemData[i]["stats"]["moveSpeed"], (int)itemData[i]["stats"]["damage"], (int)itemData[i]["stats"]["range"], 
                    (bool)itemData[i]["droppable"]);
                dataBase[i] = newWeapon;
            }
            else if (dataBase[i].Type == "Component")
            {
                ComponentItem newComponent = new ComponentItem(dataBase[i]);
                for (int j = 0; j < itemData[i]["parts"].Count; j++ )
                {
                    ComponentData newComponentData = new ComponentData(dataBase[i].Slug, itemData[i]["parts"][j].ToString());
                    newComponent.componentTypes.Add(newComponentData);
                }
                dataBase[i] = newComponent;
            }
        }
    }

    public Item FetchItemWithID(int id)
    {
        for (int i = 0; i < dataBase.Count; i++)
        {
            if (dataBase[i].ID == id)
            {
                return dataBase[i];
            }
        }

        return null;
    }
}

public class Item
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
    public int Value { get; set; }
    public string Descritption { get; set; }
    public int Rarity { get; set; }
    public bool Stackable { get; set; }
    public string Slug { get; set; }
    public Sprite Sprite { get; set; }
    public bool Craftable { get; set; }
    public List<ComponentData> componentTypes;

    public Item(int idIn, string titleIn, string typeIn, int valueIn, string descritpionIn, int rarityIn, bool stackableIn, string slugIn)
    {
        ID = idIn;
        Title = titleIn;
        Type = typeIn;
        Value = valueIn;
        Descritption = descritpionIn;
        Rarity = rarityIn;
        Stackable = stackableIn;
        Slug = slugIn;
        Sprite = Resources.Load<Sprite>(Slug);
        Craftable = false;
    }

    public Item()
    {
        ID = -1;
    }
}
