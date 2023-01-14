using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CarInput : MonoBehaviour
{
    public bool debug = false;
    public InputTypes inputType = InputTypes.Wheel;
    public int steeringLock = 450;
    public int wheeloffset = 30;
    public GameObject steeringWheel;

    public float steeringValue = 0;
    public float gasAxis = 0;
    public float brakeAxis = 0;
    public bool clutchPressed = false;
    public bool blinkLeft = false;
    public bool blinkRight = false;
    
    private string _debugState = "";
    private string _deviceName = "";

    void Start()
    {
        Debug.Log("SteeringInit:" + LogitechGSDK.LogiSteeringInitialize(false));
    }
    
    void OnApplicationQuit()
    {
        Debug.Log("SteeringShutdown:" + LogitechGSDK.LogiSteeringShutdown());
    }

    // Update is called once per frame
    void Update()
    {
        
        if ((inputType == InputTypes.Wheel) && LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            var builder = new StringBuilder(256);
            LogitechGSDK.LogiGetFriendlyProductName(0, builder, 256);
            _deviceName = builder.ToString();
            
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            // Check Steering
            {
                // X-Axis pos -32767 -> +32768 (Full Lock)
                steeringValue = Mapf(rec.lX, -32768, 32767, -1, 1);
            }
        
            // Pedals
            {
                // Gas
                // Y-Axis pos
                // +32767 = 0%
                // -32768 = 100%
                gasAxis = Mapf(-rec.lY, -32768, 32767, 0, 1);
                
                // Brake
                // Z-Axis rotation
                // +32767 = 0%
                // -32768 = 100%
                brakeAxis = Mapf(-rec.lRz, -32768, 32767, 0, 1);
                
                // Clutch
                // Extra Axis pos 1
                // +32767 = 0%
                // -32768 = 100%
                clutchPressed = rec.rglSlider[0] < 0;
            }
        
            // Check Shifting
            {
                // Buttons 12-18 (1st - 6th - reverse)
                if (rec.rgbButtons[12] == 128)
                {
                    //shifter.setGear(Gear.Gear1);
                } else if (rec.rgbButtons[13] == 128)
                {
                    //shifter.setGear(Gear.Gear2);
                } else if (rec.rgbButtons[14] == 128)
                {
                    //shifter.setGear(Gear.Gear3);
                } else if (rec.rgbButtons[15] == 128)
                {
                    //shifter.setGear(Gear.Gear4);
                } else if (rec.rgbButtons[16] == 128)
                {
                    //shifter.setGear(Gear.Gear5);
                } else if (rec.rgbButtons[18] == 128)
                {
                    //shifter.setGear(Gear.GearR);
                }
                else
                {
                    //shifter.setGear(Gear.GearN);
                }
    
            }
        
            // Check Blinker
            {
                // btn 7 = left
                blinkLeft = rec.rgbButtons[7] == 128;

                // btn 6 = right
                blinkRight = rec.rgbButtons[6] == 128;
            }
            
            // Start-Button
            {
                if (rec.rgbButtons[0] == 128)
                {
                    //StartCoroutine(engine.StartEngine());
                }
            }
            
        }
        else
        {
            _deviceName   = "Fallback";
            steeringValue = Input.GetAxis("Steering");
            gasAxis       = Input.GetAxis("Gas");
            brakeAxis     = Input.GetAxis("Brake");

            clutchPressed = Input.GetKey(KeyCode.C);
            blinkLeft     = Input.GetKeyUp(KeyCode.Q);
            blinkRight    = Input.GetKeyUp(KeyCode.E);

            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                //shifter.setGear(Gear.Gear1);
            }
            else if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                //shifter.setGear(Gear.Gear2);
            } 
            else if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                //shifter.setGear(Gear.Gear3);
            } 
            else if (Input.GetKeyUp(KeyCode.Alpha4))
            {
                //shifter.setGear(Gear.Gear4);
            } 
            else if (Input.GetKeyUp(KeyCode.Alpha5))
            {
                //shifter.setGear(Gear.Gear5);
            }
            else if (Input.GetKeyUp(KeyCode.R))
            {
                //shifter.setGear(Gear.GearR);
            }
            else if (Input.GetKeyUp(KeyCode.N))
            {
                //shifter.setGear(Gear.GearN);
            }
            
            if (Input.GetKeyUp(KeyCode.Space))
            {
                //StartCoroutine(engine.StartEngine());
            }
        }
      
        var rotation = Vector3.zero;
        var eulerAngles = steeringWheel.transform.eulerAngles;

        rotation.x = eulerAngles.x;
        rotation.y = eulerAngles.y;
        rotation.z = -steeringValue * steeringLock;
        steeringWheel.transform.eulerAngles = rotation;
    }

    private void OnGUI()
    {
        if (!debug) return;
        
        _debugState = $"Device: {_deviceName}\n";
        _debugState += $"Steering: {steeringValue}\n";
        _debugState += $"Gas: {gasAxis}\n";
        _debugState += $"Brake: {brakeAxis}\n";
        _debugState += $"Clutch: {clutchPressed}\n";
        _debugState += $"Blinker Left: {blinkLeft}\n";
        _debugState += $"Blinker Right: {blinkRight}\n";
        _debugState = GUI.TextArea(new Rect(10, 10, 190, 200), _debugState, 400);
    }

    private static int Map(int value, int fromLow, int fromHigh, int toLow, int toHigh) 
    {
        return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
    }
    
    private static float Mapf(float value, float fromLow, float fromHigh, float toLow, float toHigh) 
    {
        return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
    }
}
