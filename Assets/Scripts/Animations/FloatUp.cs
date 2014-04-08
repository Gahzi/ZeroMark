using UnityEngine;

public class FloatUp : MonoBehaviour
{

    public float upSpeed;
    
    private void Start()
    {
    }

    private void Update()
    {
        transform.Translate(new Vector3(0, upSpeed * Time.deltaTime, 0));
    }
}