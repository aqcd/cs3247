using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField]
    Button button;
    
    [SerializeField]
    Text displayString;

    public Skill skill;

    void Start() {
        
    }

    public bool SetSkill(Skill skill) {
        this.skill = skill;
        displayString.text = skill.ToString();
        return true;
    }

    public void SelectSkill() {
        Debug.Log("select");
        SelectionManager.instance.SetSkill(this.skill);
    }

    public void SetDescription() {
        EditLoadoutUIManager.instance.SetDescription(this.skill.GetDescription());
    }

    public void UnsetDescription() {
        EditLoadoutUIManager.instance.UnsetDescription();
    }
}