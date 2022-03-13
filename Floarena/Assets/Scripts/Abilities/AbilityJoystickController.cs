using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityJoystickController : MonoBehaviour
{
    public Image abilityImageOverlay;

    [SerializeField]
    Text cooldownDisplay;

    public Ability ability;

    bool isCooldown = false;

    private UIVirtualJoystick joystick;

    public Canvas abilityCanvas;

    public float RotationSmoothTime = 0.12f;

    private GameObject mainCamera;
    
    private float rotationVelocity;

    // Start is called before the first frame update
    void Start()
    {
        // placeholder
        ability = new Ability("test", 5);
        abilityImageOverlay.fillAmount = 0;
        joystick = transform.GetChild(0).gameObject.GetComponent<UIVirtualJoystick>();
        joystick.joystickOutputEvent.AddListener(UpdateAbility);
        joystick.joystickUpEvent.AddListener(ResolveAbility);
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

    public void UpdateAbility(Vector2 pointerPosition) {
        Vector3 position = new Vector3(pointerPosition.x, 0.0f, pointerPosition.y).normalized;
        float targetRotation = Mathf.Atan2(position.x, position.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);
        Canvas skillshotCanvas = (abilityCanvas.transform.GetChild(2).gameObject.GetComponent<Canvas>());
        skillshotCanvas.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        // apply relevant logic to the canvas
        // if canvas is not enabled, enable it

    }

    public void ResolveAbility(Vector2 pointerPosition) {
        // disable canvas
        // activate ability based on joystick output
        abilityImageOverlay.fillAmount = 1;
        isCooldown = true;
    }
}