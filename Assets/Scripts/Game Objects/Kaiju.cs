using UnityEngine;
using System.Collections;
using KBConstants;

public class Kaiju : MonoBehaviour {

    //private X attachedPlayer;
    private int[] coreHealth; // red, green, blue, black
    private enum KaijuState { idle };

	void Start () {
        coreHealth = new int[4];

        for (int i = 0; i < coreHealth.Length; i++)
        {
            coreHealth[i] = KaijuConstants.CORE_HEALTH_DEFAULT;
        }
	}
	
	void Update () {
	
	}

    void damageCore(int which, int amount) {
        coreHealth[which] -= amount;

        if (coreHealth[which] < 0)
        {
            coreHealth[which] = 0;
        }
    }
    void activateAbilityOne() { }
    void activateAbilityTwo() { }
    void activateAbilityThree() { }
    void activateAbilityFour() { }
}
