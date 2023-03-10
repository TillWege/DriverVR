using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskController : MonoBehaviour
{
    private readonly List<Task> _openTasks = new List<Task>();
    private readonly List<Task> _completedTasks = new List<Task>();
    public TextMesh taskText;

    public List<Task> OpenTasks => _openTasks;
    public List<Task> CompletedTasks => _completedTasks;
    
    private int _ShiftingTaskProgress = 0;

    void Start()
    {
        foreach (Task task in System.Enum.GetValues(typeof(Task)))
        {
            if (task is not Task.NoneTask)
                _openTasks.Add(task);
        }
        UpdateTaskText();
    }

    public void FinishTask(Task task)
    {
        // remove task from openTasks
        _openTasks.Remove(task);
        // add task to completedTasks
        if (!_completedTasks.Contains(task))
            _completedTasks.Add(task);

        UpdateTaskText();
    }
    
    public void IncreaseShiftingTaskProgress()
    {
        _ShiftingTaskProgress++;
        FinishTask(Task.ShiftGearOnceTask);
        if (_ShiftingTaskProgress == 5)
        {
            FinishTask(Task.ShiftGearFiveTimesTask);
        }
    }
    
    public void ResetShiftingTaskProgress()
    {
        _ShiftingTaskProgress = 0;
    }
    
    private void UpdateTaskText()
    {
        if (_openTasks.Count > 0)
        {
            taskText.text = _openTasks[0].GetDescription();
        }
        else
        {
            taskText.text = "No tasks left!";
        }
    }
}
