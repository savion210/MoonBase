using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerStatus : MonoBehaviour
{
    public bool debug = false;
    public SC_FPSController controller;
    public Camera main;
    public PostProcessVolume volume;
    
    [Range(0.0f, 10.0f)]
    public float sustenance = 10.0f;
    
    [Range(0.0f, 10.0f)]
    public float stamina = 10.0f;
    
    [Range(0.0f, 10.0f)]
    public float sanity = 10.0f;

    [Range(1.0f, 5.0f)]
    public float water = 5.0f;

    [Range(1.0f, 3.0f)]
    public float food = 3.0f;


    private DepthOfField dof;
    private Grain grain;
    private ChromaticAberration abberation;
    
    // Start is called before the first frame update
    private void Start()
    {
        // Initializing Default Values
        water = 5.0f;
        food = 3.0f;
        sustenance = 10.0f;
        stamina = 10.0f;
        sanity = 10.0f;
        debug = false;

        dof = volume.sharedProfile.GetSetting<DepthOfField>();
        grain = volume.sharedProfile.GetSetting<Grain>();
        abberation = volume.sharedProfile.GetSetting<ChromaticAberration>();
        
        
        // Initialize their default values, cause these dang things don't save between sessions.
        if (dof != null)
        {
            dof.focusDistance.value = 10.0f;
            dof.aperture.value = 10.0f;
            dof.focalLength.value = 100.0f;
        }
        else
        {
            Debug.LogError("Depth of Field is Missing!");
        }

        if (grain != null)
        {
            grain.colored.value = true;
            grain.intensity.value = 0.0f;
            grain.size.value = 0.3f;
            grain.lumContrib.value = 0.0f;
        }
        else
        {
            Debug.LogError("Grain is Missing!");
        }

        if (abberation != null)
        {
            abberation.intensity.value = 0.0f;
            abberation.fastMode.value = false;
        }
        else
        {
            Debug.LogError("Chromatic Aberration is Missing!");
        }
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

    private void ProcessAberration()
    {
        
    }

    public HealthDeterrent _deterrent;
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

        if (debug)
        {
            print(hit.collider.tag);
            print("There is something in front of the object!  " + hit.transform.name);
        }

        if (_deterrent == null) return;

       
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hit.collider.tag == "Food")
            {
                _deterrent.Interaction(this);
                if (sustenance < 10.0f)
                {
                    sustenance += food;
                    hit.collider.gameObject.SetActive(false);
                    if (sustenance > 10.0f)
                    {
                        sustenance = 10.0f;
                    }

                }
            }

            if (hit.collider.tag == "Drink")
            {
                _deterrent.Interaction(this);
                if (sustenance < 10.0f)
                {
                    sustenance += water;
                    hit.collider.gameObject.SetActive(false);
                    if (sustenance > 10.0f)
                    {
                        sustenance = 10.0f;
                    }

                }
            }
        }
            
    }
}
