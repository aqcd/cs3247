using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{

    [SerializeField]
    private GameObject healthBarPrefab;

    private Health health;

    [SerializeField]
    private float positionOffset;

    // Start is called before the first frame update
    void Start()
    {
        health = gameObject.GetComponent<Health>();
        if (health.hasBar) {
            GameObject canvas = GameObject.Find("UI_Canvas");
            HealthBar healthBar = Instantiate(healthBarPrefab, canvas.transform).GetComponent<HealthBar>();
            healthBar.objectHealth = health;
            healthBar.positionOffset = positionOffset;
            health.healthBar = healthBar;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
