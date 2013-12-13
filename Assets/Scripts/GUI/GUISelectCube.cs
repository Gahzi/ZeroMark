using UnityEngine;
using System.Collections;

public class GUISelectCube : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null)
        {
            transform.position = transform.parent.position;
            transform.rotation = transform.parent.rotation;

            if (transform.parent.gameObject.CompareTag("Tower"))
            {
                transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
            else
            {
                transform.localScale = new Vector3(5f, 5f, 5f);
            }
        }

    }
}
