public class Item
{
    string name;

    public Effect[] effects;

    public Item(string name, Effect[] effects) {
        this.name = name;
        this.effects = effects;
    }

    override public string ToString() {
        return this.name;
    }

    public bool Equals(Item item) {
        return this.name == item.name;
    }
}
