using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{
    public float MoveSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        
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
        // move forward (multiplication by deltaTime is to account for frame drops)
        transform.position += transform.forward * MoveSpeed * Time.deltaTime;

        // Restarts game if Key "R" pushed
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }
}
