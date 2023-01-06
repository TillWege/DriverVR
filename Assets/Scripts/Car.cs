using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public bool debug = false;
    public Engine engine;
    public Gearbox gearbox;
    public CarInput input;
    
    private string _debugState;
    

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void OnGUI()
    {
        _debugState = $"RPM: {engine.GetRPM()}\n";
        _debugState += $"Gear: {gearbox.getCurrentGear()}\n";
        _debugState = GUI.TextArea(new Rect(200, 10, 190, 200), _debugState, 400);
    }
}
