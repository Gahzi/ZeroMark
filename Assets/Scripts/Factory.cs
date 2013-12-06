using UnityEngine;
using System.Collections;

public class Factory : KBGameObject
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
        if (other.gameObject.CompareTag("Item")) // TODO This is probably broken
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
