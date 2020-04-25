using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private float fixedDeltaTime;
    
    void Awake()
    {
        // Make a copy of the fixedDeltaTime, it defaults to 0.02f, but it can be changed in the editor
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }
    // Start is called before the first frame update
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log("Active Scene is '" + scene.name + "'.");
        if (scene.name == "YouVsVirus_Level2")
        {
         //   Time.timeScale = 0.1f;
            // Adjust fixed delta time according to timescale
            // The fixed delta time will now be 0.02 frames per real-time second
           // Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
