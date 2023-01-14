public enum Gear
{
    Gear1,
    Gear2,
    Gear3,
    Gear4,
    Gear5,
    GearR,
    GearN
}

static class GearMethods
{
    public static Gear NextGear(this Gear gear)
    {
        return gear switch
        {
            Gear.Gear1 => Gear.Gear2,
            Gear.Gear2 => Gear.Gear3,
            Gear.Gear3 => Gear.Gear4,
            Gear.Gear4 => Gear.Gear5,
            Gear.Gear5 => Gear.Gear5,
            Gear.GearN => Gear.Gear1,
            Gear.GearR => Gear.GearN,
            _ => Gear.GearN
        };
    }
    
    public static Gear PreviousGear(this Gear gear)
    {
        return gear switch
        {
            Gear.Gear1 => Gear.GearN,
            Gear.Gear2 => Gear.Gear1,
            Gear.Gear3 => Gear.Gear2,
            Gear.Gear4 => Gear.Gear3,
            Gear.Gear5 => Gear.Gear4,
            Gear.GearN => Gear.GearR,
            Gear.GearR => Gear.GearR,
            _ => Gear.GearN
        };
    }

    public static float Factor(this Gear gear)
    {
        return gear switch
        {
            Gear.Gear1 => 3.2f,
            Gear.Gear2 => 1.9f,
            Gear.Gear3 => 1.33f,
            Gear.Gear4 => 1,
            Gear.Gear5 => 0.814f,
            Gear.GearN => 2,
            Gear.GearR => -3.2f,
            _ => 1
        };
    }
}