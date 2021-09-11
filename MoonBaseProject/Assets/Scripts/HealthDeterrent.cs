using UnityEngine;

public class HealthDeterrent : MonoBehaviour
{
    [Range(0.0f, 5.0f)]
    public float staminaHit;
    
    public void Interaction(PlayerStatus controller)
    {
        controller.stamina -= staminaHit;
    }
}
