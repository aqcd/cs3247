using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBarSlider;
    public Health objectHealth;
    private float barWidth;

    public GameObject dividerPrefab;

    [SerializeField]
    private float division = 100;

    public void Init(float maxHealth)
    {
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = maxHealth;
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        barWidth = rt.rect.width;

        float pixelsPerHP = (float)barWidth / maxHealth;
        float lineOffset = pixelsPerHP * division;
        int numberOfLines = Mathf.FloorToInt(maxHealth / division);
        RectTransform dividerRt = (RectTransform)dividerPrefab.transform;
        float dividerWidth = dividerRt.rect.width;
        for (int i = 1; i < numberOfLines + 1; i++) {
            float offset_current = i * lineOffset - dividerWidth;
            GameObject divider = Instantiate(dividerPrefab, transform);
            divider.transform.position -= new Vector3(offset_current, 0 , 0);
        }
    }

    public void SetBarColor(int whichPlayersBar) {
        if (whichPlayersBar == 1) {
            // If I'm player 1, set healthbar to blue
            transform.GetChild(0).GetComponent<Image>().color = new Color(0f, 0.4f, 1f, 1f);
        } else {
            // If I'm player 2, set healthbar to red
            transform.GetChild(0).GetComponent<Image>().color = new Color(0.87f, 0.05f, 0f, 1f);
        }
    }

    public void SetHealth(float hp)
    {
        healthBarSlider.value = hp;
    }
}
