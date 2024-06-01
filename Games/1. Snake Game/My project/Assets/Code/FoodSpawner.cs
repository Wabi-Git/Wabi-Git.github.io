using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab; // The food prefab to spawn
    public Transform ground; // The transform of the ground object
    public float padding = 1.0f; // Padding from the edges

    // TEMP: surely I can get this pulled from the code...
    private float groundSizeX;
    private float groundSizeZ;

    // Start is called before the first frame update
    void Start()
    {
        // Get the size of the ground from its transform
        if (ground != null)
        {
            groundSizeX = ground.localScale.x * 5; // Adjust the multiplier as needed
            groundSizeZ = ground.localScale.z * 5; // Adjust the multiplier as needed
        }
        else
        {
            Debug.LogError("Ground Transform is not assigned!");
        }


        // Spawn the first food
        SpawnFood();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnFood()
    {
        // Calculate random position within the ground bounds
        float randomX = Random.Range(-groundSizeX / 2 + padding, groundSizeX / 2 - padding);
        float randomZ = Random.Range(-groundSizeZ / 2 + padding, groundSizeZ / 2 - padding);
        Vector3 spawnPosition = new Vector3(randomX, ground.position.y , randomZ); // Assuming Y position is 0.5 for visibility

        // Instantiate the food at the calculated position
        Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
    }
}
