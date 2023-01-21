public enum Task
{
    NoneTask,
    StartEngineTask,
    StartDrivingTask,
    TurnLeftTask,
    TurnRightTask,
    CrossIntersectionTask,
    CrossStopSignIntersectionTask,
    ShiftGearOnceTask,
    ShiftGearFiveTimesTask,
    ReachHillHouseTask,
    ActivateHazardsTask,
    LightsAtNightTask,
}

static class TaskMethods
{
    public static string GetDescription(this Task task)
    {
        return task switch
        {
            Task.StartEngineTask => "Start the engine",
            Task.StartDrivingTask => "Start driving", 
            Task.TurnLeftTask => "Turn left at an intersection, including setting the correct Blinker",
            Task.TurnRightTask => "Turn right at an intersection, including setting the correct Blinker",
            Task.CrossIntersectionTask => "Safely cross Straight any intersection",
            Task.CrossStopSignIntersectionTask => "Safely cross an intersection with a Stop Sign",
            Task.ShiftGearOnceTask => "Change your gear without Stalling",
            Task.ShiftGearFiveTimesTask => "Change your gear without Stalling 5 times in a Row",
            Task.ReachHillHouseTask => "Reach the house on the hill",
            Task.ActivateHazardsTask => "Test your while standing still Hazards",
            Task.LightsAtNightTask => "Use your Lights at night",
            _ => "Unknown task"
        };
    }
}