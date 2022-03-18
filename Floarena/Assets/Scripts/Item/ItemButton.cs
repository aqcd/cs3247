using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField]
    Button button;
    
    [SerializeField]
    Image displayImage;

    public Item item;

    void Start() {
        
    }

    public bool SetItem(Item item) {
        this.item = item;
        displayImage.sprite = SelectionManager.instance.itemSprites[item.name];
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