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
                go = Instantiate(ADBuffIcon, parentElement.transform.position, parentElement.transform.rotation);
                image = go.GetComponent<Image>();
                image.transform.SetParent(parentElement.transform, false);
                break;
            case Attribute.AS:
                go = Instantiate(ASBuffIcon, parentElement.transform.position, parentElement.transform.rotation);
                image = go.GetComponent<Image>();
                image.transform.SetParent(parentElement.transform, false);
                break;
            case Attribute.MS:
                go = Instantiate(MSBuffIcon, parentElement.transform.position, parentElement.transform.rotation);
                image = go.GetComponent<Image>();
                image.transform.SetParent(parentElement.transform, false);
                break;
        }
    }
}