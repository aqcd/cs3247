using System.Collections.Generic;

public static class Data
{
    public static Item[] ITEMS = new Item[]{
        new Item("Sturdy Soil", new Effect[]{new Effect(Attribute.HP, 100), new Effect(Attribute.MS, -0.2)}),
        new Item("Light Soil", new Effect[]{new Effect(Attribute.HP, -20), new Effect(Attribute.MS, 0.4)}),
        new Item("Prickly Thorns", new Effect[]{new Effect(Attribute.HP, 20), new Effect(Attribute.AD, 3)}),
    };

    public static Item[] DEFAULT_ITEMS = new Item[]{ITEMS[0], ITEMS[1]};

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