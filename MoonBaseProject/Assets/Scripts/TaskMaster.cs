using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskMaster : MonoBehaviour
{
    private ObjectiveManager obj = null;

    [HideInInspector] public bool requiredGameObject = false;
    [SerializeField] private string objectiveName;
    [SerializeField] private GameObject actualObject = null;
    
    

    // Start is called before the first frame update
    void Start()
    {
        obj = ObjectiveManager.Instance;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag is "Player")
        {
            CheckForRequiredItem();
        }
    }


    private void CompleteObjective()
    {
        obj.SetObjectiveStatus(objectiveName, true);
        actualObject.SetActive(true);
        Destroy(gameObject);
    }

    private void CheckForRequiredItem()
    {
        if (requiredGameObject)
        {
            CompleteObjective();
        }
    }
}