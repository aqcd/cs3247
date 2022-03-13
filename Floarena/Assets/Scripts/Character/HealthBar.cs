using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBarSlider;
    public Health objectHealth;
    //public Text healthString;
    public float positionOffset;
    private float barWidth;
    private int division = 100;

    private void Start()
    {
        //healthString = gameObject.transform.GetChild(1).transform.GetComponent<Text>();
        healthBarSlider.maxValue = objectHealth.maxHealth;
        healthBarSlider.value = objectHealth.maxHealth;

        RectTransform rt = gameObject.GetComponent<RectTransform>();
        barWidth = rt.rect.width;

        float pixelsPerHP = (float)barWidth / objectHealth.maxHealth;//~0.6f
        int lineOffset = Mathf.RoundToInt(pixelsPerHP * division);//~60
        int numberOfLines = Mathf.RoundToInt((float)objectHealth.maxHealth / division);
        for (int i = 1; i < numberOfLines + 1; i++) {
            int offset_current = i * lineOffset;
            //Draw line using offset_current
        }
    }

    public void SetHealth(int hp)
    {
        healthBarSlider.value = hp;
        //healthString.text = hp.ToString();
    }

    private void LateUpdate()
    {
        if (objectHealth) {
            transform.position = Camera.main.WorldToScreenPoint(objectHealth.transform.position + Vector3.up * positionOffset);
        }
    }
}
