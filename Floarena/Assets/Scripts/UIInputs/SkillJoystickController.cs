using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillJoystickController : MonoBehaviour
{
    public Image skillImage;
    public Image skillImageOverlay;

    [SerializeField]
    Text cooldownDisplay;

    private Skill skill;
    public GameObject skillObj;

    public int skillIndex;

    bool isCooldown = false;
    float remainingCooldown = 0.0f;

    private UISkillVirtualJoystick joystick;

    private Canvas skillCanvas;

    public Image cancelZone;

    private Canvas skillshotCanvas;
    private Canvas skillshotHeadCanvas;
    private Canvas targetCircleCanvas;
    private Canvas rangeIndicatorCanvas;

    private GameObject mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        joystick = transform.GetChild(0).gameObject.GetComponent<UISkillVirtualJoystick>();
        joystick.joystickOutputEvent.AddListener(UpdateSkill);
        joystick.joystickUpEvent.AddListener(ResolveSkill);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCooldown)
        {
            remainingCooldown -= Time.deltaTime;
            cooldownDisplay.text = remainingCooldown.ToString("#0");
            skillImageOverlay.fillAmount -= 1 / skill.cooldown * Time.deltaTime;

            if (skillImageOverlay.fillAmount <= 0)
            {
                StopCooldown();
                joystick.enabled = true;
            }
        }
    }

    public void StartCooldown() {
        skillImageOverlay.fillAmount = 1;
        isCooldown = true;
        remainingCooldown = skill.cooldown;
        Color skillImageColor = skillImage.color;
        skillImageColor.a = 0.5f;
        skillImage.color = skillImageColor;
        Color skillImageOverlayColor = skillImageOverlay.color;
        skillImageOverlayColor.a = 0.5f;
        skillImageOverlay.color = skillImageOverlayColor;
        cooldownDisplay.text = remainingCooldown.ToString("#0");
    }

    public void StopCooldown() {
        skillImageOverlay.fillAmount = 0;
        isCooldown = false;
        Color skillImageColor = skillImage.color;
        skillImageColor.a = 1.0f;
        skillImage.color = skillImageColor;
        Color skillImageOverlayColor = skillImageOverlay.color;
        skillImageOverlayColor.a = 1.0f;
        skillImageOverlay.color = skillImageOverlayColor;
        cooldownDisplay.text = "";
    }

    public void UpdateSkill(Vector2 pointerPosition) {
        Vector3 position = new Vector3(pointerPosition.x, 0.0f, pointerPosition.y);

        
        if (skill.aimType == Skill.AimType.SKILLSHOT && !isCooldown)
        {
            float targetRotation = Mathf.Atan2(position.x, position.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            Debug.Log("SKILLSHOT: " + targetRotation);
            Debug.Log(skillshotCanvas);
            skillshotCanvas.transform.rotation = Quaternion.Euler(0.0f, targetRotation, 0.0f);
            skillshotHeadCanvas.transform.rotation = Quaternion.Euler(0.0f, targetRotation, 0.0f);

            Vector3 newPosition = skillCanvas.transform.position + position;
            float distance = Vector3.Distance(newPosition, skillCanvas.transform.position);
            distance = Mathf.Min(distance, 5);
            Vector3 offset = new Vector3(0.0f, 0.01f, 0.0f);
            skillshotCanvas.transform.localScale = new Vector3(1.0f, 0.01f, (distance / 5));
            skillshotHeadCanvas.transform.position = skillCanvas.transform.position + offset - (position.normalized * distance);
            skillshotCanvas.enabled = true;
            skillshotHeadCanvas.enabled = true;
            cancelZone.enabled = true;

        }

        if (skill.aimType == Skill.AimType.TARGETCIRCLE && !isCooldown)
        {
            Vector3 newPosition = skillCanvas.transform.position + position;
            float distance = Vector3.Distance(newPosition, skillCanvas.transform.position);
            distance = Mathf.Min(distance, 5);
            Vector3 offset = new Vector3(0.0f, 0.01f, 0.0f);
            targetCircleCanvas.transform.position = skillCanvas.transform.position + offset - (position.normalized * distance);
            targetCircleCanvas.enabled = true;
            rangeIndicatorCanvas.enabled = true;
            cancelZone.enabled = true;

        }

        if (skill.aimType == Skill.AimType.SELF && !isCooldown)
        {
            joystick.isButton = true;
        }
        
        // apply relevant logic to the canvas
        // if canvas is not enabled, enable it

    }

    // public void ResolveOrCancelSkill(Vector2 pointerPosition)
    // {
    //     if (isCancel)
    //     {
    //         CancelSkill(pointerPosition);
    //     }

    //     else {
    //         ResolveSkill(pointerPosition);
    //     }
    // }


    public void ResolveSkill(Vector2 pointerPosition) {
        // disable canvas
        // activate Skill based on joystick output
        StartCooldown();
        skillshotCanvas.enabled = false;
        skillshotHeadCanvas.enabled = false;
        targetCircleCanvas.enabled = false;
        rangeIndicatorCanvas.enabled = false;
        cancelZone.enabled = false;

        Vector3 skillPosition = new Vector3(pointerPosition.x, 0.0f, pointerPosition.y);
        Vector3 localPosition = new Vector3(pointerPosition.x, pointerPosition.y, 0.0f);
        Vector3 worldPosition = transform.TransformPoint(transform.localPosition + localPosition);

        Vector3 parentPosition = transform.parent.position;
        
        RectTransform cancelArea = cancelZone.rectTransform;
        Vector3[] cancelSpace = new Vector3[4];
        cancelArea.GetLocalCorners(cancelSpace);

        for (var i = 0; i < 4; i++)
        {
            print(cancelSpace[i]);
        }

        print(localPosition);
        
        
        if (worldPosition.x >  cancelSpace[0].x && worldPosition.y > cancelSpace[0].y &&
        worldPosition.x < cancelSpace[2].x && worldPosition.y < cancelSpace[2].y)
        {
            print("cancel");
            return;
        }

        skillImageOverlay.fillAmount = 1;
        isCooldown = true;
        joystick.enabled = false;

        skillObj.SendMessage("Execute", -skillPosition);
    }

    public void CancelSkill() {
        skillshotCanvas.enabled = false;
        skillshotHeadCanvas.enabled = false;
        targetCircleCanvas.enabled = false;
        rangeIndicatorCanvas.enabled = false;
        cancelZone.enabled = false;
    }

    public void AttachSkillCanvas(Canvas canvas)
    {
        skillCanvas = canvas;
        skillshotCanvas = (skillCanvas.transform.GetChild(2).gameObject.GetComponent<Canvas>());
        skillshotHeadCanvas = (skillCanvas.transform.GetChild(3).gameObject.GetComponent<Canvas>());
        rangeIndicatorCanvas = (skillCanvas.transform.GetChild(0).gameObject.GetComponent<Canvas>());
        targetCircleCanvas = (skillCanvas.transform.GetChild(1).gameObject.GetComponent<Canvas>());

        skillshotCanvas.enabled = false;
        skillshotHeadCanvas.enabled = false;
        targetCircleCanvas.enabled = false;
        rangeIndicatorCanvas.enabled = false;
        cancelZone.enabled = false;
    }

    public void SetSkill(Skill skill)
    {
        this.skill = skill;
        if (skill.aimType == Skill.AimType.SELF)
        {
            joystick.isButton = true;
        }
    }

    public void SetSkillObject(GameObject skillObj)
    {
        this.skillObj = skillObj;
    }

    public void SetSkillImage(Sprite skillImage) {
        // Setting SkillImage
        transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = skillImage;
        // Setting SkillImageOverlay
        transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>().sprite = skillImage;
    }
}