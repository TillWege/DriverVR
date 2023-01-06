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
}