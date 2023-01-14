using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public CarInput input;
    public Gearbox gearBox;
    public AudioSource source;
    public AudioClip start, idle, startNoGear, stall;
  
    public AnimationCurve torqueCurve;
    
    
    // Change of RPM at max throttle (per second)
    public int maxRPMAccel = 4000;
    public int redline = 7200;
    
    public bool running
    {
        get { return _running; }
    }
    private bool _running = false;
    private float _rpm = 0;

    public float Rpm => _rpm;

    private void Update()
    {
        if (!_running) return;
        var throttle = input.gasAxis * 100;
        
        float rpmDiff;
        if (throttle > 1)
        {
            rpmDiff = maxRPMAccel * (throttle / 100f) * Time.deltaTime;
            if (!input.clutchPressed)
            {
                rpmDiff *= gearBox.GetCurrentGear().Factor();
            }
            else
            {
                rpmDiff *= Gear.GearN.Factor();
            }
        }
        else
        {
            if ((input.clutchPressed || gearBox.GetCurrentGear() == Gear.GearN))
            {
                rpmDiff = -maxRPMAccel * Time.deltaTime;
            }
            else
            {
                rpmDiff = -maxRPMAccel * Time.deltaTime * 0.4f;
            }
            
        }
        
        _rpm = Mathf.Max(Mathf.Min(_rpm + rpmDiff, redline), 1000);
        source.pitch = Math.Mapf(_rpm, 1000, redline, 1, 5);
        source.volume = Math.Mapf(_rpm, 1000, redline, 0.6f, 1f);
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
            _rpm = 1000;
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
        _rpm = 0;
    }
    
    public float GetTorque()
    {
        if (_rpm == 0)
        {
            return 0;
        }

        return torqueCurve.Evaluate(Rpm);

    }
}