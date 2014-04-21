using UnityEngine;

public class ShellCasing : MonoBehaviour
{

    private float lifetime = 7.0f;
    public float spawnTime;
    
    private void Start()
    {
    }

    private void Update()
    {
        if (Time.time > spawnTime + lifetime)
        {
            ObjectPool.Recycle(this);
        }
    }
}