using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemButtonManager : MonoBehaviour
{
    public static ItemButtonManager instance;

    bool visibility = false;

    List<Button> itemButtons;

    [SerializeField]
    GameObject itemButtonPrefab;

    [SerializeField]
    GameObject parentElement;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Start() {
        itemButtons = new List<Button>();
        foreach (Item item in Data.ITEMS) {
            GameObject go = Instantiate(itemButtonPrefab, parentElement.transform.position, parentElement.transform.rotation);
            Button button = go.GetComponent<Button>();
            button.transform.SetParent(parentElement.transform, false);
            ItemButton itemButton = go.GetComponent<ItemButton>();
            itemButton.SetItem(item);

            GameObject textGo = button.transform.Find("Text").gameObject;
            Text text = textGo.GetComponent<Text>();
            text.text = item.ToString();

            this.itemButtons.Add(button);
        }
        HideItems();
    }

    void Update() {
        if (ItemSelectionManager.instance.isItemSelected && !visibility) {
            ShowItems();
            visibility = true;
        }
        if (!ItemSelectionManager.instance.isItemSelected && visibility) {
            HideItems();
            visibility = false;
        }
    }

    void ShowItems() {
        foreach (Button button in itemButtons) {
            button.gameObject.SetActive(true);
        }
    }

    void HideItems() {
        foreach (Button button in itemButtons) {
            button.gameObject.SetActive(false);
        }
    }
}