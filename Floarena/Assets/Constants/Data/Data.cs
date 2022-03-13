using System.Collections.Generic;

public static class Data
{
    public static Item[] ITEMS = new Item[]{
        new Item("Sturdy Soil", new Effect[]{new Effect(Attribute.HP, 100), new Effect(Attribute.MS, -0.2)}),
        new Item("Light Soil", new Effect[]{new Effect(Attribute.HP, -20), new Effect(Attribute.MS, 0.4)}),
        new Item("Prickly Thorns", new Effect[]{new Effect(Attribute.HP, 20), new Effect(Attribute.AD, 3)}),
    };

    public static Skill[] BASIC_SKILLS = new Skill[]{
        new Skill("Heal", "Restores " + SkillConstants.HEAL_HP_RATIO * 100 + "% of your maximum health. Cooldown: " + SkillConstants.HEAL_COOLDOWN + " seconds."),
        new Skill("Rush", "Move up to " + SkillConstants.RUSH_RANGE + " units to a targetted location. When you reach the target location, deal " 
                + SkillConstants.RUSH_DAMAGE + " damage to all entities within " + SkillConstants.RUSH_AOE_RADIUS + " range. Cooldown: " + SkillConstants.RUSH_COOLDOWN + " seconds."),
        new Skill("Vine Attack", "Shoot vines " + SkillConstants.VINE_ATTACK_RANGE + " units to a targetted location. If the vines hit a target, deal " 
                + SkillConstants.VINE_ATTACK_DAMAGE + " damage to the target. " + "Cooldown: " + SkillConstants.VINE_ATTACK_COOLDOWN + " seconds."),
    };

    public static Skill[] ULTIMATE_SKILLS = new Skill[]{
        new Skill("Vine Pull", "Shoot vines " + SkillConstants.VINE_PULL_RANGE + " units to a targetted location. If the vines hit a target, deal " 
                + SkillConstants.VINE_PULL_DAMAGE + " damage to the target and pull yourself to the target. " + "Cooldown: " + SkillConstants.VINE_PULL_COOLDOWN + " seconds."),
        new Skill("Test Placeholder", "Test Placeholder"),
    };

    public static Item[] DEFAULT_ITEMS = new Item[]{ITEMS[0], ITEMS[1]};
    public static Skill[] DEFAULT_BASIC_SKILLS = new Skill[]{BASIC_SKILLS[0], BASIC_SKILLS[1]};
    public static Skill[] DEFAULT_ULTIMATE_SKILLS = new Skill[]{ULTIMATE_SKILLS[0]};

    public static PlayerStats BASE_ATTRIBUTES = new PlayerStats(new Dictionary<Attribute, double>{
        { Attribute.HP, 100 },
        { Attribute.AD, 5 },
        { Attribute.AS, 1 },
        { Attribute.AR, 1 },
        { Attribute.MS, 2 },
    });

    public static PlayerStats ZEROED_ATTRIBUTES = new PlayerStats(new Dictionary<Attribute, double>{
        { Attribute.HP, 0 },
        { Attribute.AD, 0 },
        { Attribute.AS, 0 },
        { Attribute.AR, 0 },
        { Attribute.MS, 0 },
    });
}