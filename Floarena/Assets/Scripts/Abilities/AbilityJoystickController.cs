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

    public float abilityNumber;

    bool isCooldown = false;

    private UIVirtualJoystick joystick;

    public Canvas abilityCanvas;

    public Image cancelZone;

    private Canvas skillshotCanvas;
    private Canvas skillshotHeadCanvas;
    private Canvas targetCircleCanvas;
    private Canvas rangeIndicatorCanvas;

    private GameObject mainCamera;

    bool isCancel = false;

    // Start is called before the first frame update
    void Start()
    {
        // placeholder
        ability = new Ability("test", 5, abilityNumber);
        abilityImageOverlay.fillAmount = 0;

        skillshotCanvas = (abilityCanvas.transform.GetChild(2).gameObject.GetComponent<Canvas>());
        skillshotHeadCanvas = (abilityCanvas.transform.GetChild(3).gameObject.GetComponent<Canvas>());
        rangeIndicatorCanvas = (abilityCanvas.transform.GetChild(0).gameObject.GetComponent<Canvas>());
        targetCircleCanvas = (abilityCanvas.transform.GetChild(1).gameObject.GetComponent<Canvas>());

        skillshotCanvas.enabled = false;
        skillshotHeadCanvas.enabled = false;
        targetCircleCanvas.enabled = false;
        rangeIndicatorCanvas.enabled = false;
        cancelZone.enabled = false;

        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

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
                joystick.enabled = true;
            }
        }
    }

    public void UpdateAbility(Vector2 pointerPosition) {
        Vector3 position = new Vector3(pointerPosition.x, 0.0f, pointerPosition.y);
        Vector3 localPosition = new Vector3(pointerPosition.x, pointerPosition.y, 0.0f);
        Vector3 worldPosition = transform.TransformPoint(localPosition);
        
        print(cancelZone.transform.position);

        //Check if ability gets cancelled

        
        if (ability.isSkillshot() && !isCooldown)
        {
            float targetRotation = Mathf.Atan2(position.x, position.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            skillshotCanvas.transform.rotation = Quaternion.Euler(0.0f, targetRotation, 0.0f);
            skillshotHeadCanvas.transform.rotation = Quaternion.Euler(0.0f, targetRotation, 0.0f);

            Vector3 newPosition = abilityCanvas.transform.position + position;
            float distance = Vector3.Distance(newPosition, abilityCanvas.transform.position);
            distance = Mathf.Min(distance, 5);
            Vector3 offset = new Vector3(0.0f, 0.5f, 0.0f);
            skillshotCanvas.transform.localScale = new Vector3(1.0f, 0.5f, (distance / 5));
            skillshotHeadCanvas.transform.position = abilityCanvas.transform.position + offset - (position.normalized * distance);
            skillshotCanvas.enabled = true;
            skillshotHeadCanvas.enabled = true;
            cancelZone.enabled = true;

        }

        if (ability.isTargetCircle() && !isCooldown)
        {
            Vector3 newPosition = abilityCanvas.transform.position + position;
            float distance = Vector3.Distance(newPosition, abilityCanvas.transform.position);
            distance = Mathf.Min(distance, 5);
            Vector3 offset = new Vector3(0.0f, 0.5f, 0.0f);
            targetCircleCanvas.transform.position = abilityCanvas.transform.position + offset - (position.normalized * distance);
            targetCircleCanvas.enabled = true;
            rangeIndicatorCanvas.enabled = true;
            cancelZone.enabled = true;
            

        }
        
        // apply relevant logic to the canvas
        // if canvas is not enabled, enable it

    }

    // public void ResolveOrCancelAbility(Vector2 pointerPosition)
    // {
    //     if (isCancel)
    //     {
    //         CancelAbility(pointerPosition);
    //     }

    //     else {
    //         ResolveAbility(pointerPosition);
    //     }
    // }


    public void ResolveAbility(Vector2 pointerPosition) {
        // disable canvas
        // activate ability based on joystick output
        abilityImageOverlay.fillAmount = 1;
        isCooldown = true;
        skillshotCanvas.enabled = false;
        skillshotHeadCanvas.enabled = false;
        targetCircleCanvas.enabled = false;
        rangeIndicatorCanvas.enabled = false;
        joystick.enabled = false;
        cancelZone.enabled = false;
    }

    public void CancelAbility() {
        skillshotCanvas.enabled = false;
        skillshotHeadCanvas.enabled = false;
        targetCircleCanvas.enabled = false;
        rangeIndicatorCanvas.enabled = false;
        cancelZone.enabled = false;
    }

    public void AttachAbilityCanvas(Canvas canvas)
    {
        abilityCanvas = canvas;
        skillshotCanvas = (abilityCanvas.transform.GetChild(2).gameObject.GetComponent<Canvas>());
        skillshotHeadCanvas = (abilityCanvas.transform.GetChild(3).gameObject.GetComponent<Canvas>());
        rangeIndicatorCanvas = (abilityCanvas.transform.GetChild(0).gameObject.GetComponent<Canvas>());
        targetCircleCanvas = (abilityCanvas.transform.GetChild(1).gameObject.GetComponent<Canvas>());

        skillshotCanvas.enabled = false;
        skillshotHeadCanvas.enabled = false;
        targetCircleCanvas.enabled = false;
        rangeIndicatorCanvas.enabled = false;
        cancelZone.enabled = false;
    }
}