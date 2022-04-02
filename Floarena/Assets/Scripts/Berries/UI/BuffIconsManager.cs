using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BuffIconsManager : MonoBehaviour
{
    public static BuffIconsManager instance;

    [SerializeField]
    GameObject ADBuffIcon, ASBuffIcon, MSBuffIcon;

    [SerializeField]
    GameObject parentElement;

    public bool hasADBuff, hasASBuff, hasMSBuff;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Start() {
        
    }

    void Update() {
        
    }

    public void SetBuff(Attribute attribute) {
        GameObject go;
        Image image;
        switch (attribute) {
            case Attribute.AD:
                if (hasADBuff) { break; }
                hasADBuff = true;
                go = Instantiate(ADBuffIcon, parentElement.transform.position, parentElement.transform.rotation);
                image = go.GetComponent<Image>();
                image.transform.SetParent(parentElement.transform, false);
                break;
            case Attribute.AS:
                if (hasASBuff) { break; }
                hasASBuff = true;
                go = Instantiate(ASBuffIcon, parentElement.transform.position, parentElement.transform.rotation);
                image = go.GetComponent<Image>();
                image.transform.SetParent(parentElement.transform, false);
                break;
            case Attribute.MS:
                if (hasMSBuff) { break; }
                hasMSBuff = true;
                go = Instantiate(MSBuffIcon, parentElement.transform.position, parentElement.transform.rotation);
                image = go.GetComponent<Image>();
                image.transform.SetParent(parentElement.transform, false);
                break;
        }
    }
}