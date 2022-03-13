public class Skill
{
    string name;

    string description;

    public Skill(string name, string description) {
        this.name = name;
        this.description = description;
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
