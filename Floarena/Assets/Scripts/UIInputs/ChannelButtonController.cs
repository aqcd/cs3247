using UnityEngine;
using UnityEngine.UI;

public class ChannelButtonController : MonoBehaviour
{
    [SerializeField]
    Button button;
    
    [SerializeField]
    Image buttonIcon;
    private BerryPickupManager pickupManager;

    public void RegisterBerryPickup(BerryPickupManager pickup){
        this.pickupManager = pickup;
    }
    public void EnableButton() {
        if (!pickupManager.isChanneling){
            button.interactable = true;
            buttonIcon.color = new Color(buttonIcon.color.r, buttonIcon.color.g, buttonIcon.color.b, 1f);
        }
    }

    public void DisableButton() {
        button.interactable = false;
        buttonIcon.color = new Color(buttonIcon.color.r, buttonIcon.color.g, buttonIcon.color.b, 0.5f);
    }

    public void BeginChannel() {
        pickupManager.BeginChannel();
    }
}