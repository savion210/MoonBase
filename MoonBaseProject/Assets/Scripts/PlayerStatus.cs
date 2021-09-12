using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public bool debug;
    public SC_FPSController controller;
    public Camera main;
    public PostProcessVolume volume;
    public Radiation radiation;

    public Image sanityImage;
    public Image staminaImage;
    public Image sustenanceImage;

    public Image blindWarning;
    public Image injuryWarning;
    
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
    
    private DepthOfField _dof;
    private Grain _grain;
    private ChromaticAberration _aberration;

    private float _radiationMultiplier;
    
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
        _radiationMultiplier = 1.0f;
        
        blindWarning.gameObject.SetActive(false);    
        injuryWarning.gameObject.SetActive(false);
        
        _dof = volume.sharedProfile.GetSetting<DepthOfField>();
        _grain = volume.sharedProfile.GetSetting<Grain>();
        _aberration = volume.sharedProfile.GetSetting<ChromaticAberration>();
        
        // Initialize their default values, cause these dang things don't save between sessions.
        if (_dof != null)
        {
            _dof.focusDistance.value = 10.0f;
            _dof.aperture.value = 10.0f;
            _dof.focalLength.value = 100.0f;
        }
        else
        {
            Debug.LogError("Depth of Field is Missing!");
        }

        if (_grain != null)
        {
            _grain.colored.value = true;
            _grain.intensity.value = 0.0f;
            _grain.size.value = 0.3f;
            _grain.lumContrib.value = 0.0f;
        }
        else
        {
            Debug.LogError("Grain is Missing!");
        }

        if (_aberration != null)
        {
            _aberration.intensity.value = 0.0f;
            _aberration.fastMode.value = false;
        }
        else
        {
            Debug.LogError("Chromatic Aberration is Missing!");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        ProcessBlur();
        ProcessGrain();
        ProcessAberration();
        ProcessRadiationLevels();
        
        sanityImage.fillAmount = Map(sanity, 0.0f, 10.0f, 0.0f, 1.0f);
        staminaImage.fillAmount = Map(stamina, 0.0f, 10.0f, 0.0f, 1.0f);
        sustenanceImage.fillAmount = Map(sustenance, 0.0f, 10.0f, 0.0f, 1.0f);
        
        // Sanity
        if (sustenance < 1.0f)
            sanity -= 0.1f * Time.deltaTime * _radiationMultiplier;

        if (sanity < 0.0f)
            sanity = 0.0f;

        // Stamina
        if(stamina > 0.5f)
            controller.canMove = true;
        
        if (controller.IsMoving)
        {
            sustenance -= 0.01f * Time.deltaTime * _radiationMultiplier;            
            if (stamina > 0.0f)
            {
                stamina -= 0.1f * Time.deltaTime * _radiationMultiplier;
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
                stamina += 0.25f * Time.deltaTime;
            else
                stamina = 10.0f;
            
            if (stamina < 0.0f)
                stamina = 0.0f;
        }
        
        // Sustenance
        if(sustenance > 0.0f)
            sustenance -= 0.01f * Time.deltaTime * _radiationMultiplier;
        else
            sustenance = 0.0f;
    }

    private void ProcessBlur()
    {
        if (stamina < 5.0f)
        {
            _dof.focusDistance.value = Map(stamina, 5.0f, 0.0f, 10.0f, 4.0f);
            blindWarning.gameObject.SetActive(true);
        }
        else
        {
            blindWarning.gameObject.SetActive(false);
        }
    }

    private void ProcessGrain()
    {
        if (sanity < 7.0f)
        {
            _grain.intensity.value = Map(sanity, 7.0f, 0.0f, 0.0f, 1.0f);
        }

        if (sanity < 5.0f)
        {
            _grain.size.value = Map(sanity, 5.0f, 0.0f, 0.3f, 1.0f);
        }
    }

    private void ProcessAberration()
    {
        if (stamina < 3.0f)
        {
            _aberration.intensity.value = Map(stamina, 3.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    private void ProcessRadiationLevels()
    {
        _radiationMultiplier = radiation.exposureLevel switch
        {
            RadiationExposure.Normal => 1.0f,
            RadiationExposure.Sickness => 1.15f,
            RadiationExposure.Dangerous => 1.30f,
            RadiationExposure.Severe => 1.5f,
            RadiationExposure.Lethal => 2.0f,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    [FormerlySerializedAs("_deterrent")] [HideInInspector] public HealthDeterrent deterrent;
    private string _prevObjectName = string.Empty;

    private void FixedUpdate()
    {
        //Debug.DrawRay(main.transform.position, main.transform.forward, Color.black);

        if (!Physics.Raycast(main.transform.position, main.transform.forward, out var hit, 100.0f)) return;

        if (debug)
        {
            print(hit.collider.tag);
            print("There is something in front of the object!  " + hit.transform.name);
        }

        if (_prevObjectName != hit.transform.name)
        {
            _prevObjectName = hit.transform.name;
            deterrent = hit.transform.gameObject.GetComponent<HealthDeterrent>();
        }
        
        if (deterrent == null) return;
       
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hit.collider.CompareTag("Food"))
            {
                deterrent.Interaction(this);
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

            if (hit.collider.CompareTag("Drink"))
            {
                deterrent.Interaction(this);
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

    private static float Map(float x, float inMIN, float inMAX, float outMIN, float outMAX)
    {
        return (x - inMIN) * (outMAX - outMIN) / (inMAX - inMIN) + outMIN;
    }
}
