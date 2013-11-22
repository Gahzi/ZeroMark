using UnityEngine;
using System.Collections;
using KBConstants;

public class SpawnBoxes : MonoBehaviour {

    public int numberOfCubes;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < numberOfCubes; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject.Instantiate(Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.BasicRigidbodyCube]), new Vector3(i, 10 + j, 0), Quaternion.identity);
            }

        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
