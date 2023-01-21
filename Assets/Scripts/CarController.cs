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
    
    private float _speed;
    public float speed => _speed;



    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0) {
            return;
        }
     
        Transform visualWheel = collider.transform.GetChild(0);
     
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
     
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void Start()
    {
        rb.centerOfMass = new Vector3 (0, -1, 0);
    }

    public void FixedUpdate()
    {
        _speed = rb.velocity.magnitude;

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

        Debug.Log("torque: " + torque);
        Debug.Log("brake: " + brake);
        
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