using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private GameObject healthBarPrefab;
    [SerializeField]
    private float positionOffset;

    public Health health;

    void Awake()
    {
        if (health.hasBar) {
            GameObject canvas = GameObject.Find("UI_Canvas");
            HealthBar newHealthBar = Instantiate(healthBarPrefab, canvas.transform).GetComponent<HealthBar>();
            newHealthBar.objectHealth = health;
            newHealthBar.positionOffset = positionOffset;
            health.healthBar = newHealthBar;
        }
    }
}
