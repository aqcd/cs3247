using UnityEngine;
using UnityEngine.UI;

public class ChannelButtonController : MonoBehaviour
{
    [SerializeField]
    Button button;
    
    [SerializeField]
    Image buttonIcon;

    private IBerry activeBerry;

    public void EnableButton(IBerry berry)
    {
        this.activeBerry = berry;
        button.interactable = true;
        buttonIcon.color = new Color(buttonIcon.color.r, buttonIcon.color.g, buttonIcon.color.b, 1f);
    }

    public void DisableButton()
    {
        this.activeBerry = null;
        button.interactable = false;
        buttonIcon.color = new Color(buttonIcon.color.r, buttonIcon.color.g, buttonIcon.color.b, 0.5f);
    }

    public void BeginChannel()
    {
        activeBerry.Consume(MatchManager.instance.GetPlayer().GetComponent<PlayerManager>());
        DisableButton();
    }
}