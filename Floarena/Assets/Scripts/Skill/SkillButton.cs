using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField]
    Button button;
    
    [SerializeField]
    Image displayImage;

    public Skill skill;

    void Start() {
        
    }

    public bool SetSkill(Skill skill) {
        this.skill = skill;
        displayImage.sprite = SelectionManager.instance.skillSprites[skill.name];
        return true;
    }

    public void SelectSkill() {
        SelectionManager.instance.SetSkill(this.skill);
    }

    public void SetDescription() {
        EditLoadoutUIManager.instance.SetDescription(this.skill.GetDescription());
    }

    public void UnsetDescription() {
        EditLoadoutUIManager.instance.UnsetDescription();
    }
}