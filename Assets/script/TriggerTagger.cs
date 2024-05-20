using UnityEngine;

public class TriggerTagger : MonoBehaviour
{

//    public Vector3 areaMinBounds;
//     public Vector3 areaMaxBounds;

//     // Update is called once per frame
//     void Update()
//     {
//         // Check if the GameObject is within the defined bounds
//         if (transform.position.x >= areaMinBounds.x && transform.position.x <= areaMaxBounds.x &&
//             transform.position.y >= areaMinBounds.y && transform.position.y <= areaMaxBounds.y &&
//             transform.position.z >= areaMinBounds.z && transform.position.z <= areaMaxBounds.z)
//         {
//             // Change the tag
//             gameObject.tag = "plot-1";
//         }
//     }

 public Vector3 areaCenter;
    public Vector3 areaSize;

    // The tag to assign to GameObjects within the area
    private string targetTag = "Plot";

    // Update is called once per frame
    void Update()
    {
        // Create a Bounds object
        Bounds areaBounds = new Bounds(areaCenter, areaSize);

        // Find all GameObjects in the scene
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            // Check if the GameObject's position is within the bounds
            if (areaBounds.Contains(obj.transform.position))
            {
                // Change the tag
                obj.tag = targetTag;
            }
        }
    }
}
