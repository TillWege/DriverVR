using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    public bool engineDebug, gearboxDebug, inputDebug, carDebug;
    public Gearbox gearbox;
    public Engine engine;
    public CarInput input;
    public CarController car;
    
    private string _engineDebug, _gearboxDebug, _inputDebug, _carDebug;

    private void OnGUI()
    {
        if (!(engineDebug || gearboxDebug || inputDebug|| carDebug)) return;
        
        if (inputDebug)
        {
            _inputDebug =  $"Device: {input.deviceName}\n";
            _inputDebug += $"Steering: {input.steeringValue}\n";
            _inputDebug += $"Gas: {input.gasAxis}\n";
            _inputDebug += $"Brake: {input.brakeAxis}\n";
            _inputDebug += $"Clutch: {input.clutchPressed}\n";
            _inputDebug += $"Blinker Left: {input.blinkLeft}\n";
            _inputDebug += $"Blinker Right: {input.blinkRight}\n";
            _inputDebug = GUI.TextArea(new Rect(10, 10, 190, 200), _inputDebug, 400);
        }

        if (gearboxDebug)
        {
            _gearboxDebug = $"Gear: {gearbox.GetCurrentGear()}\n";
            _gearboxDebug += $"Gearbox torque: {gearbox.GetTorque()}\n";
            _gearboxDebug = GUI.TextArea(new Rect(210, 10, 190, 200), _gearboxDebug, 400);
        }
        
        
        if (engineDebug)
        {
            _engineDebug = $"RPM: {engine.Rpm}\n";
            _engineDebug += $"Engine torque: {engine.GetTorque()}\n";
            _engineDebug = GUI.TextArea(new Rect(410, 10, 190, 200), _engineDebug, 400);
        }
        
        if (carDebug)
        {
            _carDebug = $"Speed in kmh: {car.speed * 3.6f}\n";
            _carDebug = GUI.TextArea(new Rect(610, 10, 190, 200), _carDebug, 400);
        }
    }
}
