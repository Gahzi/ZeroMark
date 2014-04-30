using UnityEngine;
using System.Collections;
public class KillTag : KBGameObject
{

    public int pointValue;
    public TextMesh textMesh;
    public int decayPercent = 10;
    public int decayPeriod = 3;
    private float timer;

    public override void Start()
    {
        base.Start();
        //textMesh = GetComponentInChildren<TextMesh>();
    }

    private new void OnTriggerEnter(Collider other)
    {
        KBPlayer player = other.gameObject.GetComponent<KBPlayer>();
        if (player != null)
        {
            if (player.team != team && PhotonNetwork.player.Equals(player.networkPlayer))
            {
                player.totalTokensGained += player.killTokens;
                player.killTokens += pointValue;

                GameManager.Instance.photonView.RPC("DestroyObject", PhotonTargets.All, photonView.viewID);
                player.audio.PlayOneShot(player.itemPickupClip);
                collider.enabled = false;
                renderer.enabled = false;
                // todo play return sound
            }
            else if (player.team == team && PhotonNetwork.player.Equals(player.networkPlayer))
            {
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
            if (Time.time > timer + decayPeriod)
            {
                gameObject.GetPhotonView().RPC("SetPointValue", PhotonTargets.AllBuffered, Mathf.RoundToInt((float)pointValue * ((100.0f - (float)decayPercent) / 100.0f)));
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
}