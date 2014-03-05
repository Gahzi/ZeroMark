using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{

    public float lifetime;
    private float initTime;

    void Start()
    {
        initTime = Time.time;
    }
    
    private void Update()
    {
        if (Time.time > initTime + lifetime)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}