using System;

using UnityEngine;

public class EventsManager : MonoBehaviour
{
    #region Event Actions
    public static Action<SubSystemsType, bool> SubSystemStatusChanged;
    public static Action<Task> TaskCompleated;
    #endregion


    #region Event Calls

    public static void On_SubSystem_Status_Change(SubSystemsType type, bool isActive)
    {
        SubSystemStatusChanged?.Invoke(type, isActive);
    }

    public static void On_Task_Compleated(Task task)
    {
        TaskCompleated?.Invoke(task);
    }


    #endregion
}
