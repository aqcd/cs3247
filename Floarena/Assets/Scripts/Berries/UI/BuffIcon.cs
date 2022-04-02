using UnityEngine;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour {
    [SerializeField]
    public Attribute attribute;

    private GameObject player;
    private PlayerManager playerManager;

    private float fullDuration;
    private float remainingDuration;

    private Image image;

    void Start() {
        player = MatchManager.instance.GetPlayer();
        playerManager = player.GetComponent<PlayerManager>();
        image = gameObject.GetComponent<Image>();
        fullDuration = playerManager.GetBuffDuration(attribute);
        remainingDuration = fullDuration;
    }

    void Update() {
        remainingDuration -= Time.deltaTime;
        image.fillAmount = remainingDuration / fullDuration;
        if (remainingDuration < 0.0f) {
            Destroy(gameObject);
        }
    }
}