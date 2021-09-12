using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] private Transform Door1;
    [SerializeField] private Transform Door2;

    private void FixedUpdate()
    {
        //RaycastHit hit;
        // Ray ray = Camera.main.ScreenPointToRay(gameObject.transform.position); //or whatever you're doing for your ray
        // float distance = 1f; //however far your ray shoots
        // int layerMask = 1 << 7; // "7" here needing to be replaced by whatever layer it is you're wanting to use
        // layerMask = ~layerMask; //invert the mask so it targets all layers EXCEPT for this one


        if (!Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var hit,
            100.0f)) return;

        if (!hit.collider.CompareTag("Door")) return;

        // Open
        // Door1.transform.Rotate(0, -90 * Time.deltaTime, 0, Space.Self);
        // Door2.transform.Rotate(0, 90 * Time.deltaTime, 0, Space.Self);

        return;
        Door1.transform.Rotate(0, -90 * Time.deltaTime, 0, Space.Self);
        Door2.transform.Rotate(0, 90 * Time.deltaTime, 0, Space.Self);

        //if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            Door1.transform.Rotate(0, -90 * Time.deltaTime, 0, Space.Self);
            Door2.transform.Rotate(0, 90 * Time.deltaTime, 0, Space.Self);
        }
        //else
        {
            Door1.transform.Rotate(0, 0 * Time.deltaTime, 0, Space.Self);
            Door2.transform.Rotate(0, 0 * Time.deltaTime, 0, Space.Self);
        }
        
        
    }

    public void OpenDoors()
    {
        Door1.transform.Rotate(0, -90, 0, Space.Self);
        Door2.transform.Rotate(0, 90, 0, Space.Self);
    }
}