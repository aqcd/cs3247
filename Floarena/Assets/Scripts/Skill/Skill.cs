public class Skill
{
    public string name;
    string description;
    public enum AimType {
        SKILLSHOT,
        TARGETCIRCLE,
        SELF
    }

    public AimType aimType;

    public float cooldown;

    public Skill(string name, string description, AimType aimType, float cooldown) {
        this.name = name;
        this.description = description;
        this.aimType = aimType;
        this.cooldown = cooldown;
    }

    override public string ToString() {
        return this.name;
    }

    public string GetDescription() {
        return this.description;
    }


    public bool Equals(Skill skill) {
        return this.name == skill.name;
    }
}
