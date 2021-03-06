using UnityEngine;
using UnityEngine.UI;

public enum BoneDensity
{
    Normal,
    LowBoneMass,
    Osteoporosis,
    SevereOsteoporosis
}

public class BoneLoss : MonoBehaviour
{
    [Range(1.0f, -4.0f)]
    public float tScore;

    public float speedOfDecay = 1.0f;

    public BoneDensity boneDensityStatus;

    public Image warning;
    
    // Start is called before the first frame update
    private void Start()
    {
        tScore = 1.0f;
        boneDensityStatus = BoneDensity.Normal;
    }

    // Update is called once per frame
    private void Update()
    {
        // Rate of decay 0.5 * 1.62 (Moon Gravity) * Time
        tScore -= 0.5f * (1.62f / 2) * Time.deltaTime * speedOfDecay;

        if (tScore > 1.0f)
            tScore = 1.0f;
        
        if (tScore < -4.0f)
            tScore = -4.0f;
        
        if (tScore > -1.0f)
        {
            boneDensityStatus = BoneDensity.Normal;
            warning.gameObject.SetActive(false);
        }

        if (tScore < -1.0f && tScore > -2.5f)
        {
            boneDensityStatus = BoneDensity.LowBoneMass;
            warning.gameObject.SetActive(false);
        }

        if (tScore < -2.5f && tScore > -3.0f)
        {
            boneDensityStatus = BoneDensity.Osteoporosis;
            warning.gameObject.SetActive(true);
        }

        if (tScore < -3.0f)
        {
            boneDensityStatus = BoneDensity.SevereOsteoporosis;
            warning.gameObject.SetActive(true);
        }
    }
}
