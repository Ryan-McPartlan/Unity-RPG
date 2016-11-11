using UnityEngine;
using System.Collections;

public class WeaponItem : Item {

    public int moveModifier;
    public int damage;
    public int maxRange;
    public bool droppable;

    string[] componentKey = new string[3];
    public ComponentItem baseComponent;
    public ComponentItem secondaryComponent;
    public ComponentItem tertiaryComponent;

    public WeaponItem(Item baseItem, string componentKey1, string componentKey2, string componentKey3, int moveIn, int damageIn, int rangeIn, bool droppableIn)
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

        ComponentItem[] defaultComponents = WeaponController.GetDefaultComponents(Slug);
        baseComponent = defaultComponents[0];
        secondaryComponent = defaultComponents[1];
        tertiaryComponent = defaultComponents[2];

        componentKey[0] = componentKey1;
        componentKey[1] = componentKey2;
        componentKey[2] = componentKey3;
        moveModifier = moveIn;
        damage = damageIn;
        maxRange = rangeIn;
        droppable = droppableIn;

        Craftable = true;
    }

    //FightingUnit will call for its favorite of the two attacks based on range and LOS
    //weaponController.Instantiateprojectile(this, targetlocation, creator), set the fighters cooldowns
    //weaponController.InstantiateProjectile2(this, targetLocation, creator)

}
