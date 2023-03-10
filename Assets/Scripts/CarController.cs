using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Serialization;

public class CarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos; 
    public float maxBrakeTorque;
    public float maxSteeringAngle;
    
    public CarInput input;
    public Gearbox gearbox;
    public Rigidbody rb;
    public TaskController taskController;
    public LightController lights;
    public TextMesh speedometer;
    public WorldController worldController;

    private bool _blinkingLeft = false;
    public bool blinkingLeft => _blinkingLeft;
    private bool _blinkingRight = false;
    public bool blinkingRight => _blinkingRight;
    private bool _headlights = false;
    private bool _hazardLights = false;
    
    private IntersectionZone _currentZone;
    public IntersectionZone CurrentZone => _currentZone;
    private float _speed;
    public float Speed => _speed;

    
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0) {
            return;
        }
     
        Transform visualWheel = collider.transform.GetChild(0);
     
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        
        var wheelTransform = visualWheel.transform;
     
        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }

    public void Start()
    {
        rb.centerOfMass = new Vector3 (0, -1, 0);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IntersectionZone>())
        {
            _currentZone = other.gameObject.GetComponent<IntersectionZone>();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<IntersectionZone>())
        {
            _currentZone = null;
        }
    }

    public void BlinkLeft()
    {
        _blinkingLeft = !_blinkingLeft;
        lights.SetLeftBlinker(_blinkingLeft);
    }
    
    public void BlinkRight()
    {
        _blinkingRight = !_blinkingRight;
        lights.SetRightBlinker(_blinkingRight);
    }

    public void Headlights()
    {
        _headlights = !_headlights;
        lights.SetHeadlights(_headlights);
        if (worldController.CurrentTime == WorldTime.Night)
            taskController.FinishTask(Task.LightsAtNightTask);
    }
    
    public void ToggleLeftBlinker()
    {
        _blinkingLeft = !_blinkingLeft;
        lights.SetLeftBlinker(_blinkingLeft);
    }
    
    public void ToggleRightBlinker()
    {
        _blinkingRight = !_blinkingRight;
        lights.SetRightBlinker(_blinkingRight);
    }
    
    public void ToggleHeadlights()
    {
        _headlights = !_headlights;
        lights.SetHeadlights(_headlights);
    }
    
    public void FixedUpdate()
    {
        _speed = rb.velocity.magnitude;
        
        if (_speed > 1f)
        {
            taskController.FinishTask(Task.StartDrivingTask);
        }
        else
        {
            if (_currentZone is not null)
            {
                if (_currentZone.isStopSignZone)
                {
                    _currentZone.SetPlayerStoppedAtStopSign(true);
                }
            }
        }
        
        speedometer.text = $"{(int)_speed * 2.5} km/h";

        float torque = 0;
        float brake = 0;
        if ((gearbox.GetCurrentGear() != Gear.GearR) &&(_speed >= (gearbox.GetCurrentGear().MaxSpeed())))
        {
            torque = 0;
        }
        else
        {
            torque = gearbox.GetTorque();
        }

        if ((torque < 0.1f) && (_speed > 1f))
        {
            brake = maxBrakeTorque * 0.25f;
        }
        
        if (input.brakeAxis > 0.1f)
        {
            brake = maxBrakeTorque * input.brakeAxis; 
        }

        float steering = maxSteeringAngle * input.steeringValue;
        foreach (AxleInfo axleInfo in axleInfos) {
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor) {
                axleInfo.leftWheel.motorTorque = torque;
                axleInfo.rightWheel.motorTorque = torque;
            }
            
            axleInfo.rightWheel.brakeTorque = brake;
            axleInfo.leftWheel.brakeTorque = brake;
            
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    public void ToggleHazardLights()
    {
        _hazardLights = !_hazardLights;
        lights.SetHazards(_hazardLights);
        
        if (_speed < 0.1f)
        {
            taskController.FinishTask(Task.ActivateHazardsTask);
        }
    }
}
    
[System.Serializable]
public class AxleInfo
{
    public String name;
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}