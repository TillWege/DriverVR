public enum Task
{
    StartEngineTask,
    StartDrivingTask,
    BlinkBeforeTurnTask,
    TurnLeftTask,
    TurnRightTask,
    ShiftGearOnceTask,
    ShiftGearFiveTimesTask,
    StopAtStopSignTask,
}

static class TaskMethods
{
    public static string GetDescription(this Task task)
    {
        return task switch
        {
            Task.StartEngineTask => "Start the engine",
            Task.StartDrivingTask => "Start driving",
            Task.BlinkBeforeTurnTask => "Blink before turning",
            Task.TurnLeftTask => "Turn left",
            Task.TurnRightTask => "Turn right",
            Task.ShiftGearOnceTask => "Change your gear without Stalling",
            Task.ShiftGearFiveTimesTask => "Change your gear without Stalling 5 times in a Row",
            Task.StopAtStopSignTask => "Stop at stop sign",
            _ => "Unknown task"
        };
    }
}