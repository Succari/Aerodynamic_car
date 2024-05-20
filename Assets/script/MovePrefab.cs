using UnityEngine;

public class MovePrefab : MonoBehaviour
{
    private float moveSpeed = 0.5f; // The step size

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0, moveSpeed* Time.deltaTime); // Move along the x-axis
         //transform.position += new Vector3(moveSpeed, 0, 0); // Move along the x-axis
    }
}
