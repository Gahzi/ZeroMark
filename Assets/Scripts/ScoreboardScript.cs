using UnityEngine;
using System.Collections;
using System;

public class ScoreboardScript : MonoBehaviour
{

    GameObject target;
    // Use this for initialization
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        transform.LookAt(target.transform);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180f, 0);

        TextMesh t = gameObject.GetComponent<TextMesh>();
        t.text = "Red: " + Math.Round((double)transform.parent.gameObject.GetComponent<GoalZone>().redTeamTime).ToString() + " Blue: " + Math.Round((double) transform.parent.gameObject.GetComponent<GoalZone>().blueTeamTime).ToString();
    }
}
