using UnityEngine;

public class HealthDeterrent : MonoBehaviour
{
    

    [Range(0.0f, 5.0f)]
    public float staminaHit;
    
    public bool Interaction(PlayerStatus controller)
    {
        if (controller.stamina < staminaHit) return false;
        
        controller.stamina -= staminaHit;

        return true;
    }
}
