using UnityEngine;
using System.Collections;

public class RotateCube : MonoBehaviour {

    private Quaternion targetRotation;

	// Use this for initialization
	void Start () {
        targetRotation = new Quaternion(0, 0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.5f * Time.deltaTime);
	}

    void OnGUI()
    {
        if (Event.current.isKey && Event.current.type == EventType.keyDown)
        {
            switch (Event.current.keyCode)
            {
                case KeyCode.A:
                    targetRotation = Quaternion.Euler(0, 90, 0);
                    renderer.material.color = Color.green;
                    break;
                case KeyCode.Z:
                    targetRotation = Quaternion.Euler(90, 0, 0);
                    renderer.material.color = Color.blue;
                    break;
                case KeyCode.S:
                    targetRotation = Quaternion.Euler(0, 0, 90);
                    renderer.material.color = Color.red;
                    break;
                default:
                    break;
            }
        }
    }
}
