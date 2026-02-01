using UnityEngine;
using UnityEngine.SceneManagement;

public class StorySceneRunner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeSinceLevelLoad > 2f)
        {
            SceneManager.LoadScene(0);
        }
    }
}
