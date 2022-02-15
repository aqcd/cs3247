using UnityEngine;
using UnityEngine.UI;

public class ItemSelection : MonoBehaviour
{
    [SerializeField]
    public int position;

    [SerializeField]
    Button button;
    
    [SerializeField]
    Text displayString;

    public Item item;

    void Start() {
        SetItem(Data.DEFAULT_ITEMS[position - 1]);
    }

    public bool SetItem(Item item) {
        this.item = item;
        SetText();
        return true;
    }

    void SetText() {
        displayString.text = this.item.ToString();
    }
}