using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    private SnakeController snakeController;
    private FoodSpawner foodSpawner;

    void Start()
    {
        snakeController = GetComponentInParent<SnakeController>();
        foodSpawner = FindObjectOfType<FoodSpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with object: " + other.gameObject.name); // Debug statement to check if the method is called

        if (other.CompareTag("Food"))
        {
            Debug.Log("Food collided with: " + other.gameObject.name); // Debug statement to check if the food is detected
            snakeController.GrowSnake();
            Destroy(other.gameObject);
            foodSpawner.SpawnFood();
        }
    }
}
