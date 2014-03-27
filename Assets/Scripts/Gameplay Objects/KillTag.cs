using UnityEngine;
using System.Collections;
public class KillTag : KBGameObject
{
    public override void Start()
    {
        base.Start();
    }

    private new void OnTriggerEnter(Collider other)
    {
        KBPlayer player = other.gameObject.GetComponent<KBPlayer>();
        if(player != null)
        {
            if (player.team != team && PhotonNetwork.player.Equals(player.networkPlayer))
            {
                player.killTokens *= 2;
                // play pickup sound;


                GameManager.Instance.photonView.RPC("DestroyObject", PhotonTargets.All, photonView.viewID);
                player.audio.PlayOneShot(player.grabSound);
                collider.enabled = false;
                renderer.enabled = false;
                // todo play return sound
            }
        }
    }

    private void OnPhotonInstantiate(PhotonMessageInfo msg)
    {
        GameManager.Instance.killTags.Add(this);
    }

    //private void OnDestroy()
    //{
    //    GameManager.Instance.killTags.Remove(this);
    //}

}