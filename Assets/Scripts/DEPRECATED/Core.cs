//using UnityEngine;
//using System.Collections;
//using KBConstants;

//public class Core : KBGameObject
//{


//    public enum State { acceptingItems, ready };
//    public State state = State.acceptingItems;
//    private TowerInfo towerInfo;

//    private Collider itemCollider;
//    TimerScript timer;
//    int[] itemTimer;
//    public Item[] item;
//    public int numberOfItems;

//    public ItemType itemType; // Temporary
//    AudioClip itemAccept;
//    AudioClip itemFinish;

//    ItemDropZone itemDropZone;

//    public override void Start()
//    {
//        base.Start();
//        towerInfo.itemType = new ItemType[3];
//        itemType = ItemType.undefined;
//        item = new Item[3];
//        itemTimer = new int[3];
//        numberOfItems = 0;
//        targetPosition = transform.position;

//        GameObject model = transform.Find("CoreModel").gameObject;

//        if (teamScript.Team == KBConstants.Team.Blue)
//        {
//            model.renderer.material = Resources.Load<Material>(MaterialConstants.MATERIAL_NAMES[MaterialConstants.type.CoreBlue]);
//        }
//        else
//        {
//            model.renderer.material = Resources.Load<Material>(MaterialConstants.MATERIAL_NAMES[MaterialConstants.type.CoreRed]);
//        }

//        itemAccept = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.FactoryItemAccept]);
//        itemFinish = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.FactoryItemFinish]);

//        if (GetComponent<TimerScript>() == null)
//        {
//            gameObject.AddComponent<TimerScript>();
//        }

//        timer = GetComponent<TimerScript>();

//        Collider[] c = GetComponentsInChildren<Collider>();
//        foreach (Collider d in c)
//        {
//            if (d.gameObject.CompareTag("RangeTrigger"))
//            {
//                itemCollider = d;
//            }
//        }
//        if (itemCollider == null)
//        {
//            Debug.LogError("Factory could not find its itemZone collider");
//        }

//        itemDropZone = (ItemDropZone)GetComponentInChildren<ItemDropZone>();
//    }

//    void Update()
//    {
//        switch (state)
//        {
//            case State.acceptingItems:
//                transform.position = Vector3.Lerp(transform.position, targetPosition, 5.0f * Time.deltaTime);

//                if (numberOfItems == 3)
//                {
//                    state = State.ready;
//                }

//                for (int i = 0; i < item.Length; i++)
//                {
//                    float yOffset = 10.0f;
//                    Vector3 p = itemDropZone.gameObject.transform.position;
//                    if (item[i] != null)
//                    {
//                        switch (i)
//                        {
//                            case 0:
//                                item[i].targetPosition.Set(p.x, p.y + yOffset, p.z);
//                                break;
//                            case 1:
//                                item[i].targetPosition.Set(p.x, p.y + item[i].transform.localScale.y + yOffset, p.z);
//                                break;
//                            case 2:
//                                item[i].targetPosition.Set(p.x, p.y - item[i].transform.localScale.y + yOffset, p.z);
//                                break;
//                            default:
//                                break;
//                        }
//                    }
//                }
//                break;
//            case State.ready:
//                setTowerInfo();
//                GameManager.Instance.SpawnTower(towerInfo);
//                ResetCore();
//                //GameManager.Instance.createObject(ObjectConstants.type.Tower, transform.position, transform.rotation);
//                Destroy(gameObject);
//                break;
//            default:
//                break;
//        }

//    }

//    void OnTriggerEnter(Collider other)
//    {
//        if (other.gameObject.CompareTag("Item"))
//        {
//            Item o = other.gameObject.GetComponent<Item>();
//            if (itemType == ItemType.undefined)
//            {
//                itemType = o.itemType;
//            }

//            if (itemType == o.itemType)
//            {
//                for (int i = 0; i < item.Length; i++)
//                {
//                    if (item[i] == null && o.State != Item.ItemState.isInFactory)
//                    {
//                        item[i] = o;
//                        itemTimer[i] = timer.StartTimer(3.0f);
//                        item[i].State = Item.ItemState.isInFactory;
//                        item[i].StartGrowAnimation();
//                        //particleSystem.Play();
//                        numberOfItems++;
//                        audio.PlayOneShot(itemAccept);
//                    }
//                }
//            }

//        }
//        else
//        {
//            //particleSystem.Stop();
//        }
//    }

//    public void ResetCore()
//    {
//        audio.PlayOneShot(itemFinish);
//        state = State.acceptingItems;
//        numberOfItems = 0;
//        foreach (Item i in item)
//        {
//            if (i != null)
//            {
//                PhotonNetwork.Destroy(i.gameObject);
//            }
//        }
//        item = new Item[3];
//        itemType = ItemType.undefined;
//    }

//    void setTowerInfo()
//    {
//        towerInfo.team = teamScript.Team;
//        towerInfo.spawnLocation = transform.position;
//        towerInfo.spawnRotation = transform.rotation;
//        for (int i = 0; i < towerInfo.itemType.Length; i++)
//        {
//            towerInfo.itemType[i] = item[i].itemType;
//        }
//    }

//    public void setTeam(KBConstants.Team team)
//    {
//        this.teamScript.Team = team;
//        towerInfo.team = team;
//    }
//}
