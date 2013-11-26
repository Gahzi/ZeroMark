using UnityEngine;
using System.Collections;

public class Factory : MonoBehaviour
{

    public bool itemSelected;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            particleSystem.Play();
            Destroy(other.gameObject);
            itemSelected = true;
        }
        else
        {
            particleSystem.Stop();
        }
    }

}
