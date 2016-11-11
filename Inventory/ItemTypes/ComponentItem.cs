using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComponentItem : Item{
    public ComponentItem(Item baseItem)
    {
        ID = baseItem.ID;
        Title = baseItem.Title;
        Type = baseItem.Type;
        Value = baseItem.Value;
        Descritption = baseItem.Descritption;
        Rarity = baseItem.Rarity;
        Stackable = false;
        Slug = baseItem.Slug;
        Sprite = Resources.Load<Sprite>(Slug);

        componentTypes = new List<ComponentData>();
    }
}

public class ComponentData{
    public string type;
    public string slug;
    public Sprite sprite;

    public ComponentData(string mainComponent, string typeIn)
    {
        type = typeIn;
        slug = mainComponent + "_" + typeIn;
        sprite = Resources.Load<Sprite>(slug);
    }
}
