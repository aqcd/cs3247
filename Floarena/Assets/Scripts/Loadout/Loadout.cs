public class Loadout
{
    Item[] items;

    public Loadout(Item item1, Item item2) {
        items = new Item[]{item1, item2};
    }

    public Loadout() {
        items = new Item[]{Data.DEFAULT_ITEMS[0], Data.DEFAULT_ITEMS[1]};
    }

    // Position is 1-indexed.
    public bool SetItem(Item item, int position) {
        if (position > 2 || position < 1) {
            return false;
        }
        if (this.items[0].Equals(item) || this.items[1].Equals(item)) {
            return false;
        }
        this.items[position - 1] = item;
        return true;
    }

    public PlayerStats GetItemNetEffects() {
        PlayerStats stats = Data.ZEROED_ATTRIBUTES.DeepCopy().ApplyItems(items);
        return stats;
    }

    public PlayerStats GetLoadoutStats() {
        PlayerStats stats = Data.BASE_ATTRIBUTES.DeepCopy().ApplyItems(items);
        return stats;
    }
}