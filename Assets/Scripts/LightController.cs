using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Serialization;

public class LightController : MonoBehaviour
{
    public Light light1;
    public Light light2;
    public Light frontLeftBlinker;
    public Light frontRightBlinker;
    public Light backLeftBlinker;
    public Light backRightBlinker;
    public AudioSource source;
    
    private IEnumerator _leftBlinkerRoutine;
    private IEnumerator _rightBlinkerRoutine;
    
    private void Start()
    {
        light1.enabled = false;
        light2.enabled = false;
        frontLeftBlinker.enabled = false;
        frontRightBlinker.enabled = false;
        backLeftBlinker.enabled = false;
        backRightBlinker.enabled = false;
    }
    
    public void SetHeadlights(bool headlights)
    {
        light1.enabled = headlights;
        light2.enabled = headlights;
    }
    
    public void SetLeftBlinker(bool leftBlinker)
    {
        if (!leftBlinker)
        {
            if (_leftBlinkerRoutine is not null)
            {
                StopCoroutine(_leftBlinkerRoutine);
            }
            frontLeftBlinker.enabled = false;
            backLeftBlinker.enabled = false;
        }
        else
        {
            _leftBlinkerRoutine = LeftBlinkerRoutine();
            StartCoroutine(_leftBlinkerRoutine);
            SetRightBlinker(false);
        }
    }

    public void SetRightBlinker(bool rightBlinker)
    {
        if (!rightBlinker)
        {
            if (_rightBlinkerRoutine is not null)
            {
                StopCoroutine(_rightBlinkerRoutine);
            }
            frontRightBlinker.enabled = false;
            backRightBlinker.enabled = false;
        }
        else
        {
            _rightBlinkerRoutine = RightBlinkerRoutine();
            StartCoroutine(_rightBlinkerRoutine);
            SetLeftBlinker(false);
        }
    }
    
    public void SetHazards(bool hazards)
    {
        SetLeftBlinker(hazards);
        SetRightBlinker(hazards);
    }
    
    private IEnumerator LeftBlinkerRoutine()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.5f);
        while (true)
        {
            frontLeftBlinker.enabled = !frontLeftBlinker.enabled;
            backLeftBlinker.enabled = !backLeftBlinker.enabled;
            source.Play();
            yield return waitTime;
        }
    }
    
    private IEnumerator RightBlinkerRoutine()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.5f);
        while (true)
        {
            frontRightBlinker.enabled = !frontRightBlinker.enabled;
            backRightBlinker.enabled = !backRightBlinker.enabled;
            source.Play();
            yield return waitTime;
        }
    }
}

