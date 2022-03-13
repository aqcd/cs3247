using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField]
    Button button;
    
    [SerializeField]
    Text displayString;

    public Item item;

    void Start() {
        
    }

    public bool SetItem(Item item) {
        this.item = item;
        displayString.text = item.ToString();
        return true;
    }

    public void SelectItem() {
        SelectionManager.instance.SetItem(this.item);
    }

    public void SetDescription() {
        EditLoadoutUIManager.instance.SetDescription(this.item.GetDescription());
    }

    public void UnsetDescription() {
        EditLoadoutUIManager.instance.UnsetDescription();
    }
}