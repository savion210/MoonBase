using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerStatus : MonoBehaviour
{
    public SC_FPSController controller;
    public Camera main;
    
    [Range(0.0f, 10.0f)]
    public float sustenance = 10.0f;
    
    [Range(0.0f, 10.0f)]
    public float stamina = 10.0f;
    
    [Range(0.0f, 10.0f)]
    public float sanity = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Initializing Default Values
        sustenance = 10.0f;
        stamina = 10.0f;
        sanity = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Sanity
        if (sustenance < 1.0f)
            sanity -= 0.1f * Time.deltaTime;

        if (sanity < 0.0f)
            sanity = 0.0f;

        // Stamina
        if(stamina > 0.5f)
            controller.canMove = true;
        
        if (controller.IsMoving)
        {
            sustenance -= 0.01f * Time.deltaTime;            
            if (stamina > 0.0f)
            {
                stamina -= 0.1f * Time.deltaTime;
            }
            else
            {
                stamina = 0.0f;
                controller.canMove = false;
            }
        }
        else
        {
            if (stamina < 10.0f)
                stamina += 0.15f * Time.deltaTime;
            else
                stamina = 10.0f;
            
            if (stamina < 0.0f)
                stamina = 0.0f;
        }
        
        // Sustenance
        if(sustenance > 0.0f)
            sustenance -= 0.01f * Time.deltaTime;
        else
            sustenance = 0.0f;
    }
    
    void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        
        Debug.DrawRay(main.transform.position, main.transform.forward, Color.black);

        RaycastHit hit;
        if (Physics.Raycast(main.transform.position, main.transform.forward, out hit, 100.0f))
        {
            var deterrent = hit.transform.gameObject.GetComponent<HealthDeterrent>();
            print("There is something in front of the object!  " + hit.transform.gameObject.name);

            if (deterrent != null)
            {
                // TODO: Make the interaction function only be called when pressing 'E' when on this object.
                deterrent.Interaction(this);
            }
        }
    }
}
