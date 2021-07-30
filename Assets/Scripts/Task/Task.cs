using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Task", menuName ="Task")]
public class Task : ScriptableObject
{
    
    public string taskName;

    public string description;

    public float timeLimit;
    public float timeToComplete;
    public List<SubSystemsType> systemsRequired;
    public bool isCompleate;

    
    
}
