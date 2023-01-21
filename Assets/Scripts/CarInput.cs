using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

public class CarInput : MonoBehaviour
{
    public InputTypes inputType = InputTypes.Wheel;
    public GameObject steeringWheel;
    public Engine engine;
    public Gearbox gearbox;
    public CarController controller;
    public WorldController worldController;

    public int steeringLock = 450;
    public string deviceName;
    public float steeringValue = 0;
    public float gasAxis = 0;
    public float brakeAxis = 0;
    public bool clutchPressed = false;
    public bool blinkingLeft = false;
    public bool blinkingRight = false;
    

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
            deviceName = builder.ToString();
            
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            // Check Steering
            {
                // X-Axis pos -32767 -> +32768 (Full Lock)
                steeringValue = Math.Mapf(rec.lX, -32768, 32767, -1, 1);
            }
        
            // Pedals
            {
                // Gas
                // Y-Axis pos
                // +32767 = 0%
                // -32768 = 100%
                gasAxis = Math.Mapf(-rec.lY, -32768, 32767, 0, 1);
                
                // Brake
                // Z-Axis rotation
                // +32767 = 0%
                // -32768 = 100%
                brakeAxis = Math.Mapf(-rec.lRz, -32768, 32767, 0, 1);
                
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
                    gearbox.SetGear(Gear.Gear1);
                } else if (rec.rgbButtons[13] == 128)
                {
                    gearbox.SetGear(Gear.Gear2);
                } else if (rec.rgbButtons[14] == 128)
                {
                    gearbox.SetGear(Gear.Gear3);
                } else if (rec.rgbButtons[15] == 128)
                {
                    gearbox.SetGear(Gear.Gear4);
                } else if (rec.rgbButtons[16] == 128)
                {
                    gearbox.SetGear(Gear.Gear5);
                } else if (rec.rgbButtons[18] == 128)
                {
                    gearbox.SetGear(Gear.GearR);
                }
                else
                {
                    gearbox.SetGear(Gear.GearN);
                }
    
            }
        
            // Check Blinker
            {
                // btn 7 = left
                if (rec.rgbButtons[7] == 128)
                {
                    controller.BlinkLeft();
                }
                
                // btn 6 = right
                if (rec.rgbButtons[6] == 128)
                {
                    controller.BlinkRight();
                }
            }
            
            // Start-Button
            {
                if (rec.rgbButtons[0] == 128)
                {
                    StartCoroutine(engine.StartEngine());
                }
            }
            
        }
        else
        {
            deviceName   = "Keyboard Fallback";
            steeringValue = Input.GetAxis("Steering");
            gasAxis       = Input.GetAxis("Gas");
            brakeAxis     = Input.GetAxis("Brake");

            clutchPressed = Input.GetKey(KeyCode.C);
            
            if (Input.GetKeyUp(KeyCode.Q))
            {
                controller.BlinkLeft();
            }
            
            if (Input.GetKeyUp(KeyCode.E))
            {
                controller.BlinkRight();
            }
            

            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                    gearbox.SetGear(Gear.Gear1);
            }
            else if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                    gearbox.SetGear(Gear.Gear2);
            } 
            else if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                    gearbox.SetGear(Gear.Gear3);
            } 
            else if (Input.GetKeyUp(KeyCode.Alpha4))
            {
                    gearbox.SetGear(Gear.Gear4);
            } 
            else if (Input.GetKeyUp(KeyCode.Alpha5))
            {
                    gearbox.SetGear(Gear.Gear5);
            }
            else if (Input.GetKeyUp(KeyCode.R))
            {
                    gearbox.SetGear(Gear.GearR);
            }
            else if (Input.GetKeyUp(KeyCode.N))
            {
                gearbox.SetGear(Gear.GearN);
            } else if (Input.GetKeyUp(KeyCode.H))
            {
                controller.ToggleHeadlights();
            } else if (Input.GetKeyUp(KeyCode.B))
            {
                controller.ToggleHazardLights();
            } else if (Input.GetKeyUp(KeyCode.L))
            {   
                worldController.ToggleTime();
            } else if (Input.GetKeyUp(KeyCode.K))
            {
                worldController.ToggleRain();
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                StartCoroutine(engine.StartEngine());
            }
        }
      
        var rotation = Vector3.zero;
        var eulerAngles = steeringWheel.transform.eulerAngles;

        rotation.x = eulerAngles.x;
        rotation.y = eulerAngles.y;
        rotation.z = -steeringValue * steeringLock;
        steeringWheel.transform.eulerAngles = rotation;
    }
}
