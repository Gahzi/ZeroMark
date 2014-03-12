using UnityEngine;

public class WeaponAmmoGUI : MonoBehaviour
{

    public ProjectileAbilityBaseScript[] attachedAbility;
    public TextMesh[] text;
    
    private void Start()
    {

    }

    private void Update()
    {
        for (int i = 0; i < text.Length; i++)
        {
            text[i].text = attachedAbility[i].ammo.ToString();
        }
    }
}