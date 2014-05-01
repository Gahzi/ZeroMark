using UnityEngine;
using System.Collections.Generic;

public class BankLocationIndicator : MonoBehaviour {

    GameManager gm;
    List<BankZone> bankZones;
    public BankZone targetedBank;

	// Use this for initialization
	void Start () {
        gm = GameManager.Instance;

        if (gm != null)
        {
            bankZones = gm.bankZones;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (bankZones.Count > 0)
        {
            BankZone currentBank = bankZones[0];
            targetedBank = currentBank;
            Vector3 bankView = currentBank.transform.position;
            Vector3 viewPos = Camera.main.WorldToViewportPoint(bankView);
            viewPos.x = Mathf.Clamp01(viewPos.x);
            viewPos.y = Mathf.Clamp01(viewPos.y);
           
            viewPos = Camera.main.ViewportToWorldPoint(viewPos);
            transform.position = viewPos;
        }
	}
}
