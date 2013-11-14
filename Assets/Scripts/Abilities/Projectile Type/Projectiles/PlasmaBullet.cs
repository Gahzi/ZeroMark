using UnityEngine;
using System.Collections;

public class PlasmaBullet : ProjectileBaseScript
{

    #region CONSTANTS
    public static float PLASMABULLET_COOLDOWN = 1.0f;
    #endregion

    // Use this for initialization
    void Start()
    {
        base.Start();
        lifetime = PLASMABULLET_COOLDOWN;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

}
