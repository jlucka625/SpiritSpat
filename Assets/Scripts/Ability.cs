using UnityEngine;
using System.Collections;

public class Ability : MonoBehaviour
{
    [Tooltip("The ability cooldown time in seconds.")]
    public float cooldownTimeInSeconds;

    [Tooltip("The name of the ability. Useful for debugging.")]
    public string abilityName = "GenericAbility";

    [Tooltip("The name of the button from the InputManager.")]
    public string button;

    private float timeUntilReady = 0;
   
    public Ability()
    {
    }

    // Activate an ability, putting it on cooldown
    protected virtual bool Activate()
    {
        if (GetButton() && IsReady() && !GameOver.paused)
        {
            LogAbilityUse();
            ActivateCooldown();
            return true;
        }

        return false;
    }

    //deactivates ability if it requires to hold down a key
    protected virtual bool Deactivate()
    {
        if (GetButtonUp())
        {
            return true;
        }
        else return false;
    }
    
    // Returns true if the ability is off of cooldown, false otherwise
    private bool IsReady()
    {
        return (Time.time > timeUntilReady);
    }

    private bool GetButton()
    {
        return (Input.GetButton(button));
    }

    private bool GetButtonUp()
    {
        return (Input.GetButtonUp(button));
    }

    private void ActivateCooldown()
    {
        timeUntilReady = Time.time + cooldownTimeInSeconds;
    }

    private void ResetCooldown()
    {
        cooldownTimeInSeconds = 0;
    }

    private void LogAbilityUse()
    {
        Debug.Log("Ability Activated: " + abilityName);
    }
}
