﻿using UnityEngine;

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
        for (int i = 0; i < text.Length; i++)
        {
            if (attachedAbility.Length > 0 && text.Length == attachedAbility.Length)
            {
                text[i].text = attachedAbility[i].ammo.ToString();
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