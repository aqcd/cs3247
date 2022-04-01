using UnityEngine;
using UnityEngine.UI;

public class ChannelButtonController : MonoBehaviour
{
    [SerializeField]
    Button button;
    
    [SerializeField]
    Image buttonIcon;

    public IBerry activeBerry;

    private GameObject player;

    void Awake() {
        player = MatchManager.instance.GetPlayer();
    }

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
        activeBerry.Consume(player.GetComponent<PlayerManager>());
        DisableButton();
    }
}