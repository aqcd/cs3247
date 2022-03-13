using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager instance;

    List<Button> itemButtons;
    List<Button> basicSkillbuttons;
    List<Button> ultimateSkillbuttons;

    [SerializeField]
    GameObject itemButtonPrefab;
    
    [SerializeField]
    GameObject skillButtonPrefab;

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

        basicSkillbuttons = new List<Button>();
        foreach (Skill skill in Data.BASIC_SKILLS) {
            GameObject go = Instantiate(skillButtonPrefab, parentElement.transform.position, parentElement.transform.rotation);
            Button button = go.GetComponent<Button>();
            button.transform.SetParent(parentElement.transform, false);
            SkillButton skillButton = go.GetComponent<SkillButton>();
            skillButton.SetSkill(skill);

            GameObject textGo = button.transform.Find("Text").gameObject;
            Text text = textGo.GetComponent<Text>();
            text.text = skill.ToString();

            this.basicSkillbuttons.Add(button);
        }

        ultimateSkillbuttons = new List<Button>();
        foreach (Skill skill in Data.ULTIMATE_SKILLS) {
            GameObject go = Instantiate(skillButtonPrefab, parentElement.transform.position, parentElement.transform.rotation);
            Button button = go.GetComponent<Button>();
            button.transform.SetParent(parentElement.transform, false);
            SkillButton skillButton = go.GetComponent<SkillButton>();
            skillButton.SetSkill(skill);

            GameObject textGo = button.transform.Find("Text").gameObject;
            Text text = textGo.GetComponent<Text>();
            text.text = skill.ToString();

            this.ultimateSkillbuttons.Add(button);
        }
        HideItems();
        HideBSkills();
        HideUSkills();
    }

    void Update() {
        if (SelectionManager.instance.isItemSelected) {
            ShowItems();
            HideBSkills();
            HideUSkills();
            return;
        }
        if (SelectionManager.instance.isBSkillSelected) {
            ShowBSkills();
            HideItems();
            HideUSkills();
            return;
        }
        if (SelectionManager.instance.isUSkillSelected) {
            ShowUSkills();
            HideItems();
            HideBSkills();
            return;
        }
        HideItems();
        HideBSkills();
        HideUSkills();
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

    void ShowBSkills() {
        foreach (Button button in basicSkillbuttons) {
            button.gameObject.SetActive(true);
        }
    }

    void HideBSkills() {
        foreach (Button button in basicSkillbuttons) {
            button.gameObject.SetActive(false);
        }
    }

    void ShowUSkills() {
        foreach (Button button in ultimateSkillbuttons) {
            button.gameObject.SetActive(true);
        }
    }

    void HideUSkills() {
        foreach (Button button in ultimateSkillbuttons) {
            button.gameObject.SetActive(false);
        }
    }
}