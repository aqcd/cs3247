using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [SerializeField]
    Button button;

    public Image abilityImageOverlay;

    [SerializeField]
    Text cooldownDisplay;

    public Ability ability;
    public int abilityPosition;

    bool isCooldown = false;

    // Start is called before the first frame update
    void Start()
    {
        // placeholder
        ability = new Ability("test", 5);
        abilityImageOverlay.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCooldown)
        {
            abilityImageOverlay.fillAmount -= 1 / ability.cooldown * Time.deltaTime;

            if (abilityImageOverlay.fillAmount <= 0)
            {
                abilityImageOverlay.fillAmount = 0;
                isCooldown = false;
            }
        }
    }

    public void ActivateAbility() {
        abilityImageOverlay.fillAmount = 1;
        isCooldown = true;
    }
}
