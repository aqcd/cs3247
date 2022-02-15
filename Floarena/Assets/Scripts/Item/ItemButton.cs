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
        ItemSelectionManager.instance.SetItem(this.item);
    }
}