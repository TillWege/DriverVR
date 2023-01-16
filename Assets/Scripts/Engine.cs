using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public CarInput input;
    public Gearbox gearBox;
    public CarController controller;
    public AudioSource source;
    public AudioClip start, idle, startNoGear, stall;
  
    public AnimationCurve torqueCurve;
    public float redline = 7500;
    
    public bool running
    {
        get { return _running; }
    }
    private bool _running = false;

    private void Update()
    {
        if (!_running) return;

        var rpm = GetEffectiveRPM();
        source.pitch = Math.Mapf(rpm, 1000, redline, 1, 5);
        source.volume = Math.Mapf(rpm, 1000, redline, 0.6f, 1f);
    }

    public IEnumerator StartEngine()
    {
        if (_running) yield break;
        
        source.volume = 1;
        source.pitch = 1;
        source.loop = false;
        
        if (input.clutchPressed || gearBox.GetCurrentGear() == Gear.GearN)
        {            
            source.clip = start;
            source.Play();
            yield return new WaitForSeconds(source.clip.length);
            _running = true;
            source.clip = idle;
            source.loop = true;
            source.Play();
        }
        else
        {
            source.clip = startNoGear;
            source.loop = false;
            source.Play();
        }
    }

    public void Stall()
    {
        source.clip   = stall;
        source.volume = 1;
        source.pitch  = 1;
        source.loop   = false;
        source.Play();
        _running = false;
    }
    
    public float GetTorque()
    {
        if (!running)
        {
            return 0;
        }

        var torque = torqueCurve.Evaluate(input.gasAxis * 100f);
        return torqueCurve.Evaluate(torque) * 3;
    }

    public float GetEffectiveRPM()
    {
        if (gearBox.GetCurrentGear() == Gear.GearN || (controller.speed < 0.2f))
        {
            return 1000;
        }
        else
        {
            var speed = controller.speed;
            var maxSpeed = gearBox.GetCurrentGear().MaxSpeed();
            var percent = speed / maxSpeed;
            
            return percent * redline;
        }
        
    }
}