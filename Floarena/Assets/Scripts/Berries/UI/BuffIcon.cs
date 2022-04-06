using UnityEngine;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour {
    [SerializeField]
    public Attribute attribute;

    private GameObject player;
    private PlayerManager playerManager;
    private ParticleSystemManager particleSystemManager;

    private float fullDuration;
    private float remainingDuration;

    private Image image;

    void Start() {
        player = MatchManager.instance.GetPlayer();
        playerManager = player.GetComponent<PlayerManager>();
        particleSystemManager = player.GetComponent<ParticleSystemManager>();
        image = gameObject.GetComponent<Image>();
        fullDuration = playerManager.GetBuffDuration(attribute);
        remainingDuration = fullDuration;

        
        switch (attribute) {
            case Attribute.AD:
                particleSystemManager.PlayADUP();
                break;
            case Attribute.AS:
                particleSystemManager.PlayASUP();
                break;
            case Attribute.MS:
                particleSystemManager.PlayMSUP();
                break;
        }
    }

    void Update() {
        remainingDuration = playerManager.GetBuffDuration(attribute);
        image.fillAmount = remainingDuration / fullDuration;
        if (remainingDuration < 0.0f) {
            switch (attribute) {
                case Attribute.AD:
                    BuffIconsManager.instance.hasADBuff = false;
                    particleSystemManager.StopADUP();
                    break;
                case Attribute.AS:
                    BuffIconsManager.instance.hasASBuff = false;
                    particleSystemManager.StopASUP();
                    break;
                case Attribute.MS:
                    BuffIconsManager.instance.hasMSBuff = false;
                    particleSystemManager.StopMSUP();
                    break;
            }
            Destroy(gameObject);
        }
    }
}