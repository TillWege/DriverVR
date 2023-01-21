using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskController : MonoBehaviour
{
    private readonly List<Task> _openTasks = new List<Task>();
    private readonly List<Task> _completedTasks = new List<Task>();

    public List<Task> OpenTasks => _openTasks;
    public List<Task> CompletedTasks => _completedTasks;
    
    void Start()
    {
        foreach (Task task in System.Enum.GetValues(typeof(Task)))
        {
            _openTasks.Add(task);
        }
    }

    public void FinishTask(Task task)
    {
        // remove task from openTasks
        _openTasks.Remove(task);
        // add task to completedTasks
        _completedTasks.Add(task);
    }
}
