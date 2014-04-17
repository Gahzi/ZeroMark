using UnityEngine;

public class WeaponAmmoGUI : MonoBehaviour
{

    public ProjectileAbilityBaseScript[] attachedAbility;
    public TextMesh[] text;
    
    private void Start()
    {
        attachedAbility = transform.parent.GetComponentsInChildren<ProjectileAbilityBaseScript>();
    }

    private void Update()
    {
        for (int i = 0; i < attachedAbility.Length; i++)
        {
            if (attachedAbility.Length > 0 && text.Length == attachedAbility.Length)
            {
                int ammo = attachedAbility[i].ammo * attachedAbility[i].burstSize;
                text[i].text = ammo.ToString();
            }
            else
            {
                text[i].text = "";
            }
        }
    }

    public void Setup()
    {
        attachedAbility = transform.parent.GetComponentsInChildren<ProjectileAbilityBaseScript>();
    }
}