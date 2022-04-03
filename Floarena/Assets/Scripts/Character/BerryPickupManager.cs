using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class BerryPickupManager : NetworkBehaviour {   
    [SerializeField]
    public SphereCollider berryCollider;
    private ChannelButtonController channelButton;
    private GameObject activeBerry;
    private float channelTime = 0.0f;
    [SyncVar(hook = nameof(UpdateIsChanneling))]
    public bool isChanneling = false;
    public ChannelBar channelBar;
    void Start()
    {
        berryCollider.radius = BerryConstants.PICKUP_RANGE;
        if (isLocalPlayer) {
            GameObject channel = GameObject.Find("ChannelButton");
            channelButton = channel.GetComponent<ChannelButtonController>();
            channelButton.RegisterBerryPickup(this);
            GameObject moveJoystick = GameObject.Find("UI_Virtual_Joystick_Move");
            moveJoystick.GetComponent<UIVirtualJoystick>().joystickMoveEvent.AddListener(InterruptChannel);
            GameObject[] skillJoysticks = GameObject.FindGameObjectsWithTag("SkillJoystick");
            foreach (GameObject joystick in skillJoysticks) {
                Debug.Log("Joystick registration" + joystick);
                joystick.GetComponent<UISkillVirtualJoystick>().joystickMoveEvent.AddListener(InterruptChannel);
            }
            Health health = gameObject.GetComponent<Health>();
            Debug.Log("Health registration" + health);
            health.damageTakenEvent.AddListener(InterruptChannel);
        }
    }

    void Update()
    {   
        if (isChanneling) {
            if (activeBerry == null) {
                InterruptChannel();
                channelButton.DisableButton();
                return;
            }
            channelTime += Time.deltaTime;
            channelBar.SetChannel(BerryConstants.CHANNEL_DURATION - channelTime);
            if (channelTime >= BerryConstants.CHANNEL_DURATION) {
                if (isLocalPlayer) {
                    ResolveChannel();
                    channelButton.DisableButton();
                }
            }
        } else {
            channelTime = 0.0f;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.CompareTag("Berry")) {
            activeBerry = collider.gameObject;
            if (isLocalPlayer) {
                channelButton.EnableButton();
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Berry")) {
            activeBerry = null;
            if (isLocalPlayer) {
                channelButton.DisableButton();
            }
        }
    }

    void UpdateIsChanneling(bool oldChannel, bool newChannel) {
        isChanneling = newChannel;
    }

    [Command]
    public void BeginChannel()
    {
        isChanneling = true;
        RpcEnableBar();
    }

    [Command]
    public void InterruptChannel()
    {
        if (isChanneling) {
            Debug.Log("Interrupting");
            isChanneling = false;
            RpcDisableBar();
        }
    }
    [Command]
    private void ResolveChannel() {
        isChanneling = false;
        RpcConsumeBerry();
        RpcDisableBar();
    }
    
    [ClientRpc]
    private void RpcEnableBar()
    {
        channelBar.EnableBar();
    }
    [ClientRpc]
    private void RpcDisableBar()
    {
        channelTime = 0.0f;
        channelBar.DisableBar();
    }
    [ClientRpc]
    private void RpcConsumeBerry()
    {
        if (activeBerry == null) {
            return;
        }
        if (isLocalPlayer) {
            activeBerry.SendMessage("Consume", MatchManager.instance.GetPlayer().GetComponent<PlayerManager>());
        } else {
            activeBerry.SendMessage("DestroySelf");
        }
        activeBerry = null;
    }
}
