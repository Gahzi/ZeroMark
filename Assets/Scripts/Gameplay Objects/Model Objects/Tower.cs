using UnityEngine;
using System.Collections;
using KBConstants;

/// <summary>
/// Towers are composed of:
///     The tower itself
///         Children:
///             AttackRange
///             HitboxCube
///             (Addons)
/// </summary>
public class Tower : KBGameObject
{
    #region CONSTANTS
    public static int TOWER_DEFAULT_HEALTH = 1000;
    public static int TOWER_DEFAULT_RANGE = 100; // In global units
    #endregion

    TowerInfo towerInfo;
    public TowerInfo TowerInfo
    {
        get { return towerInfo; }
        set { towerInfo = value; }
    }
    private ProjectileAbilityBaseScript gun;
    private CapsuleCollider attackRangeTrigger;
    public GameObject target;
    public int maxRange;
    //public Vector3 targetPosition;

    AudioClip ambient;
    AudioClip death;

    // Use this for initialization
    void Start()
    {
        base.Start();
        ambient = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.TowerAmbient]);
        audio.clip = ambient;
        audio.Play();
        audio.loop = true;

        death = Resources.Load<AudioClip>(AudioConstants.CLIP_NAMES[AudioConstants.clip.TowerDie]);

        transform.position = new Vector3(transform.position.x, transform.localScale.y / 2, transform.position.z);
        targetPosition = transform.position;

        health = TOWER_DEFAULT_HEALTH;
        maxRange = TOWER_DEFAULT_RANGE / 2;

        gun = gameObject.GetComponentInChildren<ProjectileAbilityBaseScript>();

        if (GetComponentInChildren<CapsuleCollider>().CompareTag("RangeTrigger"))
        {
            attackRangeTrigger = GetComponentInChildren<CapsuleCollider>();
        }

        attackRangeTrigger.transform.localScale = new Vector3(TOWER_DEFAULT_RANGE / transform.localScale.x, 3.0f, 0);
        gun.SetMaxRange(maxRange);
    }

    // Update is called once per frame
    void Update()
    {
        //float dx = Mathf.Lerp(transform.position.x, targetPosition.x, 5.0f * Time.deltaTime);
        //float dz = Mathf.Lerp(transform.position.y, targetPosition.y, 5.0f * Time.deltaTime);
        //transform.position.Set(dx, transform.position.y, dz);
        transform.position = Vector3.Lerp(transform.position, targetPosition, 5.0f * Time.deltaTime);

        Vector3 fwd = transform.InverseTransformDirection(Vector3.forward);

        if (target != null)
        {
            if (!target.GetComponent<Tower>().teamScript.Team.Equals(teamScript.Team))
            {
                if (!gun.GetActive())
                {
                    gun.ActivateAbility();
                }
                transform.LookAt(target.transform);
            }
        }
        else
        {
            gun.DeactivateAbility();
        }

        if (health < 0)
        {
            DestroyTower();
        }

    }

    void OnTriggerStay(Collider other)
    {

    }

    void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("Tower"))
        {
            Tower towerInRange = other.GetComponent<Tower>();

            if (target == null && !towerInRange.teamScript.team.Equals(teamScript.team))
            {
                target = other.gameObject;
                Debug.Log("Tower in range");
            }
        }

    }

    void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (other.CompareTag("Tower"))
        {
            Tower towerInRange = other.GetComponent<Tower>();

            if (target != null && !towerInRange.teamScript.team.Equals(teamScript.team))
            {
                target = null;
                gun.DeactivateAbility();
                Debug.Log("Target out of range");
            }
        }
    }

    public override void takeDamage(int amount)
    {
        health -= amount;
    }

    void OnCollisionEnter(Collision collision)
    {
    }

    private void DestroyTower()
    {
        KBDestroy();
        
        // TODO : Make the items not overlap
        // TODO : Items randomize their type @ init so the code below doesn't matter
        //foreach (ItemType t in towerInfo.itemType)
        //{
        //    for (int i = 0; i < 3; i++)
        //    {
        //        GameManager.Instance.createObject(ObjectConstants.type.Item, new Vector3(transform.position.x + Random.Range(-5.0f, 5.0f), 2.0f, transform.position.z + Random.Range(-5.0f, 5.0f)), transform.rotation);
        //    }
        //}

        audio.PlayOneShot(death);

        for (int i = 0; i < 9; i++)
        {
            GameManager.Instance.createObject(ObjectConstants.type.Item, new Vector3(transform.position.x + Random.Range(-5.0f, 5.0f), 2.0f, transform.position.z + Random.Range(-5.0f, 5.0f)), transform.rotation);
        }

        transform.Translate(Vector3.forward * 10000);
        //yield return new WaitForFixedUpdate();
        PhotonNetwork.Destroy(gameObject);
    }

}
