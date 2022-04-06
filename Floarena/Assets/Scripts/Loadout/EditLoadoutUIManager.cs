using UnityEngine;
using UnityEngine.UI;
using System;

public class EditLoadoutUIManager : MonoBehaviour
{
    public static EditLoadoutUIManager instance;

    [SerializeField]
    Text hpTextComponent, adTextComponent, asTextComponent, arTextComponent, msTextComponent;

    [SerializeField]
    Text descriptionComponent;

    private Color green = new Color(0.007075459f, 0.5f, 0.1753192f, 1);

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        UnsetDescription();
    }

    void Update() {
        Set(Attribute.HP, hpTextComponent);
        Set(Attribute.AD, adTextComponent);
        Set(Attribute.AS, asTextComponent);
        Set(Attribute.AR, arTextComponent);
        Set(Attribute.MS, msTextComponent);
    }

    void Set(Attribute attribute, Text component) {
        float val = LoadoutManager.instance.GetAttributeValue(attribute);
        float increase = val - Data.BASE_ATTRIBUTES.GetAttributeValue(attribute);
        component.text = val.ToString();
        if (increase > 0.0) {
            component.text = component.text + " (+" + increase.ToString("#0.0") + ")";
            component.GetComponent<Text>().color = green;
        } else if (increase < 0.0) {
            component.text = component.text + " (-" + Math.Abs(increase).ToString("#0.0") + ")";
            component.GetComponent<Text>().color = Color.red;
        } else {
            component.GetComponent<Text>().color = Color.black;
        }
    }

    public void SetDescription(string description) {
        descriptionComponent.text = description;
    }

    public void UnsetDescription() {
        descriptionComponent.text = "";
    }
}
