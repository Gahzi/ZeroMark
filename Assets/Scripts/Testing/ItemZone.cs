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

        for (int i = 0; i < numberOfRows; i++)
        {
            for (int j = 0; j < numberOfColumns; j++)
            {
                GameObject obj = (GameObject) GameObject.Instantiate(
                    Resources.Load(ObjectConstants.PREFAB_NAMES[ObjectConstants.type.Item]),
                    new Vector3(
                        transform.position.x - (width/2) + (i * width/numberOfRows),
                        2.0f,
                        transform.position.z - (height/2) + (j * height/numberOfColumns)),
                    Quaternion.identity);
                obj.transform.Rotate(Vector3.forward, 45);
                obj.transform.Rotate(Vector3.right, 33.3f);

            }
        }
    }

    void Update()
    {

    }
}
