using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab; // The food prefab to spawn
    public Transform ground; // The transform of the ground object
    public float padding = 1.0f; // Padding from the edges

    private float groundSizeX;
    private float groundSizeZ;

    void Start()
    {
        if (ground != null)
        {
            groundSizeX = ground.localScale.x * 5; // Adjust the multiplier as needed
            groundSizeZ = ground.localScale.z * 5; // Adjust the multiplier as needed
        }
        else
        {
            Debug.LogError("Ground Transform is not assigned!");
        }

        SpawnFood();
    }

    public void SpawnFood()
    {
        float randomX = Random.Range(-groundSizeX + padding, groundSizeX - padding);
        float randomZ = Random.Range(-groundSizeZ + padding, groundSizeZ - padding);
        Vector3 spawnPosition = new Vector3(randomX, ground.position.y, randomZ);

        Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
    }
}
