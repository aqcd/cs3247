public class Ability
{
    public string name;
    public float cooldown;
    public float range;
    public float magnitude;

    public Ability(string name, float cooldown) {
        this.name = name;
        this.cooldown = cooldown;
        this.range = 0; //placeholder
        this.magnitude = 0; //placeholder
    }
}
