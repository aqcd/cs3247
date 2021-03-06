using System.Collections.Generic;

public class PlayerStats
{
    Dictionary<Attribute, float> stats;

    public PlayerStats() {
        this.stats = new Dictionary<Attribute, float>{
        { Attribute.HP, 0 },
        { Attribute.AD, 0 },
        { Attribute.AS, 0 },
        { Attribute.AR, 0 },
        { Attribute.MS, 0 },
        };
    }

    public PlayerStats(Dictionary<Attribute, float> stats) {
        this.stats = stats;
    }

    public float GetAttributeValue(Attribute attribute) {
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

    public void ResetAttribute(Attribute attribute) {
        stats[attribute] = 0.0f;
    }

    public void ApplyEffect(Effect effect) {
        float current = stats[effect.attribute];
        float modified = current + effect.modifier;
        stats[effect.attribute] = modified;
    }

    public PlayerStats DeepCopy() {
        Dictionary<Attribute, float> deepCopyStats = new Dictionary<Attribute, float>();
        foreach (KeyValuePair<Attribute, float> kv in stats) {
            deepCopyStats[kv.Key] = kv.Value;
        }
        return new PlayerStats(deepCopyStats);
    }
}