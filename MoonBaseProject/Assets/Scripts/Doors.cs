using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] private Transform Door1;
    [SerializeField] private Transform Door2;


    private Quaternion Door1Open;
    private Quaternion Door1Closed;
    private Quaternion Door2Open;
    private Quaternion Door2Closed;
    
    [SerializeField] float smooth;
    
    private void Start()
    {
        Door1Closed = Door1.transform.rotation;
        Door1Closed = Door2.transform.rotation;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.name.Contains("Door")) return;
        if (Input.GetKeyUp(KeyCode.E))
            StartCoroutine(nameof(PassThruDoor));
    }

    private IEnumerator PassThruDoor()
    {
        // Door1.gameObject.SetActive(false);
        // Door2.gameObject.SetActive(false);

        Door1Open =  Quaternion.Euler(0, -25, 0);
        Door1.rotation = Quaternion.Lerp(Door1Closed, Door1Open, 2f);
        
        Door2Open =  Quaternion.Euler(0, 180, 0);
        Door2.rotation = Quaternion.Lerp(Door2Closed, Door2Open, 2f);


        yield return new WaitForSeconds(2f);
        
        
        Door1.rotation = Quaternion.Lerp(Door1Open, Door1Closed, smooth);
        Door2.rotation = Quaternion.Lerp(Door2Open, Door2Closed, smooth);

        // Door1.gameObject.SetActive(true);
        // Door2.gameObject.SetActive(true);

        StopAllCoroutines();
    }
}