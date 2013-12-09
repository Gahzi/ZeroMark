using UnityEngine;
using System.Collections;
using KBConstants;

public class FactoryGroup : MonoBehaviour
{
    private Factory[] factory;
    private TowerSpawnInfo towerSpawnInfo;
    private Team team;

    void Start()
    {
        towerSpawnInfo.itemType = new ItemType[3];
        
        factory = new Factory[3];
        for (int i = 0; i < factory.Length; i++)
        {
            factory[i] = (Factory) Instantiate(
                Resources.Load<Factory>(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Factory]),
                new Vector3(
                    transform.position.x + 50 * i,
                    transform.position.y,
                    transform.position.z
                    ),
                Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int factoriesReady = 0;
        foreach (Factory f in factory)
        {
            if (f.ready)
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
