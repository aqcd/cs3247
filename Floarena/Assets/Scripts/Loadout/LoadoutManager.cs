using UnityEngine;

public class LoadoutManager : MonoBehaviour
{
    public static LoadoutManager instance;

    Loadout loadout;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }
    
    void Start() {
        loadout = new Loadout();
    }

    // Position is 1-indexed.
    public bool SetItem(Item item, int position) {
        return this.loadout.SetItem(item, position);
    }

    public Loadout GetLoadout() {
        return loadout;
    }

    public double GetAttributeValue(Attribute attribute) {
        PlayerStats stats = loadout.GetLoadoutStats();
        return stats.GetAttributeValue(attribute);
    }
}
