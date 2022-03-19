public class Ability
{
    public string name;
    public float cooldown;
    public float range;
    public float magnitude;
    public float abilityType;

    public Ability(string name, float cooldown, float abilityType) {
        this.name = name;
        this.cooldown = cooldown;
        this.range = 0; //placeholder
        this.magnitude = 0; //placeholder
        this.abilityType = abilityType;
    }

    public bool isSkillshot()
    {
        if (abilityType == 1)
        {
            return true;
        } else {
            return false;
        }
    }

    public bool isTargetCircle()
    {
        if (abilityType == 2)
        {
            return true;
        } else {
            return false;
        }
    }
}
