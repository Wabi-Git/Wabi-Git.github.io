using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab; // The food prefab to spawn
    public Transform ground; // The transform of the ground object
    public float padding = 1.0f; // Padding from the edges

    private Vector3 GroundMinBounds;
    private Vector3 GroundMaxBounds;

    void Start()
    {
        if (ground != null)
        {
            Vector3 groundSize = ground.localScale;
            Vector3 groundCenter = ground.position;

            GroundMinBounds = groundCenter - groundSize / 2;
            GroundMaxBounds = groundCenter + groundSize / 2;

            // Debug.Log("Food minX, minZ, maxX, maxZ: " + GroundMinBounds.x + ", " + GroundMinBounds.z + ", " + GroundMaxBounds.x + ", " + GroundMaxBounds.z);

        }
        else
        {
            Debug.LogError("Ground Transform is not assigned!");
        }

        SpawnFood();
    }

    public void SpawnFood()
    {


        float randomX = Random.Range(GroundMinBounds.x + padding, GroundMaxBounds.x - padding);
        float randomZ = Random.Range(GroundMinBounds.z + padding, GroundMaxBounds.z - padding);
        // Magic static number to fix here
        Vector3 spawnPosition = new Vector3(randomX, ground.position.y + 1, randomZ);

        Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
    }
}
