using UnityEngine;
using System.Collections;

public class KBSpectator : MonoBehaviour {

    public Camera attachedCamera;
    public float movementSensIncrement = 1.0f;
    public float movementSens = 6.0f;

    public float rotationSens = 3.0f;
    public float rotationSensIncrement = 0.25f;

	// Use this for initialization
	void Start () {
        attachedCamera = gameObject.GetComponent<Camera>();
        GameObject.Find("GlobalAudio").GetComponent<AudioSource>().mute = false;
	}
	
	// Update is called once per frame
	void Update () {
        CheckInputs();
	
	}

    void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            movementSens -= movementSensIncrement;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            movementSens += movementSensIncrement;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            rotationSens -= rotationSensIncrement;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            rotationSens += rotationSensIncrement;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 nextPosition = (camera.transform.forward * (movementSens * v)) + (camera.transform.right * (movementSens * h));

        transform.position = transform.position + nextPosition;

        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * rotationSens;
        float rotationY = transform.localEulerAngles.x + -Input.GetAxis("Mouse Y") * rotationSens;

        transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
    }
}
