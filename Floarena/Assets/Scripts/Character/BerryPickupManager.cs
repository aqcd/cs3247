using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class BerryPickupManager : NetworkBehaviour {   
    [SerializeField]
    public SphereCollider berryCollider;
    private ChannelButtonController channelButton;
    private IBerry activeBerry;
    [SyncVar]
    private float channelTime = 0.0f;
    [SyncVar]
    public bool isChanneling = false;
    public ChannelBar channelBar;
    void Start()
    {
        berryCollider.radius = BerryConstants.PICKUP_RANGE;
        GameObject channel = GameObject.Find("ChannelButton");
        channelButton = channel.GetComponent<ChannelButtonController>();
        if (isLocalPlayer) {
            channelButton.RegisterBerryPickup(this);
            GameObject moveJoystick = GameObject.Find("UI_Virtual_Joystick_Move");
            moveJoystick.GetComponent<UIVirtualJoystick>().joystickMoveEvent.AddListener(InterruptChannel);
            GameObject[] skillJoysticks = GameObject.FindGameObjectsWithTag("SkillJoystick");
            foreach (GameObject joystick in skillJoysticks) {
                joystick.GetComponent<UISkillVirtualJoystick>().joystickMoveEvent.AddListener(InterruptChannel);
            }
        }
    }

    void Update()
    {
        if (isChanneling) {
            channelTime += Time.deltaTime;
            channelBar.SetChannel(BerryConstants.CHANNEL_DURATION - channelTime);
            if (channelTime >= BerryConstants.CHANNEL_DURATION) {
                ResolveChannel();
            }
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.CompareTag("Berry")) {
            activeBerry = collider.gameObject.GetComponent<IBerry>();
            channelButton.EnableButton();
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Berry")) {
            channelButton.DisableButton();
        }
    }

    public void BeginChannel()
    {
        isChanneling = true;
        channelBar.EnableBar();
    }

    public void InterruptChannel()
    {
        isChanneling = false;
        channelTime = 0.0f;
        channelBar.DisableBar();
    }
    
    private void ResolveChannel() {
        channelTime = 0.0f;
        isChanneling = false;
        CmdConsumeBerry();
        channelBar.DisableBar();
        channelButton.DisableButton();
    }

    [Command]
    public void CmdConsumeBerry()
    {
        RpcConsumeBerry();
    }

    [ClientRpc]
    private void RpcConsumeBerry()
    {
        if (isLocalPlayer) {
            activeBerry.Consume(MatchManager.instance.GetPlayer().GetComponent<PlayerManager>());
        } else {
            activeBerry.DestroySelf();
        }
        activeBerry = null;
    }
}
