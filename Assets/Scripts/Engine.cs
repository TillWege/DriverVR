using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public CarInput input;
    public Gearbox gearBox;
    public AudioSource source;
    public AudioClip start, idle, startNoGear, stall;
    
    // Change of RPM at max throttle (per second)
    public int MaxRPMAccel = 4000;
    public int Redline = 7500;

    public bool running
    {
        get { return _running; }
    }
    private bool _running = false;
    private float rpm = 0;

    private void Update()
    {
        if (!_running) return;
        
        var throttle = input.gasAxis;

        //float targetRpm = Map((float)throttle, 0f, 100f, 1000f, Redline * 1.25f);
        
        float rpmDiff;
        if (throttle > 0)
        {
            rpmDiff = MaxRPMAccel * (throttle / 100f) * Time.deltaTime;
            if (!input.clutchPressed)
            {
                rpmDiff *= gearBox.getCurrentGear().Factor();
            }
            else
            {
                rpmDiff *= Gear.GearN.Factor();
            }
        }
        else
        {
            if ((input.clutchPressed || gearBox.getCurrentGear() == Gear.GearN))
            {
                rpmDiff = -MaxRPMAccel * Time.deltaTime;
            }
            else
            {
                rpmDiff = -MaxRPMAccel * Time.deltaTime * 0.1f;
            }
            
        }
        
        rpm = Mathf.Max(Mathf.Min(rpm + rpmDiff, Redline), 1000);
        source.pitch = Map(rpm, 1000, Redline, 1, 5);
        source.volume = Map(rpm, 1000, Redline, 0.6f, 1f);
    }

    public float GetRPM()
    {
        return rpm;
    }

    public IEnumerator StartEngine()
    {
        if (_running) yield break;
        
        source.volume = 1;
        source.pitch = 1;
        source.loop = false;
        
        if (input.clutchPressed || gearBox.getCurrentGear() == Gear.GearN)
        {            
            source.clip = start;
            source.Play();
            yield return new WaitForSeconds(source.clip.length);
            rpm = 1000;
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
        rpm = 0;
    }
    
    private static float Map(float value, float fromLow, float fromHigh, float toLow, float toHigh) 
    {
        return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
    }
}