using UnityEngine;
using System.Collections.Generic;
using KBConstants;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(TimerScript))]

/// <summary>
/// Abstract class for abilities.
/// 
/// Contains audoSource, timer & cooldown.
/// </summary>
abstract public class AbilitySlotBaseScript : MonoBehaviour
{

    public AudioClip sound;
    protected float cooldown;
    protected float cooldownStart;
    //public bool abilityActive;
    private Team team;
    public PlayerLocal owner;

    public Team Team
    {
        get
        {
            return team;
        }
        set
        {
            team = value;
        }
    }

    public virtual void Start()
    {
        if (gameObject.GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent("AudioSource");
        }
        if (gameObject.GetComponent<TimerScript>() == null)
        {
            gameObject.AddComponent("TimerScript");
        }
        audio.clip = sound;
        cooldown = 0;
        //abilityActive = false;

        //abilityProps = new List<AbilityConstants.properties>();
    }

    public virtual void FixedUpdate()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        cooldown = Mathf.Clamp(cooldown, 0.0f, 100.0f);
    }

    //public void ActivateAbility() { abilityActive = true; }
    //public void DeactivateAbility() { abilityActive = false; }
    //public void ToggleAbility() { abilityActive = !abilityActive; }
    //public bool GetActive() { return abilityActive; }

    //public virtual T ActivateAbility<T>() { return default(T); }
    //public virtual T ActivateAbility<T>(int maxRange) { return default(T); }
    //public virtual T ActivateAbility<T>(Vector3 direction) { return default(T); }


}
