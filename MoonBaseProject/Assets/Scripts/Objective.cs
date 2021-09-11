using UnityEngine;

[CreateAssetMenu(fileName = "Objective")]
public class Objective : ScriptableObject
{
    [Tooltip("Name of the objective")]
    public string objectiveName;
    [Tooltip("Objective status")]
    public bool objectiveStatus;
}
