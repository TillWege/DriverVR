using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CarInput : MonoBehaviour
{
    public InputTypes inputType = InputTypes.LogitechWheel;
    public int steeringLock = 450;
    public GameObject steeringWheel;
    public Shifter shifter;
    
    
    private string _debugState = "";


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
        string deviceName = "";
        int steeringValue = 0;
        int gasAxis = 0;
        int brakeAxis = 0;
        bool clutchPressed = false;
        bool blinkLeft = false, blinkRight = false;

        if (false && LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            var builder = new StringBuilder(256);
            LogitechGSDK.LogiGetFriendlyProductName(0, builder, 256);

            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);

            // Check Steering
            {
                // X-Axis pos -32767 -> +32768 (Full Lock)
                steeringValue = map(rec.lX, -32768, 32767, -steeringLock, steeringLock);
            }
        
            // Pedals
            {
                // Gas
                // Y-Axis pos
                // +32767 = 0%
                // -32768 = 100%
                gasAxis = map(-rec.lY, -32768, 32767, 0, 100);
                
                // Brake
                // Z-Axis rotation
                // +32767 = 0%
                // -32768 = 100%
                brakeAxis = map(-rec.lRz, -32768, 32767, 0, 100);
                
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
                    shifter.setGear(Gear.Gear1);
                } else if (rec.rgbButtons[13] == 128)
                {
                    shifter.setGear(Gear.Gear2);
                } else if (rec.rgbButtons[14] == 128)
                {
                    shifter.setGear(Gear.Gear3);
                } else if (rec.rgbButtons[15] == 128)
                {
                    shifter.setGear(Gear.Gear4);
                } else if (rec.rgbButtons[16] == 128)
                {
                    shifter.setGear(Gear.Gear5);
                } else if (rec.rgbButtons[18] == 128)
                {
                    shifter.setGear(Gear.GearR);
                }
                else
                {
                    shifter.setGear(Gear.GearN);
                }
    
            }
        
            // Check Blinker
            {
                // btn 7 = left
                blinkLeft = rec.rgbButtons[7] == 128;

                // btn 6 = right
                blinkRight = rec.rgbButtons[6] == 128;
            }
            
        }
        else
        {
            steeringValue = Convert.ToInt32(Input.GetAxis("Steering") * 450);
            gasAxis       = Convert.ToInt32(Input.GetAxis("Gas") * 100);
            brakeAxis     = Convert.ToInt32(Input.GetAxis("Brake") * 100);

            clutchPressed = Input.GetKey(KeyCode.C);
            blinkLeft     = Input.GetKeyUp(KeyCode.Q);
            blinkRight    = Input.GetKeyUp(KeyCode.E);

            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                shifter.setGear(Gear.Gear1);
            }
            else if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                shifter.setGear(Gear.Gear2);
            } 
            else if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                shifter.setGear(Gear.Gear3);
            } 
            else if (Input.GetKeyUp(KeyCode.Alpha4))
            {
                shifter.setGear(Gear.Gear4);
            } 
            else if (Input.GetKeyUp(KeyCode.Alpha5))
            {
                shifter.setGear(Gear.Gear5);
            }
            else if (Input.GetKeyUp(KeyCode.R))
            {
                shifter.setGear(Gear.GearR);
            }
            else if (Input.GetKeyUp(KeyCode.N))
            {
                shifter.setGear(Gear.GearN);
            }
        }
        /*
        {
            //Gamepad Fallback
            
            // Check Steering
            {
                // X-Axis pos -32767 -> +32768 (Full Lock)
            }
        
            // Pedals
            {
                // Gas / Brake
                // Z-Axis pos
                // 0 = nothing pressed
                // 32640 = Full brake
                // -32640 = Full send
                gasAxis = 0;
                brakeAxis = 0;
            
                // Clutch
                // Button 0 (A)
                clutchPressed = rec.rgbButtons[0] == 128;
            }
        
            // Check Shifting
            {
                // Clutch
                // Button 4/5 (Bumper Left(down) / Right(up))
                if (rec.rgbButtons[4] == 128)
                {
                    shifter.setGear(shifter.getCurrentGear().PreviousGear());
                } 
                else if (rec.rgbButtons[5] == 128)
                {
                    shifter.setGear(shifter.getCurrentGear().NextGear());
                }
            }
        
            // Check Blinker
            {
                // btn 2(X) = left
                blinkLeft = rec.rgbButtons[2] == 128;
                // btn 1(B) = right
                blinkRight = rec.rgbButtons[1] == 128;

            }
        }
        */
        var rotation = Vector3.zero;
        rotation.x = steeringValue;
        rotation.y = 0;
        rotation.z = 0;
        steeringWheel.transform.eulerAngles = rotation;

        _debugState = $"Device: {deviceName}\n";
        _debugState += $"Steering: {steeringValue}\n";
        _debugState += $"Gas: {gasAxis}\n";
        _debugState += $"Brake: {brakeAxis}\n";
        _debugState += $"Clutch: {clutchPressed}\n";
        _debugState += $"Blinker Left: {blinkLeft}\n";
        _debugState += $"Blinker Right: {blinkRight}\n";

    }

    private void OnGUI()
    {
        _debugState = GUI.TextArea(new Rect(10, 10, 180, 200), _debugState, 400);
    }

    private void HandleWheelInput()
    {
        if (!LogitechGSDK.LogiUpdate() || !LogitechGSDK.LogiIsConnected(0)) return;
        
        LogitechGSDK.DIJOYSTATE2ENGINES rec;
        rec = LogitechGSDK.LogiGetStateUnity(0);
        


    }

    private void HandleKeyboardInput()
    {
        // Check Steering
        {
            var SteeringInput = Input.GetAxis("Horizontal");
        }
        
        // Pedals
        {            
            var PedalInput = Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.C))
            {
                
            }
        }
        
        // Check Shifting
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                
            } else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                
            } else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                
            } else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                
            } else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                
            } else if (Input.GetKeyDown(KeyCode.N))
            {
                
            } else if (Input.GetKeyDown(KeyCode.R))
            {
                
            }
        }
        
        // Check Blinker
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                
            }            
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                
            }
        }
    }

    private void HandleGamepadInput()
    {
        var SteeringInput = Input.GetAxis("Horizontal");
        var PedalInput = Input.GetAxis("Vertical");
        
        Debug.Log("L/R:"+SteeringInput);
        Debug.Log("U/D:"+PedalInput);
        // Check Steering
        {
        }
        
        // Pedals
        {            
            

            if (Input.GetKeyDown(KeyCode.C))
            {
                
            }
        }
        
        // Check Shifting
        {

        }
        
        // Check Blinker
        {

        }
    }
    
    private static int map(int value, int fromLow, int fromHigh, int toLow, int toHigh) 
    {
        return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
    }
}
