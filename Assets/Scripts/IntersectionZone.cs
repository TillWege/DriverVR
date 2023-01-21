using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// ReSharper disable Unity.NoNullPropagation

public class IntersectionZone : MonoBehaviour
{
    public bool isOccupied = false;
    public bool isStopSignZone = false;
    
    public IntersectionZone leftZone;
    public IntersectionZone rightZone;
    public IntersectionZone forwardZone;

    private List<Task> _tasksToBeCompletedOnEntry;
    private bool _playerStoppedAtStopSign = false;
    private bool _playerActivatedLeftBlinker = false;
    private bool _playerActivatedRightBlinker = false;
    
    private CarController _player;

    private void Start()
    {
        _tasksToBeCompletedOnEntry = new List<Task>();
    }
    
    private void Update()
    {
        if (_player is null) return;
        
        if (_player.blinkingLeft)
        {
            _playerActivatedLeftBlinker = true;
        }
            
        if (_player.blinkingRight)
        {
            _playerActivatedRightBlinker = true;
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Car")) return;
        
        _player = other.GetComponent<CarController>();
        
        if (_tasksToBeCompletedOnEntry.Count > 0)
        {
            foreach (var task in _tasksToBeCompletedOnEntry)
            {
                _player.taskController.FinishTask(task);
            }
            
            _tasksToBeCompletedOnEntry.Clear();
        }
        
        isOccupied = true;
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Car")) return;
        
        if (!isStopSignZone || (isStopSignZone && _playerStoppedAtStopSign))
        {
            if (_playerActivatedLeftBlinker)
            {   
                leftZone?.AddTaskToBeCompletedOnEntry(Task.TurnLeftTask);
            }
            
            if (_playerActivatedRightBlinker)
            {
                rightZone?.AddTaskToBeCompletedOnEntry(Task.TurnRightTask);
            }
            
            forwardZone?.AddTaskToBeCompletedOnEntry(Task.CrossIntersectionTask);
            if (isStopSignZone && _playerStoppedAtStopSign)
            {
                leftZone?.AddTaskToBeCompletedOnEntry(Task.CrossStopSignIntersectionTask);
                rightZone?.AddTaskToBeCompletedOnEntry(Task.CrossStopSignIntersectionTask);
                forwardZone?.AddTaskToBeCompletedOnEntry(Task.CrossStopSignIntersectionTask);
            }
        }
        

        ResetZone();
    }
    
    public void AddTaskToBeCompletedOnEntry(Task task)
    {
        _tasksToBeCompletedOnEntry.Add(task);
    }

    private void ResetZone()
    {   
        _playerStoppedAtStopSign     = false;
        _playerActivatedLeftBlinker  = false;
        _playerActivatedRightBlinker = false;
        _player = null;
        isOccupied = false;
    }
    
    public void SetPlayerStoppedAtStopSign(bool value)
    {
        _playerStoppedAtStopSign = value;
    }

    public void SetPlayerActivatedLeftBlinker(bool b)
    {
        _playerActivatedLeftBlinker = b;
    }
    
    public void SetPlayerActivatedRightBlinker(bool b)
    {
        _playerActivatedRightBlinker = b;
    }
}
