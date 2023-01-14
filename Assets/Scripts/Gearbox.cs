using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Gearbox : MonoBehaviour
{
    public GameObject gear1, gear2, gear3, gear4, gear5, gearR, gearN;
    public CarInput input;
    public Engine engine;
    public float diffGearing = 4.1f;
    
    private Gear _currentGear = Gear.GearN;

    public void SetGear(Gear gear)
    {
        if (_currentGear == gear) return;
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

        _currentGear = gear;
    }

    private void Start()
    {
        SetGear(_currentGear);
    }

    public Gear GetCurrentGear()
    {
        return _currentGear;
    }

    public float GetTorque()
    {
        if (_currentGear == Gear.GearN || input.clutchPressed)
        {
            return 0;
        }
        
        return engine.GetTorque() * _currentGear.Factor() * diffGearing;
    }

    public float GetEffectiveRation()
    {
        return _currentGear.Factor() * diffGearing;
    }
}
