using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseConditions : MonoBehaviour
{
    public BoneLoss boneLoss;

    public Radiation radiation;

    public PlayerStatus status;

    public GameObject loseBanner;

    private void Update()
    {
        if (radiation.exposureLevel == RadiationExposure.Lethal)
        {
            Lost();
        }

        if (boneLoss.boneDensityStatus == BoneDensity.SevereOsteoporosis)
        {
            Lost();
        }

        if (status.sanity <= 0.01)
        {
            Lost();
        }
    }

    private void Lost()
    {
        loseBanner.SetActive(true);
        Time.timeScale = 0.0f;
    }
}
