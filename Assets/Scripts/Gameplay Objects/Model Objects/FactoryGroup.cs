using UnityEngine;
using System.Collections;
using KBConstants;

public class FactoryGroup : MonoBehaviour
{
    public Factory[] factory;
    public Factory[] Factories { get { return factory; } } // TODO This returns null?
    private TowerSpawnInfo towerSpawnInfo;
    private Team team;

    void Start()
    {
        towerSpawnInfo.itemType = new ItemType[3];
        
        factory = new Factory[3];
        for (int i = 0; i < factory.Length; i++)
        {
            GameObject newFac = PhotonNetwork.Instantiate(
                ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Factory],
                new Vector3(
                    transform.position.x + 50 * i,
                    transform.position.y,
                    transform.position.z
                    ),
                transform.rotation,
                0);
            factory[i] = newFac.GetComponent<Factory>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        int factoriesReady = 0;
        foreach (Factory f in factory)
        {
            if (f.state == Factory.State.ready)
            {
                factoriesReady++;
            }
        }
        if (factoriesReady == factory.Length)
        {
            setTowerSpawnInfo();
            GameManager.Instance.SpawnTower(towerSpawnInfo);
            resetFactories();
        }
    }

    void setTowerSpawnInfo()
    {
        towerSpawnInfo.team = team;
        for (int i = 0; i < towerSpawnInfo.itemType.Length; i++)
        {
            towerSpawnInfo.itemType[i] = factory[i].itemType;
        }
    }

    void resetFactories()
    {
        foreach (Factory f in factory)
        {
            f.ResetFactory();
        }
    }
}
