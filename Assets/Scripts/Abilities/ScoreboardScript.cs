using UnityEngine;
using System.Collections;
using System;

public class ScoreboardScript : MonoBehaviour
{

    //GameObject target;
    // Use this for initialization
    void Start()
    {
        //target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //if (target == null)
        //{
        //    target = GameObject.FindGameObjectWithTag("Player");
        //}
        //else
        //{
        //    transform.LookAt(target.transform);
        //}
        
        //transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180f, 0);

        TextMesh t = gameObject.GetComponent<TextMesh>();

        CaptureZone parentZone = transform.parent.gameObject.GetComponent<CaptureZone>();

        switch (parentZone.state)
        {
            case CaptureZone.ZoneState.NotCaptured:
                {
                    if (parentZone.captureTotal > 0)
                    {
                        t.text = "Red: " + parentZone.captureTotal.ToString() + "/" + CaptureZone.CAPTURE_REQUIRED;
                    }
                    else if (parentZone.captureTotal < 0)
                    {
                        int s = parentZone.captureTotal * -1;
                        t.text = "Blue: " + s.ToString() + "/" + CaptureZone.CAPTURE_REQUIRED;
                    }
                    else if (parentZone.captureTotal == 0)
                    {
                        t.text = "Unoccupied:";
                    }
                }
                break;
            case CaptureZone.ZoneState.BlueCaptured:
                {
                    t.text = "Blue Captured";
                    break;
                }

            case CaptureZone.ZoneState.RedCaptured:
                {
                    t.text = "Red Captured";
                    break;
                }
        }
        //t.text = "Red: " + Math.Round((double)transform.parent.gameObject.GetComponent<GoalZone>().goalScore).ToString() + " Blue: " + Math.Round((double) transform.parent.gameObject.GetComponent<GoalZone>().blueTeamTime).ToString();
    }
}
