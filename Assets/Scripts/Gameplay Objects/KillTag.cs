using KBConstants;
using UnityEngine;
using System.Collections;
public class KillTag : KBGameObject
{

    public int pointValue;
    public TextMesh textMesh;
    private float timer;

    public override void Start()
    {
        base.Start();
        //textMesh = GetComponentInChildren<TextMesh>();
    }

    private void OnTriggerEnter(Collider other)
    {
        KBPlayer player = other.gameObject.GetComponent<KBPlayer>();
        if(player != null && player.team != Team.None)
        {
            if (player.team != team && PhotonNetwork.player.Equals(player.networkPlayer))
            {
                player.totalPointsGained += pointValue;
                player.currentPoints += pointValue;

                GameManager.Instance.photonView.RPC("DestroyObject", PhotonTargets.All, photonView.viewID);
                player.audio.PlayOneShot(player.itemPickupClip);
                collider.enabled = false;
                renderer.enabled = false;
                // todo play return sound
            }
            else if (player.team == team && PhotonNetwork.player.Equals(player.networkPlayer))
            {
                if (player.health > 0 && player.type != PlayerType.core)
                {
                    GameManager.Instance.photonView.RPC("DestroyObject", PhotonTargets.All, photonView.viewID);
                    player.audio.PlayOneShot(player.itemPickupClip);
                    collider.enabled = false;
                    renderer.enabled = false;
                }
                

                //player.totalTokensGained += player.killTokens;
                //player.killTokens += Mathf.FloorToInt(pointValue / 2);

                //GameManager.Instance.photonView.RPC("DestroyObject", PhotonTargets.All, photonView.viewID);
                //player.audio.PlayOneShot(player.itemPickupClip);
                //collider.enabled = false;
                //renderer.enabled = false;
                //// todo play return sound
            }
        }
    }

    void Update()
    {
        textMesh.text = pointValue.ToString();

        if (pointValue > 1)
        {
            if (Time.time > timer + GameConstants.pointObjectDecayPeriod)
            {
                gameObject.GetPhotonView().RPC("SetPointValue", PhotonTargets.All, Mathf.RoundToInt((float)pointValue * ((100.0f - (float)GameConstants.pointObjectDecayPercentPerPeriod) / 100.0f)));
                transform.localScale = Vector3.one * (1.0f + ((float)pointValue / 100.0f));
            }
        }
    }

    [RPC]
    private void SetPointValue(int _points)
    {
        pointValue = _points;
        timer = Time.time;
    }

    private void OnPhotonInstantiate(PhotonMessageInfo msg)
    {
        GameManager.Instance.killTags.Add(this);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            int _val = pointValue;
            Vector3 _pos = transform.position;
            Vector3 _scl = transform.localScale;

            stream.Serialize(ref _val);
            stream.Serialize(ref _pos);
            stream.Serialize(ref _scl);
        }
        else
        {
            int _val = 0;
            Vector3 _pos = Vector3.zero;
            Vector3 _scl = Vector3.one;

            stream.Serialize(ref _val);
            stream.Serialize(ref _pos);
            stream.Serialize(ref _scl);

            pointValue = _val;
            transform.position = _pos;
            transform.localScale = _scl;
        }
    }

}