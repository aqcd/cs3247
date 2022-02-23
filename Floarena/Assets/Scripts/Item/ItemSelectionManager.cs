using UnityEngine;
public class ItemSelectionManager : MonoBehaviour
{
    public static ItemSelectionManager instance;

    public bool isItemSelected = false;
    public int editingPosition = 1;

    [SerializeField]
    public ItemSelection[] itemSelections;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Start() {
        
    }

    public void SetEditingPosition(int position) {
        this.isItemSelected = true;
        this.editingPosition = position;
    }

    public void SetItem(Item item) {
        if (this.isItemSelected && LoadoutManager.instance.SetItem(item, editingPosition)) {
            this.itemSelections[editingPosition - 1].SetItem(item);
            this.isItemSelected = false;
        }
    }

    public void SetFromLoadout(Loadout loadout) {
        for (int i = 0; i < itemSelections.Length; i++) {
            this.itemSelections[i].SetItem(loadout.items[i]);
        }
        this.isItemSelected = false;
    }
}