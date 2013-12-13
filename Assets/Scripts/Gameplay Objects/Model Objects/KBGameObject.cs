using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections;
using System.Collections.Generic;
using KBConstants;

[RequireComponent(typeof(Team))]

public class KBGameObject : Photon.MonoBehaviour
{

    public int health;
    public Vector3 targetPosition;
    public TeamScript teamScript;

    protected GameManager gm;

    protected List<KBGameObject> collisionObjects;

    void Awake()
    {
        /*
        if (photonView.isMine)
        {
            this.enabled = true;
        }
         */
        collisionObjects = new List<KBGameObject>();
    }

    public virtual void Start()
    {


        gm = GameManager.Instance;

        if (!GetComponentInChildren<TeamScript>())
        {
            gameObject.AddComponent<TeamScript>();
        }

        teamScript = GetComponentInChildren<TeamScript>();

    }

    void Update()
    {

    }

    public virtual void takeDamage(int amount) { }

    public IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        float i = 0.0f;
        float rate = 1.0f / time;
        while (i < 1.0)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, i);
            yield break;
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        KBGameObject o = (KBGameObject) other.gameObject.GetComponentInChildren<KBGameObject>();
        if (o != null)
        {
            collisionObjects.Add(o);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        KBGameObject o = (KBGameObject) other.gameObject.GetComponentInChildren<KBGameObject>();
        if (o != null)
        {
            collisionObjects.Remove(o);
        }
    }

    public void hasDied(KBGameObject deadObj)
    {
        collisionObjects.Remove(deadObj);
    }

    protected void KBDestroy()
    {
        foreach (KBGameObject o in collisionObjects)
        {
            o.hasDied(this);
        }
    }

}
