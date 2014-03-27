using UnityEngine;

public class FloatingText : MonoBehaviour
{

    private float spawnTime;
    public float lifetime;
    
    private void Start()
    {
    }

    private void Update()
    {
        if (Time.time - spawnTime > lifetime)
        {
            ObjectPool.Recycle(this);
        }
    }

    public void Init()
    {
        spawnTime = Time.time;
    }

    public void Init(float _lifetime)
    {
        Init();
        lifetime = _lifetime;
    }
}