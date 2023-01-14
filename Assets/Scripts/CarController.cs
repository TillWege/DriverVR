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


        float torque = gearbox.GetTorque();
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
            
            if (input.brakeAxis > 0.1f)
            {
                axleInfo.rightWheel.brakeTorque = maxBrakeTorque * input.brakeAxis; 
                axleInfo.leftWheel.brakeTorque = maxBrakeTorque * input.brakeAxis;
            }
            else
            {
                axleInfo.rightWheel.brakeTorque = 0;
                axleInfo.leftWheel.brakeTorque = 0;
            }
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