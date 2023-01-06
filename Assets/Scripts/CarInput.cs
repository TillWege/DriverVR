using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CarInput : MonoBehaviour
{
    public InputTypes inputType = InputTypes.LogitechWheel;

    // Update is called once per frame
    void Update()
    {
        StringBuilder deviceName = new StringBuilder(256);
        LogitechGSDK.LogiGetFriendlyProductName(0, deviceName, 256);
        
        if (deviceName.ToString().StartsWith("Logitech"))
        {
            Debug.Log("Wheel");
        }
        else
        {
            //Gamepad Fallback
            Debug.Log("Pad");
        }
    }

    private void HandleWheelInput()
    {
        if (!LogitechGSDK.LogiUpdate() || !LogitechGSDK.LogiIsConnected(0)) return;
        
        LogitechGSDK.DIJOYSTATE2ENGINES rec;
        rec = LogitechGSDK.LogiGetStateUnity(0);
        
        // Check Steering
        {
            // X-Axis pos -32767 -> +32768 (Full Lock)
            
        }
        
        // Pedals
        {
            // Gas
            // Y-Axis pos
            // +32767 = 0%
            // -32768 = 100%
            
            // Brake
            // Z-Axis rotation
            // +32767 = 0%
            // -32768 = 100%
            
            // Clutch
            // Extra Axis pos 1
            // +32767 = 0%
            // -32768 = 100%
        }
        
        // Check Shifting
        {
            // Buttons 12-18 (1st - 6th - reverse)
            
        }
        
        // Check Blinker
        {
            // btn 7 = left
            // btn 6 = right
        }

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
}
