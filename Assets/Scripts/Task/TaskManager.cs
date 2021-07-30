using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] List<Task> tasks;
    [SerializeField] GameObject displayPrefab;
    int taskNumber = 0;
    List<GameObject> tasksDisplays = new List<GameObject>();
    List<Task> activeTasks = new List<Task>();
    List<Task> compleatedTasks = new List<Task>();

    #region Events
    void SubToEvents(bool subscribe)
    {
        EventsManager.TaskCompleated -= OnTaskCompleated;

        if (subscribe)
        {
            EventsManager.TaskCompleated += OnTaskCompleated;
        }
    }

    void OnEnable()
    {
        SubToEvents(true);
    }
    void OnDisable()
    {
        SubToEvents(false);
    }

    #endregion 

    // Start is called before the first frame update
    void Start()
    {
        LoadTask(tasks[taskNumber++]);
    }

    public void LoadTask(Task task)
    {
        GameObject taskDisplay = Instantiate(displayPrefab, transform);
        taskDisplay.GetComponent<TaskDisplay>().task = task;
        tasksDisplays.Add(taskDisplay);
        activeTasks.Add(task);
    }

    public bool RemoveTask(Task task)
    {
        bool taskFound = false;
        for(int i = 0; i < tasksDisplays.Count; i++)
        {
            GameObject display = tasksDisplays[i];
            if (display.GetComponent<TaskDisplay>().task == task)
            {
                taskFound = true;
                tasksDisplays.Remove(display);
                Destroy(display);
                break;
            }
        }
        if (!taskFound) return false;

        activeTasks.Remove(task);
        compleatedTasks.Add(task);
        return true;
    }
    public void OnTaskCompleated(Task task)
    {
        if (!RemoveTask(task))
        {
            Debug.LogError($"Failed to remove task :{task.name}");
        }else if (taskNumber < tasks.Count)
        {
            LoadTask(tasks[taskNumber++]);
        }
    }
}
