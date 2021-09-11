using UnityEngine;

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
    private void Start()
    {
        // Initializing Default Values
        sustenance = 10.0f;
        stamina = 10.0f;
        sanity = 10.0f;
    }

    // Update is called once per frame
    private void Update()
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

    private HealthDeterrent _deterrent;
    private string _prevObjectName = string.Empty;

    private void FixedUpdate()
    {
        ///Debug.DrawRay(main.transform.position, main.transform.forward, Color.black);

        if (!Physics.Raycast(main.transform.position, main.transform.forward, out var hit, 100.0f)) return;

        if (_prevObjectName != hit.transform.name)
        {
            _prevObjectName = hit.transform.name;
            _deterrent = hit.transform.gameObject.GetComponent<HealthDeterrent>();
        }

        print("There is something in front of the object!  " + hit.transform.name);

        if (_deterrent == null) return;
        
        // TODO: Add a if statement for if the interaction is unsuccessful.
        if(Input.GetKeyDown(KeyCode.E))
            _deterrent.Interaction(this);
    }
}
