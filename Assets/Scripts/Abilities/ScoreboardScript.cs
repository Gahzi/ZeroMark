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

        GoalZone parentZone = transform.parent.gameObject.GetComponent<GoalZone>();

        switch (parentZone.state)
        {
            case GoalZone.GoalState.NotCaptured:
                {
                    if (parentZone.goalScore > 0)
                    {
                        t.text = "Red: " + parentZone.goalScore.ToString();
                    }
                    else if (parentZone.goalScore < 0)
                    {
                        t.text = "Blue: " + parentZone.goalScore.ToString();
                    }
                    else if (parentZone.goalScore == 0)
                    {
                        t.text = "Unoccupied:";
                    }
                }
                break;
            case GoalZone.GoalState.BlueCaptured:
                {
                    t.text = "Blue Captured";
                    break;
                }

            case GoalZone.GoalState.RedCaptured:
                {
                    t.text = "Red Captured";
                    break;
                }
        }
        //t.text = "Red: " + Math.Round((double)transform.parent.gameObject.GetComponent<GoalZone>().goalScore).ToString() + " Blue: " + Math.Round((double) transform.parent.gameObject.GetComponent<GoalZone>().blueTeamTime).ToString();
    }
}
