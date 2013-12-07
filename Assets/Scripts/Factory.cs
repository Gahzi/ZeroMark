using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TimerScript))]
public class Factory : KBGameObject
{

    private Collider itemCollider;
    TimerScript timer;
    int[] itemTimer;
    Item[] item;


    void Start()
    {
        item = new Item[3];
        itemTimer = new int[3];

        if (GetComponent<TimerScript>() == null)
        {
            gameObject.AddComponent<TimerScript>();
        }

        timer = GetComponent<TimerScript>();

        Collider[] c = GetComponentsInChildren<Collider>();
        foreach (Collider d in c)
        {
            if (d.gameObject.CompareTag("RangeTrigger"))
            {
                itemCollider = d;
            }
        }
        if (itemCollider == null)
        {
            Debug.LogError("Factory could not find its itemZone collider");
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < itemTimer.Length; i++)
        {
            if (!timer.IsTimerActive(i))
            {
                if (item[i] != null)
                {
                    Destroy(item[i].gameObject);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            for (int i = 0; i < item.Length; i++)
            {
                if (item[i] == null)
                {
                    item[i] = other.gameObject.GetComponent<Item>();
                    itemTimer[i] = timer.StartTimer(3.0f);
                    item[i].State = Item.ItemState.isInFactory;
                    item[i].targetPosition = new Vector3(transform.position.x, transform.position.y + 10.0f, transform.position.z + 10.0f);
                    particleSystem.Play();
                    break;
                }
            }
        }
        else
        {
            particleSystem.Stop();
        }
    }

}
