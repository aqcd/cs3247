using System.Collections.Generic;

public class PlayerStats
{
    Dictionary<Attribute, double> stats;

    public PlayerStats(Dictionary<Attribute, double> stats) {
        this.stats = stats;
    }

    public double GetAttributeValue(Attribute attribute) {
        return this.stats[attribute];
    }

    public PlayerStats ApplyItems(Item[] items) {
        foreach (Item item in items) {
            ApplyItem(item);
        }
        return this;
    }

    void ApplyItem(Item item) {
        foreach (Effect effect in item.effects) {
            ApplyEffect(effect);
        }
    }

    void ApplyEffect(Effect effect) {
        double current = stats[effect.attribute];
        double modified = current + effect.modifier;
        stats[effect.attribute] = modified;
    }

    public PlayerStats DeepCopy() {
        Dictionary<Attribute, double> deepCopyStats = new Dictionary<Attribute, double>();
        foreach (KeyValuePair<Attribute, double> kv in stats) {
            deepCopyStats[kv.Key] = kv.Value;
        }
        return new PlayerStats(deepCopyStats);
    }
}