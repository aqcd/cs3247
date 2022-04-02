using UnityEngine;
using UnityEngine.UI;

public class ChannelButtonController : MonoBehaviour
{
    [SerializeField]
    Button button;
    
    [SerializeField]
    Image buttonIcon;

    private float channelTime = 0.0f;

    private bool isChanneling = false;
    private BerryPickupManager pickupManager;

    void Start()
    {
        GameObject moveJoystick = GameObject.Find("UI_Virtual_Joystick_Move");
        moveJoystick.GetComponent<UIVirtualJoystick>().joystickMoveEvent.AddListener(InterruptChannel);
        GameObject[] skillJoysticks = GameObject.FindGameObjectsWithTag("SkillJoystick");
        foreach (GameObject joystick in skillJoysticks) {
            joystick.GetComponent<UISkillVirtualJoystick>().joystickMoveEvent.AddListener(InterruptChannel);
        }
    }

    void Update() 
    {
        if (isChanneling) {
            channelTime += Time.deltaTime;
            if (channelTime >= BerryConstants.CHANNEL_DURATION) {
                ResolveChannel();
            }
        }
    }

    public void RegisterBerryPickup(BerryPickupManager pickup){
        this.pickupManager = pickup;
    }
    public void EnableButton() {
        if (!isChanneling){
            button.interactable = true;
            buttonIcon.color = new Color(buttonIcon.color.r, buttonIcon.color.g, buttonIcon.color.b, 1f);
        }
    }

    public void DisableButton() {
        button.interactable = false;
        buttonIcon.color = new Color(buttonIcon.color.r, buttonIcon.color.g, buttonIcon.color.b, 0.5f);
    }

    public void BeginChannel() {
        isChanneling = true;
    }

    public void InterruptChannel() {
        channelTime = 0.0f;
        isChanneling = false;
    }
    private void ResolveChannel() {
        channelTime = 0.0f;
        isChanneling = false;
        pickupManager.CmdConsumeBerry();
        DisableButton();
    }
}