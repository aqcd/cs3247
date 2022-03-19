using UnityEngine;
using UnityEngine.UI;

public class LoadoutManager : MonoBehaviour
{
    public static LoadoutManager instance;

    [SerializeField]
    Button[] buttons;

    Loadout[] savedLoadouts;

    Loadout loadout;
    int loadoutIdx;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }
    
    void Start() {
        this.loadout = new Loadout();
        this.loadoutIdx = 0;
        this.savedLoadouts = new Loadout[]{new Loadout(), new Loadout(), new Loadout(), new Loadout(), new Loadout(), new Loadout()};
        this.savedLoadouts[this.loadoutIdx] = this.loadout;
        FillButton();
    }

    // Position is 1-indexed.
    public bool SetItem(Item item, int position) {
        bool ret = this.loadout.SetItem(item, position);
        savedLoadouts[loadoutIdx] = loadout;
        return ret;
    }

    // Position is 1-indexed.
    public bool SetSkill(Skill skill, int position) {
        bool ret = this.loadout.SetSkill(skill, position);
        savedLoadouts[loadoutIdx] = loadout;
        return ret;
    }

    public void SetLoadoutIdx(int idx) {
        UnfillButton();
        if (idx < 0 || idx > 6) {
            return;
        }
        this.loadoutIdx = idx;
        this.loadout = this.savedLoadouts[this.loadoutIdx];
        SelectionManager.instance.SetFromLoadout(this.loadout);
        FillButton();
        return;
    }

    public Loadout GetLoadout() {
        return loadout;
    }

    public float GetAttributeValue(Attribute attribute) {
        PlayerStats stats = loadout.GetLoadoutStats();
        return stats.GetAttributeValue(attribute);
    }

    void FillButton() {
        buttons[this.loadoutIdx].GetComponent<Image>().color = Color.yellow;
    }

    void UnfillButton() {
        buttons[this.loadoutIdx].GetComponent<Image>().color = Color.white;
    }
}
