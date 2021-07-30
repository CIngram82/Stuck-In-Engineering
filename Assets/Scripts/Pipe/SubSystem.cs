using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum SubSystemsType
{
    Power,
    Engines,
    Weapons,
    Shields,
    Comms,
    Science,
    Sensors,
    Life_Support
}

[RequireComponent(typeof(Pipe))]
public class SubSystem : MonoBehaviour
{
    public SubSystemsType subsystemType;

    public string systemName;
    public bool isPowered = false;
    public bool fullPower = false;
    public float powerUpTime = 2f;
    private float currentPowerTime = 0f;

    public Slider bgSlider;
    public Image powerImg;
    public Sprite icon;
    private Image onSystemCircle;

    public Pipe pipe;

    private void Start()
    {
        systemName = subsystemType.ToString();
        pipe = GetComponent<Pipe>();
        Transform go = transform.GetChild(0);
        onSystemCircle = go.GetComponent<Image>();
        go.GetChild(0).GetComponent<Image>().sprite = icon;

    }

    private void Update()
    {

        // event Listner fire when the pipe isPowered event
        isPowered = pipe.isPowered;

        if (isPowered && !fullPower)
        {
            ChangePowerLevel(Time.deltaTime);
        }
        if (!isPowered && currentPowerTime > 0)
        {
            ChangePowerLevel(-Time.deltaTime);
        }

        if (fullPower && currentPowerTime < powerUpTime)
        {
            fullPower = false;
            EventsManager.On_SubSystem_Status_Change(subsystemType, fullPower);
            powerImg.color = Color.red;
            onSystemCircle.color = Color.red;

        }
        else if (!fullPower && currentPowerTime >= powerUpTime)
        {
            fullPower = true;
            EventsManager.On_SubSystem_Status_Change(subsystemType, fullPower);
            powerImg.color = Color.green;
            onSystemCircle.color = Color.green;

        }
    }

    private void ChangePowerLevel(float delta)
    {
        currentPowerTime += delta;
        currentPowerTime = Mathf.Clamp(currentPowerTime, 0, powerUpTime);
        bgSlider.value = currentPowerTime / powerUpTime;
    }

}

