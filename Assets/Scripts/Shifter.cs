using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shifter : MonoBehaviour
{
    public GameObject gear1, gear2, gear3, gear4, gear5, gearR, gearN;

    private void HideAll()
    {
        gear1.SetActive(false);
        gear2.SetActive(false);
        gear3.SetActive(false);
        gear4.SetActive(false);
        gear5.SetActive(false);
        gearR.SetActive(false);
        gearN.SetActive(false);
    }

    public void Show1()
    {
        gear1.SetActive(true);
    }

    public void Show2()
    {
        gear2.SetActive(true);
    }
    
    public void Show3()
    {
        gear3.SetActive(true);
    }

    public void Show4()
    {
        gear4.SetActive(true);
    }

    public void Show5()
    {
        gear5.SetActive(true);
    }

    public void ShowR()
    {
        gearR.SetActive(true);
    }
    
    public void ShowN()
    {
        gearN.SetActive(true);
    }
}
