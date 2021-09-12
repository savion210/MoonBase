using UnityEngine;

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

    private void Start()
    {
        exposureLevel = RadiationExposure.Normal;
        radiationLevel = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        radiationLevel += RadiationPerHour * Time.deltaTime * radiationSpeed;

        if (radiationLevel < 1000)
        {
            exposureLevel = RadiationExposure.Normal;
        }

        if (radiationLevel > 1000 && radiationLevel < 5000)
        {
            exposureLevel = RadiationExposure.Sickness;
        }

        if (radiationLevel >= 5000 && radiationLevel < 6000)
        {
            exposureLevel = RadiationExposure.Dangerous;
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
}
