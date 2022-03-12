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

    private void Start()
    {
        //healthString = gameObject.transform.GetChild(1).transform.GetComponent<Text>();
        healthBarSlider.maxValue = objectHealth.maxHealth;
        healthBarSlider.value = objectHealth.maxHealth;
    }

    public void SetHealth(int hp)
    {
        healthBarSlider.value = hp;
        //healthString.text = hp.ToString();
    }

    private void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(objectHealth.transform.position + Vector3.up * positionOffset);
    }
}
