using KBConstants;
using UnityEngine;

public class TypeSelect : MonoBehaviour
{
    public PlayerType type;
    private TextMesh text;

    private void Start()
    {
        text = GetComponentInChildren<TextMesh>();
        if (gameObject.tag.StartsWith("SpawnDrone"))
        {
            if (gameObject.tag.EndsWith("0"))
            {
                text.text = StringConstants.hunterPrefix + StringConstants.hunter0Name;
            }
            else if (gameObject.tag.EndsWith("1"))
            {
                text.text = StringConstants.hunterPrefix + StringConstants.hunter1Name;
            }
            else if (gameObject.tag.EndsWith("2"))
            {
                text.text = StringConstants.hunterPrefix + StringConstants.hunter2Name;
            }
        }
        else if (gameObject.tag.StartsWith("SpawnMech"))
        {
            if (gameObject.tag.EndsWith("0"))
            {
                text.text = StringConstants.mechPrefix + StringConstants.mech0Name;
            }
            else if (gameObject.tag.EndsWith("1"))
            {
                text.text = StringConstants.mechPrefix + StringConstants.mech1Name;
            }
            else if (gameObject.tag.EndsWith("2"))
            {
                text.text = StringConstants.mechPrefix + StringConstants.mech2Name;
            }
        }
        else if (gameObject.tag.StartsWith("SpawnTank"))
        {
            if (gameObject.tag.EndsWith("0"))
            {
                text.text = StringConstants.tankPrefix + StringConstants.tank0Name;
            }
            else if (gameObject.tag.EndsWith("1"))
            {
                text.text = StringConstants.tankPrefix + StringConstants.tank1Name;
            }
            else if (gameObject.tag.EndsWith("2"))
            {
                text.text = StringConstants.tankPrefix + StringConstants.tank2Name;
            }
        }
    }

}