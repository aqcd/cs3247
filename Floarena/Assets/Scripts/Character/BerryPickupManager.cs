using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class BerryPickupManager : NetworkBehaviour {   
    [SerializeField]
    public SphereCollider berryCollider;
    private ChannelButtonController channelButton;
    private IBerry activeBerry;
    void Start()
    {
        berryCollider.radius = BerryConstants.PICKUP_RANGE;
        GameObject channel = GameObject.Find("ChannelButton");
        channelButton = channel.GetComponent<ChannelButtonController>();
        if (isLocalPlayer) {
            Debug.Log("Registering SELF Pickup");
            channelButton.RegisterBerryPickup(this);
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
