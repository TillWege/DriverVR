public enum Task
{
    NoneTask,
    ActivateHazardsTask,
    StartEngineTask,
    ShiftGearOnceTask,
    StartDrivingTask,
    ShiftGearFiveTimesTask,
    TurnLeftTask,
    TurnRightTask,
    CrossIntersectionTask,
    CrossStopSignIntersectionTask,
    ReachHillHouseTask,
    LightsAtNightTask,
}

static class TaskMethods
{
    public static string GetDescription(this Task task)
    {
        return task switch
        {
            Task.ActivateHazardsTask => "Test your Hazards while standing still",
            Task.StartEngineTask => "Start the engine",
            Task.ShiftGearOnceTask => "Change your gear without Stalling",
            Task.StartDrivingTask => "Start driving", 
            Task.ShiftGearFiveTimesTask => "Change your gear without Stalling 5 times in a Row",
            Task.TurnLeftTask => "Turn left at an intersection, including setting the correct Blinker",
            Task.TurnRightTask => "Turn right at an intersection, including setting the correct Blinker",
            Task.CrossIntersectionTask => "Safely cross Straight any intersection",
            Task.CrossStopSignIntersectionTask => "Safely cross an intersection with a Stop Sign",
            Task.ReachHillHouseTask => "Reach the house on the hill",
            Task.LightsAtNightTask => "Use your Lights at night",
            _ => "Unknown task"
        };
    }
}