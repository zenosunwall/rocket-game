using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugTools : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Debug.isDebugBuild)
        {
            HandleInputs();
        } 
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings; // Loops scenes
            SceneManager.LoadScene(nextSceneIndex);

        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            GetComponent<Rocket>().TroggleCollision();
        }
    }
}
