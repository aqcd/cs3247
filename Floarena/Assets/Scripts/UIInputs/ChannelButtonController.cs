using UnityEngine;
using UnityEngine.UI;

public class ChannelButtonController : MonoBehaviour
{
    [SerializeField]
    Button button;
    
    [SerializeField]
    Image buttonIcon;

    private IBerry activeBerry;

    private float channelTime = 0.0f;

    private bool isChanneling = false;

    void Update() 
    {
        if (isChanneling) {
            channelTime += Time.deltaTime;
            if (channelTime >= BerryConstants.CHANNEL_DURATION) {
                ResolveChannel();
            }
        }
    }
    public void EnableButton(IBerry berry) {
        if (!isChanneling){
            this.activeBerry = berry;
            button.interactable = true;
            buttonIcon.color = new Color(buttonIcon.color.r, buttonIcon.color.g, buttonIcon.color.b, 1f);
        }
    }

    public void DisableButton() {
        this.activeBerry = null;
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
        activeBerry.Consume(MatchManager.instance.GetPlayer().GetComponent<PlayerManager>());
    }
}