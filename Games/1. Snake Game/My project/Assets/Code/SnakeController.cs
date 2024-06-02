using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    public float MoveSpeed = 5;
    public float SteerSpeed = 180;
    public float BodySpeed = 5;
    public int Gap = 100;
    public int InitialLength = 3;

    public GameObject BodyPrefab;
    public GameObject Head; // Reference to the Head GameObject

    public float ColliderBoxScale = 1.5f;
    private Vector3 ColliderSize; // Variable to store the calculated collider size
    public List<GameObject> BodyParts { get; private set; } = new List<GameObject>();
    private List<Vector3> PositionsHistory = new List<Vector3>();

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
}
