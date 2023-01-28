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
    public GameObject leftBlinkerArrow, rightBlinkerArrow;
    
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
        leftBlinkerArrow.SetActive(false);
        rightBlinkerArrow.SetActive(false);
    }
    
    public void SetHeadlights(bool headlights)
    {
        light1.enabled = headlights;
        light2.enabled = headlights;
    }
    
    public void SetLeftBlinker(bool leftBlinker, bool deactivateRight = true)
    {
        if (!leftBlinker)
        {
            if (_leftBlinkerRoutine is not null)
            {
                StopCoroutine(_leftBlinkerRoutine);
            }
            frontLeftBlinker.enabled = false;
            backLeftBlinker.enabled = false;
            leftBlinkerArrow.SetActive(false);
        }
        else
        {
            _leftBlinkerRoutine = LeftBlinkerRoutine();
            StartCoroutine(_leftBlinkerRoutine);
            if(deactivateRight)
                SetRightBlinker(false);
        }
    }

    public void SetRightBlinker(bool rightBlinker, bool deactivateLeft = true)
    {
        if (!rightBlinker)
        {
            if (_rightBlinkerRoutine is not null)
            {
                StopCoroutine(_rightBlinkerRoutine);
            }
            frontRightBlinker.enabled = false;
            backRightBlinker.enabled = false;
            rightBlinkerArrow.SetActive(false);
        }
        else
        {
            _rightBlinkerRoutine = RightBlinkerRoutine();
            StartCoroutine(_rightBlinkerRoutine);
            if(deactivateLeft)
                SetLeftBlinker(false);
        }
    }
    
    public void SetHazards(bool hazards)
    {
        SetLeftBlinker(hazards, false);
        SetRightBlinker(hazards, false);
    }
    
    private IEnumerator LeftBlinkerRoutine()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.5f);
        while (true)
        {
            frontLeftBlinker.enabled = !frontLeftBlinker.enabled;
            backLeftBlinker.enabled = !backLeftBlinker.enabled;
            leftBlinkerArrow.SetActive(backLeftBlinker.enabled);
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
            rightBlinkerArrow.SetActive(backRightBlinker.enabled);
            source.Play();
            yield return waitTime;
        }
    }
}

