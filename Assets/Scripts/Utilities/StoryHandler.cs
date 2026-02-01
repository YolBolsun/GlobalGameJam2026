using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryHandler : MonoBehaviour
{
    static int unlockProgression = -1;
    static int currStoryScene = 0;
    static int maxStorySceneIndex = 10;
    public static StoryHandler instance;


    private PlayerController playerController;

    [SerializeField] private int combatScene = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevents the object from being destroyed
        }
        else
        {
            Destroy(gameObject); // Destroys duplicate instances
            return;
        }
        
    }

    public static void GoNextScene()
    {
        currStoryScene += 1;
        if (currStoryScene > maxStorySceneIndex)
        {
            currStoryScene = 1;
        }
        SceneManager.LoadScene(currStoryScene);
    }

    private void SetupProgression()
    {
        Debug.Log("SetupProgression " + unlockProgression);
        if(unlockProgression > 0)
        {
            ProgressionPoint1();
        }
        if (unlockProgression > 1)
        {
            ProgressionPoint2();
        }
        if (unlockProgression > 2)
        {
            ProgressionPoint3();
        }
        if (unlockProgression > 3)
        {
            ProgressionPoint4();
        }
        if (unlockProgression > 4)
        {
            ProgressionPoint5();
        }
    }

    private void ProgressionPoint1()
    {
        Debug.Log("ProgressionPoint1 unlock a weapon or some crap");
        playerController.attacks[0].enabled = true;
    }

    private void ProgressionPoint2()
    {
        Debug.Log("ProgressionPoint2 unlock some other shit");
        playerController.movementSpeed *= 1.5f;
    }
    private void ProgressionPoint3()
    {
        Debug.Log("ProgressionPoint3 unlock some other shit");
        playerController.attacks[2].enabled = true;
    }
    private void ProgressionPoint4()
    {
        Debug.Log("ProgressionPoint4 unlock some other shit");
        foreach(PlayerController.AttackData attack in playerController.attacks)
        {
            attack.attackDamage *= 2;
        }
    }
    private void ProgressionPoint5()
    {
        Debug.Log("ProgressionPoint5 unlock some other shit");
        playerController.attacks[3].enabled = true;
    }

    void OnEnable()
    {
        // Subscribe to the sceneLoaded event when the object is enabled
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe when the object is disabled or destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("Scene " + scene.name + " loaded. Executing actions for the persistent object.");
        // Place code here that needs to run on every screen load
        if (combatScene == SceneManager.GetActiveScene().buildIndex)
        {
            //setup combat scene
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            unlockProgression += 1;
            SetupProgression();
        }
    }
}
