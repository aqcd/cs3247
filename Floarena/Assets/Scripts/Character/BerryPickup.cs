using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryPickup : MonoBehaviour
{   
    [SerializeField]
    public SphereCollider berryCollider;
    private ChannelButtonController channelButton;

    void Start()
    {
        berryCollider.radius = BerryConstants.PICKUP_RANGE;
        GameObject channel = GameObject.Find("ChannelButton");
        channelButton = channel.GetComponent<ChannelButtonController>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Berry")) {
            channelButton.EnableButton(collider.gameObject.GetComponent<IBerry>());
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Berry")) {
            channelButton.DisableButton();
        }
    }
}
