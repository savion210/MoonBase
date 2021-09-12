using UnityEngine;
using UnityEngine.UI;

public enum RadiationExposure
{
    Normal,     // < 1,000
    Sickness,   // < 5,000
    Dangerous,  // < 6,000
    Severe,     // < 10,000
    Lethal     // >= 10,000
}
public class Radiation : MonoBehaviour
{
    private const float RadiationPerHour = 60.0f;

    [SerializeField] private float radiationLevel;
    public float radiationSpeed = 1.0f;
    public RadiationExposure exposureLevel;

    public Image radiationWarning;

    public GameObject gaugeNeedle;
    private void Start()
    {
        exposureLevel = RadiationExposure.Normal;
        radiationWarning.gameObject.SetActive(false);
        radiationLevel = 0.0f;
    }

    // Update is called once per frame
    private void Update()
    {
        radiationLevel += RadiationPerHour * Time.deltaTime * radiationSpeed;
        ProcessAlpha();
        ProcessGauge();

        if (radiationLevel < 1000)
        {
            exposureLevel = RadiationExposure.Normal;
        }

        if (radiationLevel > 1000 && radiationLevel < 5000)
        {
            exposureLevel = RadiationExposure.Sickness;
            radiationWarning.gameObject.SetActive(true);
        }

        if (radiationLevel >= 5000 && radiationLevel < 6000)
        {
            exposureLevel = RadiationExposure.Dangerous;
            radiationWarning.gameObject.SetActive(true);
        }

        if (radiationLevel >= 6000 && radiationLevel < 10000)
        {
            exposureLevel = RadiationExposure.Severe;
        }

        if (radiationLevel >= 10000)
        {
            exposureLevel = RadiationExposure.Lethal;
        }
    }

    private void ProcessGauge()
    {
        var newRotation = Vector3.zero;
        newRotation.z = PlayerStatus.Map(radiationLevel, 0, 10000, 135, -130);
        gaugeNeedle.transform.localEulerAngles = newRotation;
    }

    private void ProcessAlpha()
    {
        if (radiationLevel > 10000.0f)
        {
            var tempColor = radiationWarning.color;
            tempColor.a = 1.0f;
            radiationWarning.color = tempColor;
        }
        else
        {
            var tempColor = radiationWarning.color;
            tempColor.a = PlayerStatus.Map(radiationLevel, 1000.0f, 10000.0f, 0.0f, 1.0f);
            radiationWarning.color = tempColor;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Station"))
            radiationSpeed = 0.0f;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Station"))
            radiationSpeed = 1.0f;
    }
}
