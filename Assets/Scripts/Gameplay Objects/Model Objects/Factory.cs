using UnityEngine;
using System.Collections;
using KBConstants;

[RequireComponent(typeof(TimerScript))]
public class Factory : KBGameObject
{

    public enum State { acceptingItems, ready };
    public State state = State.acceptingItems;

    private Collider itemCollider;
    TimerScript timer;
    int[] itemTimer;
    public Item[] item;
    private int numberOfItems;

    public ItemType itemType; // Temporary
    AudioClip itemAccept;
    AudioClip itemFinish;

    ItemDropZone itemDropZone;

    void Start()
    {
        itemType = ItemType.undefined;
        item = new Item[3];
        itemTimer = new int[3];
        numberOfItems = 0;

        itemAccept = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.FactoryItemAccept]);
        itemFinish = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.FactoryItemFinish]);

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

        itemDropZone = (ItemDropZone) GetComponentInChildren<ItemDropZone>();
    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfItems == 3)
        {
            state = State.ready;
        }

        for (int i = 0; i < item.Length; i++)
        {
            Vector3 p = itemDropZone.gameObject.transform.position;
            if (item[i] != null)
            {
                switch (i)
                {
                    case 0:
                        item[i].targetPosition.Set(p.x, p.y, p.z);
                        break;
                    case 1:
                        item[i].targetPosition.Set(p.x, p.y + item[i].transform.localScale.y, p.z);
                        break;
                    case 2:
                        item[i].targetPosition.Set(p.x, p.y - item[i].transform.localScale.y, p.z);
                        break;
                    default:
                        break;
                }
            }
        }

        //foreach (Item i in item)
        //{
        //    i.targetPosition = itemDropZone.gameObject.transform.position;   
        //}

        //for (int i = 0; i < itemTimer.Length; i++)
        //{
        //    if (!timer.IsTimerActive(i))
        //    {
        //        if (item[i] != null)
        //        {
        //            //Destroy(item[i].gameObject);
        //            //audio.PlayOneShot(itemFinish);
        //        }
        //    }
        //    else
        //    {
        //        item[i].targetPosition = itemDropZone.gameObject.transform.position;
        //    }
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            Item o = other.gameObject.GetComponent<Item>();
            if (itemType == ItemType.undefined)
            {
                itemType = o.itemType;
            }

            if (itemType == o.itemType)
            {
                for (int i = 0; i < item.Length; i++)
                {
                    if (item[i] == null && o.State != Item.ItemState.isInFactory)
                    {
                        item[i] = o;
                        itemTimer[i] = timer.StartTimer(3.0f);
                        item[i].State = Item.ItemState.isInFactory;
                        item[i].StartGrowAnimation();
                        particleSystem.Play();
                        numberOfItems++;
                        audio.PlayOneShot(itemAccept);
                    }
                }
            }

        }
        else
        {
            particleSystem.Stop();
        }
    }

    public void ResetFactory()
    {
        audio.PlayOneShot(itemFinish);
        state = State.acceptingItems;
        numberOfItems = 0;
        foreach (Item i in item)
        {
            if (i != null)
            {
                PhotonNetwork.Destroy(i.gameObject);
            }

        }
    }

}
