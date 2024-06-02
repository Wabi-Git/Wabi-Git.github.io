using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    public float MoveSpeed = 5;
    public float SteerSpeed = 180;
    public float BodySpeed = 5;
    public float FallSpeed = 2f; // Speed at which the snake falls
    public int Gap = 100;
    public int InitialLength = 3;

    public GameObject BodyPrefab;
    public GameObject Head; // Reference to the Head GameObject
    public Transform ground; // Reference to the Ground Transform

    public float ColliderBoxScale = 1.5f;
    public List<GameObject> BodyParts { get; private set; } = new List<GameObject>();

    private Vector3 ColliderSize; // Variable to store the calculated collider size
    private List<Vector3> PositionsHistory = new List<Vector3>();
    private Vector3 GroundMinBounds;
    private Vector3 GroundMaxBounds;

    void Start()
    {
        ColliderSize = new Vector3(ColliderBoxScale, ColliderBoxScale, ColliderBoxScale);
        BoxCollider boxCollider = Head.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.size = ColliderSize;
        }

        for (int i = 0; i < InitialLength; i++)
        {
            GrowSnake();
        }

        // Calculate the map size based on the ground object's scale and position
        if (ground != null)
        {
            Vector3 groundSize = ground.localScale;
            Vector3 groundCenter = ground.position;

            GroundMinBounds = groundCenter - groundSize / 2;
            GroundMaxBounds = groundCenter + groundSize / 2;
            // Debug.Log("Snake minX, minZ, maxX, maxZ: " + GroundMinBounds.x + ", " + GroundMinBounds.z + ", " + GroundMaxBounds.x + ", " + GroundMaxBounds.z);

        }
        else
        {
            Debug.LogError("Ground Transform is not assigned!");
        }
    }

    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    void Update()
    {
        Head.transform.position += Head.transform.forward * MoveSpeed * Time.deltaTime;

        float steerDirection = Input.GetAxis("Horizontal");
        Head.transform.Rotate(Vector3.up * steerDirection * SteerSpeed * Time.deltaTime);

        PositionsHistory.Insert(0, Head.transform.position);

        int index = 0;
        foreach (var body in BodyParts)
        {
            Vector3 point = PositionsHistory[Mathf.Min(index * Gap, PositionsHistory.Count - 1)];
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * BodySpeed * Time.deltaTime;
            body.transform.LookAt(point);
            index++;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        // Check if the head is out of bounds
        if (Head.transform.position.x < GroundMinBounds.x || Head.transform.position.x > GroundMaxBounds.x ||
            Head.transform.position.z < GroundMinBounds.z || Head.transform.position.z > GroundMaxBounds.z)
        {
            StartCoroutine(FallOffMap());
        }
    }

    public void GrowSnake()
    {
        GameObject body = Instantiate(BodyPrefab);
        BodyParts.Add(body);
    }

    public void ShortenSnake(int cutIndex)
    {
        for (int i = cutIndex; i < BodyParts.Count; i++)
        {
            GameObject bodyPart = BodyParts[i];
            StartCoroutine(SinkBodyPart(bodyPart));
        }
        BodyParts.RemoveRange(cutIndex, BodyParts.Count - cutIndex);
    }

    private IEnumerator SinkBodyPart(GameObject bodyPart)
    {
        float sinkDuration = 1f;
        float elapsedTime = 0f;
        Vector3 initialPosition = bodyPart.transform.position;
        Vector3 targetPosition = initialPosition - new Vector3(0, 1, 0);

        while (elapsedTime < sinkDuration)
        {
            bodyPart.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / sinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(bodyPart);
    }

    private IEnumerator FallOffMap()
    {
        while (true)
        {
            Head.transform.position += Vector3.down * FallSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
