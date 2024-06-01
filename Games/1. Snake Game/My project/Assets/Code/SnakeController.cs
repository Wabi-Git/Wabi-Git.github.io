using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    // LEARNING NOTE: when these are public within a class then the values can be edited from Unity
    public float MoveSpeed = 5;
    public float SteerSpeed = 180;
    public float BodySpeed = 5;
    public int Gap = 100;

    public GameObject BodyPrefab;
    public FoodSpawner foodSpawner;

    public float ColliderBoxScale = 1.5f;

    private Vector3 ColliderSize; // Variable to store the calculated collider size
    private List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> PositionsHistory = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        // Calculate the collider size based on the scale
        ColliderSize = new Vector3(ColliderBoxScale, ColliderBoxScale, ColliderBoxScale);

        // Set the BoxCollider size
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.size = ColliderSize;
        }

        GrowSnake();
        // TEMPORY: we're gonna grow this bitch a lil'
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
    }

    public void RestartGame()
    {
        // Get the current active scene and reload it
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Update is called once per frame
    void Update()
    {
        // Move forward (multiplication by deltaTime is to account for frame drops)
        transform.position += transform.forward * MoveSpeed * Time.deltaTime;

        // Steer
        float steerDirection = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerDirection * SteerSpeed * Time.deltaTime);

        // Store position history
        PositionsHistory.Insert(0, transform.position);

        // Move body parts
        int index = 0;
        foreach (var body in BodyParts)
        {
            // LEARNING NOTE: Gap controls the positions skipped and adjusts the space between the body parts
            // LEARNING NOTE: If the index is greater than the count of all positions stored then take the oldest existing position instead
            Vector3 point = PositionsHistory[Mathf.Min(index * Gap, PositionsHistory.Count - 1)];
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * BodySpeed * Time.deltaTime;
            body.transform.LookAt(point);
            index++;
        }

        // estarts game if Key "R" pushed
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called"); // DEBUG: statement to check if the method is called

        if (other.gameObject.CompareTag("Food"))
        {
            // Call the method to grow the snake
            GrowSnake();

            // Destroy the food object
            Destroy(other.gameObject);

            // Spawn new food
            foodSpawner.SpawnFood();
        }
    }

    private void GrowSnake()
    {
        GameObject body = Instantiate(BodyPrefab);
        BodyParts.Add(body);
    }
}
