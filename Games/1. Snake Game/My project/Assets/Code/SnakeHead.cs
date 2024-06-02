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
        else if (other.CompareTag("Body"))
        {
            // Only cut the tail if the current length exceeds the initial length
            if (snakeController.BodyParts.Count > snakeController.InitialLength)
            {
                int cutIndex = snakeController.BodyParts.IndexOf(other.gameObject);
                if (cutIndex != -1)
                {
                    snakeController.ShortenSnake(cutIndex);
                }
            }
        }
    }
}
