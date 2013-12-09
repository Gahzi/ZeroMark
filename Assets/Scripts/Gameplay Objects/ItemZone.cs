using UnityEngine;
using System.Collections;
using KBConstants;
/// <summary>
/// Spawns a grid of Items.
/// </summary>
public class ItemZone : MonoBehaviour
{

    private float width, height;
    private int numberOfRows;
    private int numberOfColumns;

    void Start()
    {
        width = 100f;
        height = width;

        numberOfColumns = 5;
        numberOfRows = 5;


    }

    public void GenerateItems()
    {
        for (int i = 0; i < numberOfRows; i++)
        {
            for (int j = 0; j < numberOfColumns; j++)
            {

                PhotonNetwork.Instantiate(
                    ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Item],
                    new Vector3(
                        transform.position.x - (width / 2) + (i * width / numberOfRows),
                        2.0f,
                        transform.position.z - (height / 2) + (j * height / numberOfColumns)),
                    Quaternion.identity, 0);
            }
        }
    }

    void Update()
    {

    }
}
