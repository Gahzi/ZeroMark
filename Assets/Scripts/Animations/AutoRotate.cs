using UnityEngine;

public class AutoRotate : MonoBehaviour
{

    public Vector3 rotateSpeed;
    
    private void Start()
    {
    }

    private void Update()
    {
        transform.Rotate(rotateSpeed * Time.deltaTime);
    }
}