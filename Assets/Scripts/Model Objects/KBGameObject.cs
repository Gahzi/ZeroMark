using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections;

[RequireComponent(typeof(tk2dSprite))]
[RequireComponent(typeof(PhotonView))]

public class KBGameObject : Photon.MonoBehaviour {
	
	tk2dSprite sprite;
    PhotonView photonView;
	GameManager gm;

    void Awake()
    {
        /*
        if (photonView.isMine)
        {
            this.enabled = true;
        }
         */
    }

	// Use this for initialization
	void Start() 
	{
	 	this.sprite = gameObject.GetComponent<tk2dSprite>();
		gm = GameManager.Instance;

        photonView = this.GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

    /// <summary>
    /// While script is observed (in a PhotonView), this is called by PUN with a stream to write or read.
    /// </summary>
    /// <remarks>
    /// The property stream.isWriting is true for the owner of a PhotonView. This is the only client that
    /// should write into the stream. Others will receive the content written by the owner and can read it.
    /// 
    /// Note: Send only what you actually want to consume/use, too!
    /// Note: If the owner doesn't write something into the stream, PUN won't send anything.
    /// </remarks>
    /// <param name="stream">Read or write stream to pass state of this GameObject (or whatever else).</param>
    /// <param name="info">Some info about the sender of this stream, who is the owner of this PhotonView (and GameObject).</param>
    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
    

}
