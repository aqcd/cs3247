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
    private Animator _animator;

    void Start() {
        berryCollider.radius = BerryConstants.PICKUP_RANGE;
        _animator = GetComponent<Animator>();
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

    void Update() {   
        if (!isServer) {
            if (isChanneling) {
                _animator.SetBool("isPicking", true);
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
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.CompareTag("Berry")) {
            GameObject closestBerry = collider.gameObject;
            if (activeBerry != null && activeBerry != closestBerry) {
                closestBerry = FindClosestBerry(activeBerry, collider.gameObject);
            }
            if (closestBerry != activeBerry) {
                if (activeBerry != null) {
                    activeBerry.SendMessage("DisableCanvas");
                }
                activeBerry = closestBerry;
                if (isLocalPlayer) {
                    channelButton.EnableButton();
                    activeBerry.SendMessage("EnableCanvas");
                }
            }
        }
    }

    private GameObject FindClosestBerry(GameObject berry1, GameObject berry2)
    {
        float sqrdist1 = (berry1.transform.position - transform.position).sqrMagnitude;
        float sqrdist2 = (berry2.transform.position - transform.position).sqrMagnitude;
        return sqrdist1 <= sqrdist2 ? berry1 : berry2;
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Berry")) {
            collider.gameObject.SendMessage("DisableCanvas");
            if (collider.gameObject == activeBerry) {
                activeBerry = null;
                if (isLocalPlayer) {
                channelButton.DisableButton();
            }
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
        PlayAudio();
        RpcEnableBar();
    }

    public void PlayAudio() {
        StartCoroutine(Wait());
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.PlaySound(AudioIndex.CHANNEL_AUDIO, this.transform.position);
    }

    public void InterruptChannel()
    {
        _animator.SetBool("isPicking", false);
        CmdInterruptChannel();
    }

    [Command]
    public void CmdInterruptChannel() {
        if (isChanneling) {
            Debug.Log("Interrupting");
            isChanneling = false;
            RpcDisableBar();
        }
    }

    private void ResolveChannel() {
        _animator.SetBool("isPicking", false);
        CmdResolveChannel();
    }

    [Command]
    private void CmdResolveChannel() {
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
            activeBerry.SendMessage("DisableCanvas");
            activeBerry.SendMessage("Consume", MatchManager.instance.GetPlayer().GetComponent<PlayerManager>());
            AudioManager.instance.PlaySound(AudioIndex.TAKE_BERRY_AUDIO, transform.position);

            Debug.Log("Giving player points!");
            MatchManager.instance.CommandAddScore(MatchManager.instance.GetOpponentNum(), 3);
        } else {
            activeBerry.SendMessage("DestroySelf");
        }

        activeBerry = null;
    }
}
