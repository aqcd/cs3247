using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBarSlider;
    public Health objectHealth;
    //public Text healthString;
    private float barWidth;

    public GameObject dividerPrefab;

    [SerializeField]
    private float division = 100;

    private void Start()
    {
        //healthString = gameObject.transform.GetChild(1).transform.GetComponent<Text>();
        healthBarSlider.maxValue = objectHealth.maxHealth;
        healthBarSlider.value = objectHealth.maxHealth;
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        barWidth = rt.rect.width;

        float pixelsPerHP = (float)barWidth / objectHealth.maxHealth;
        float lineOffset = pixelsPerHP * division;
        int numberOfLines = Mathf.FloorToInt(objectHealth.maxHealth / division);
        RectTransform dividerRt = (RectTransform)dividerPrefab.transform;
        float dividerWidth = dividerRt.rect.width;
        for (int i = 1; i < numberOfLines + 1; i++) {
            float offset_current = i * lineOffset - dividerWidth;
            GameObject divider = Instantiate(dividerPrefab, transform);
            divider.transform.position -= new Vector3(offset_current, 0 , 0);
        }
        
    }

    public void SetHealth(float hp)
    {
        healthBarSlider.value = hp;
        //healthString.text = hp.ToString();
    }
}
