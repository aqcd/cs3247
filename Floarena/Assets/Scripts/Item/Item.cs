using UnityEngine;
public class Item
{
    public string name;

    public Effect[] effects;

    public Item(string name, Effect[] effects) {
        this.name = name;
        this.effects = effects;
    }

    override public string ToString() {
        return this.name;
    }

    public string GetDescription() {
        string des = "";
        for (int i = 0; i < effects.Length; i++) {
            des = des + effects[i].modifier + " " + effects[i].attribute + ". ";
        }
        return des;
    }

    public bool Equals(Item item) {
        return this.name == item.name;
    }
}
