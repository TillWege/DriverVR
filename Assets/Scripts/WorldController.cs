using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WorldController : MonoBehaviour
{
    public Material DayClearSkybox, NightClearSkybox, DayRainSkybox, NightRainSkybox;
    public Light Sun;
    public WorldTime CurrentTime = WorldTime.Day;
    public WorldWeather CurrentWeather = WorldWeather.Clear;
    public List<WheelCollider> wheels;
    public AudioSource RainAudioSource;
    public ParticleSystem RainParticles;

    private void Start()
    {
        UpdateWorld();
    }

    public void ToggleTime()
    {
        if(CurrentTime == WorldTime.Day)
            SetNight();
        else
            SetDay();
        
        UpdateWorld();
    }
    
    private void SetNight()
    {
        CurrentTime = WorldTime.Night;
    }

    private void SetDay()
    {
        CurrentTime = WorldTime.Day;
    }

    public void ToggleRain()
    {
        if(CurrentWeather == WorldWeather.Clear)
            SetRain();
        else
            SetClear();
        
        UpdateWorld();
    }

    private void SetClear()
    {
        CurrentWeather = WorldWeather.Clear;
        RainAudioSource.Stop();
    }
    
    private void SetRain()
    {
        CurrentWeather = WorldWeather.Rain;
        RainAudioSource.Play();
    }

    private void UpdateWorld()
    {
        if (CurrentTime == WorldTime.Day)
        {
            if (CurrentWeather == WorldWeather.Clear)
            {
                RenderSettings.skybox = DayClearSkybox;
                Sun.intensity = 0.8f;
            }
            else
            {
                RenderSettings.skybox = DayRainSkybox;
                Sun.intensity = 0.3f;
            }
        }
        else
        {
            if (CurrentWeather == WorldWeather.Clear)
            {
                RenderSettings.skybox = NightClearSkybox;
                Sun.intensity = 0.2f;
            }
            else
            {
                RenderSettings.skybox = NightRainSkybox;
                Sun.intensity = 0.1f;
            }
        }
        
        RainParticles.gameObject.SetActive(CurrentWeather == WorldWeather.Rain);
        
        SetWheelSettings(CurrentWeather == WorldWeather.Rain ? 0.5f : 1f);
    }

    private void SetWheelSettings(float factor)
    {
        var forwardFriction = new WheelFrictionCurve();
        forwardFriction.extremumSlip = 0.4f * factor;
        forwardFriction.extremumValue = 1f * factor;
        forwardFriction.asymptoteSlip = 0.8f * factor;
        forwardFriction.asymptoteValue = 0.5f * factor;
        forwardFriction.stiffness = 1f;
        
        var sidewaysFriction = new WheelFrictionCurve();
        sidewaysFriction.extremumSlip = 0.25f * factor;
        sidewaysFriction.extremumValue = 1f * factor;
        sidewaysFriction.asymptoteSlip = 0.5f * factor;
        sidewaysFriction.asymptoteValue = 0.75f * factor;
        sidewaysFriction.stiffness = 1f;

        foreach (var wheel in wheels)
        {
            wheel.forwardFriction = forwardFriction;
            wheel.sidewaysFriction = sidewaysFriction;
        }
    }
}

public enum WorldTime
{
    Day,
    Night
}

public enum WorldWeather
{
    Clear,
    Rain
}