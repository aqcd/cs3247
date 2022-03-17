using UnityEngine;
using UnityEngine.UI;

public class SkillSelection : MonoBehaviour
{
    [SerializeField]
    public int position;

    [SerializeField]
    Button button;
    
    [SerializeField]
    Image displayImage;

    public Skill skill;

    void Start() {
        if (this.position < 3) {
            SetSkill(Data.DEFAULT_BASIC_SKILLS[position - 1]);
        } else {
            SetSkill(Data.DEFAULT_ULTIMATE_SKILLS[position - 3]);
        }
    }

    public bool SetSkill(Skill skill) {
        this.skill = skill;
        SetImage();
        return true;
    }

    void SetImage() {
        displayImage.sprite = SelectionManager.instance.skillSprites[skill.name];
    }
}