using UnityEngine;
using UnityEngine.UI;

public class EditLoadoutUIManager : MonoBehaviour
{
    public static EditLoadoutUIManager instance;

    [SerializeField]
    Text hpTextComponent, adTextComponent, asTextComponent, arTextComponent, msTextComponent;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Update() {
        SetLoadoutHP();
        SetLoadoutAD();
        SetLoadoutAS();
        SetLoadoutAR();
        SetLoadoutMS();
    }

    public void SetLoadoutHP() {
        double hpValue = LoadoutManager.instance.GetAttributeValue(Attribute.HP);
        hpTextComponent.text = hpValue.ToString();
    }

    public void SetLoadoutAD() {
        double adValue = LoadoutManager.instance.GetAttributeValue(Attribute.AD);
        adTextComponent.text = adValue.ToString();
    }

    public void SetLoadoutAS() {
        double asValue = LoadoutManager.instance.GetAttributeValue(Attribute.AS);
        asTextComponent.text = asValue.ToString();
    }

    public void SetLoadoutAR() {
        double arValue = LoadoutManager.instance.GetAttributeValue(Attribute.AR);
        arTextComponent.text = arValue.ToString();
    }

    public void SetLoadoutMS() {
        double msValue = LoadoutManager.instance.GetAttributeValue(Attribute.MS);
        msTextComponent.text = msValue.ToString();
    }
}
