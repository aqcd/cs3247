public class Loadout
{
    public Item[] items;
    public Skill[] skills;

    public Loadout(Item item1, Item item2, Skill skill1, Skill skill2, Skill skill3) {
        items = new Item[]{item1, item2};
        skills = new Skill[]{skill1, skill2, skill3};
    }

    public Loadout() {
        items = new Item[]{Data.DEFAULT_ITEMS[0], Data.DEFAULT_ITEMS[1]};
        skills = new Skill[]{Data.DEFAULT_BASIC_SKILLS[0], Data.DEFAULT_BASIC_SKILLS[1], Data.DEFAULT_ULTIMATE_SKILLS[0]};
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

    // Position is 1-indexed.
    public bool SetSkill(Skill skill, int position) {
        if (position > 3 || position < 1) {
            return false;
        }
        if (this.skills[0].Equals(skill) || this.skills[1].Equals(skill)) {
            return false;
        }
        this.skills[position - 1] = skill;
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