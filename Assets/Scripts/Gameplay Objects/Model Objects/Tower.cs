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
    public static int TOWER_DEFAULT_RANGE = 250; // In global units
    #endregion

    public Team team;
    private ProjectileAbilityBaseScript gun;
    private CapsuleCollider attackRangeTrigger;
    public GameObject target;
    public int maxRange;
    public Vector3 targetPosition;

    // Use this for initialization
    void Start()
    {
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
            if (!gun.GetActive())
            {
                gun.ActivateAbility();
            }
            gun.transform.LookAt(target.transform);
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
        
        if (other.CompareTag("Tower"))
        {
            if (target == null)
            {
                target = other.gameObject;
                Debug.Log("Tower in range");
            }
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            if (target != null)
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
        // TODO : Instantiate some goodies to spawn @ TOD
        PhotonNetwork.Destroy(gameObject);
    }

}
