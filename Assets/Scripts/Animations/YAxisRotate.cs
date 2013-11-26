using UnityEngine;
using System.Collections;

public class YAxisRotate : MonoBehaviour
{

    [Range(0.01f, 45.0f)]
    public float rotateSpeed;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }
}
