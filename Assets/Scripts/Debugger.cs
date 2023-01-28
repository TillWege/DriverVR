using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    public bool engineDebug, gearboxDebug, inputDebug, carDebug, taskDebug;
    public Gearbox gearbox;
    public Engine engine;
    public CarInput input;
    public CarController car;
    public TaskController taskController;
    
    private string _engineDebug, _gearboxDebug, _inputDebug, _carDebug, _taskDebug, _comptaskDebug;

    private void OnGUI()
    {
        if (!(engineDebug || gearboxDebug || inputDebug|| carDebug || taskDebug)) return;
        
        if (inputDebug)
        {
            _inputDebug =  $"Device: {input.deviceName}\n";
            _inputDebug += $"Steering: {input.steeringValue}\n";
            _inputDebug += $"Gas: {input.gasAxis}\n";
            _inputDebug += $"Brake: {input.brakeAxis}\n";
            _inputDebug += $"Clutch: {input.clutchPressed}\n";
            _inputDebug += $"Blinker Left: {input.blinkingLeft}\n";
            _inputDebug += $"Blinker Right: {input.blinkingRight}\n";
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
            _engineDebug = $"RPM: {engine.GetEffectiveRPM()}\n";
            _engineDebug += $"Engine torque: {engine.GetTorque()}\n";
            _engineDebug = GUI.TextArea(new Rect(410, 10, 190, 200), _engineDebug, 400);
        }
        
        if (carDebug)
        {
            _carDebug = $"Speed in kmh: {car.Speed}\n";
            _carDebug += $"Max Speed: {car.gearbox.GetCurrentGear().MaxSpeed()}";
            _carDebug = GUI.TextArea(new Rect(610, 10, 190, 200), _carDebug, 400);
        }

        if (taskDebug)
        {
            _taskDebug = $"Open Tasks: {taskController.OpenTasks.Count}\n";
            _taskDebug += $"Completed Tasks: {taskController.CompletedTasks.Count}\n";
            for (int i = 0; i < taskController.OpenTasks.Count; i++)
            {
                _taskDebug += $"{taskController.OpenTasks[i].GetDescription()}\n";
            }
            _taskDebug = GUI.TextArea(new Rect(810, 10, 190, 200), _taskDebug, 400);
            
            _comptaskDebug = $"Completed Tasks: {taskController.CompletedTasks.Count}\n";
            for (int i = 0; i < taskController.CompletedTasks.Count; i++)
            {
                _comptaskDebug += $"{taskController.CompletedTasks[i].GetDescription()}\n";
            }
            _comptaskDebug = GUI.TextArea(new Rect(1010, 10, 190, 200), _comptaskDebug, 400);
        }
    }
}
