using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskDisplay : MonoBehaviour
{
    public Task task;
    Dictionary<SubSystemsType, bool> activeSubSystems = new Dictionary<SubSystemsType, bool>();
    [SerializeField] Text nameText;
    [SerializeField] Text descriptionText;
    [SerializeField] Text requirementsText;
    [SerializeField] Image timeRemainingImg;
    [SerializeField] Image chargeTimeImg;

    bool isFilling = false;
    float timeRemaining = 0;
    float fillTime = 0;
    void SubToEvents(bool subscribe)
    {
        EventsManager.SubSystemStatusChanged -= OnSubSystemStatusChange;

        if (subscribe)
        {
            EventsManager.SubSystemStatusChanged += OnSubSystemStatusChange;
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

    public void OnSubSystemStatusChange(SubSystemsType type, bool isActive)
    {
        if (activeSubSystems.ContainsKey(type))
        {
            activeSubSystems[type] = isActive;
        }
        // if no req is false start filling. If any are false dont fill
        isFilling = !activeSubSystems.ContainsValue(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        nameText.text = task.taskName;
        descriptionText.text = task.description;
        
        foreach(SubSystemsType req in task.systemsRequired)
        {
            bool isActive = false;
            // Check and see if subsystem is active
            foreach (SubSystem sub in GameManager.Instance.subSystems)
            {
                if(sub.subsystemType == req)
                {
                    isActive = sub.isPowered;
                    break;
                }
            }
           
            activeSubSystems.Add(req, isActive);
            requirementsText.text += $"{req}\n";
        }
        isFilling = !activeSubSystems.ContainsValue(false);
        requirementsText.text.Trim();
        timeRemainingImg.fillAmount = 1;
        timeRemaining = task.timeLimit;
        chargeTimeImg.fillAmount = 0;

    }

    private void UpdateDisplayCircle(Image img, float time, float maxTime)
    {
        img.fillAmount = time/ maxTime;
    }

    private void Update()
    {
        timeRemaining = Mathf.Max(timeRemaining-Time.deltaTime, 0);
        UpdateDisplayCircle(timeRemainingImg, timeRemaining, task.timeLimit);

        fillTime += isFilling ? Time.deltaTime : -Time.deltaTime;
        fillTime = Mathf.Clamp(fillTime, 0, task.timeToComplete);
        UpdateDisplayCircle(chargeTimeImg, fillTime, task.timeToComplete);

        if(fillTime >= task.timeToComplete)
        {
            EventsManager.On_Task_Compleated(task);
        }

    }




}
