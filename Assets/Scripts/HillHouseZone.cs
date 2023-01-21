using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillHouseZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            other.GetComponent<CarController>().taskController.FinishTask(Task.ReachHillHouseTask);
        }
    }
}
