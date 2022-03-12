using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public Health objectHealth;
    //public Text healthString;

    private void Start()
    {
        objectHealth = gameObject.transform.parent.parent.gameObject.GetComponent<Health>();
        healthBar = GetComponent<Slider>();
        //healthString = gameObject.transform.GetChild(1).transform.GetComponent<Text>();
        healthBar.maxValue = objectHealth.maxHealth;
        healthBar.value = objectHealth.maxHealth;
    }

    public void SetHealth(int hp)
    {
        healthBar.value = hp;
        //healthString.text = hp.ToString();
    }
}
