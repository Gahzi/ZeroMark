using UnityEngine;
using System.Collections;
using System;

public class ScoreboardScript : MonoBehaviour
{

    public string txt;
    public Color color;

    void Start()
    {
    }

    void Update()
    {
        TextMesh t = gameObject.GetComponent<TextMesh>();

        CaptureZone parentZone = transform.parent.gameObject.GetComponent<CaptureZone>();

        switch (parentZone.state)
        {
            case CaptureZone.ZoneState.Unoccupied:
                {
                    if (parentZone.captureTotal > 0)
                    {
                        txt = "Red: " + parentZone.captureTotal.ToString() + "/" + CaptureZone.CAPTURE_REQUIRED;
                        color = Color.red;
                    }
                    else if (parentZone.captureTotal < 0)
                    {
                        int i = parentZone.captureTotal * -1;
                        txt = "Blue: " + i.ToString() + "/" + CaptureZone.CAPTURE_REQUIRED;
                        color = Color.blue;
                    }
                    else if (parentZone.captureTotal == 0)
                    {
                        txt = parentZone.captureTotal.ToString() + "/" + CaptureZone.CAPTURE_REQUIRED;
                        color = Color.white;
                    }
                }
                break;
            case CaptureZone.ZoneState.Blue:
                {
                    txt = "Blue Captured";
                    color = Color.blue;
                    break;
                }

            case CaptureZone.ZoneState.Red:
                {
                    txt = "Red Captured";
                    color = Color.red;
                    break;
                }
            default:
                txt = "";
                break;
        }
        t.text = txt;
        t.color = color;
    }
}
