using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gearbox : MonoBehaviour
{
    public GameObject gear1, gear2, gear3, gear4, gear5, gearR, gearN;
    public CarInput input;
    public Engine engine;
    private Gear currentGear = Gear.GearN;

    public void setGear(Gear gear)
    {
        if (currentGear == gear) return;
        if (engine.running && !input.clutchPressed)
        {
            engine.Stall();
        }
        
        gear1.SetActive(gear == Gear.Gear1);
        gear2.SetActive(gear == Gear.Gear2);
        gear3.SetActive(gear == Gear.Gear3);
        gear4.SetActive(gear == Gear.Gear4);
        gear5.SetActive(gear == Gear.Gear5);
        gearR.SetActive(gear == Gear.GearR);
        gearN.SetActive(gear == Gear.GearN);

        currentGear = gear;
    }

    private void Start()
    {
        setGear(currentGear);
    }

    public Gear getCurrentGear()
    {
        return currentGear;
    }
}
