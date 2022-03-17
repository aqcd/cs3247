using UnityEngine;
using UnityEngine.UI;

public class ItemSelection : MonoBehaviour
{
    [SerializeField]
    public int position;

    [SerializeField]
    Button button;
    
    [SerializeField]
    Image displayImage;

    public Item item;

    void Start() {
        SetItem(Data.DEFAULT_ITEMS[position - 1]);
    }

    public bool SetItem(Item item) {
        this.item = item;
        SetImage();
        return true;
    }

    void SetImage() {
        displayImage.sprite = SelectionManager.instance.itemSprites[item.name];
    }
}