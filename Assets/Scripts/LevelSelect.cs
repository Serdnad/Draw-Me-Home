using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public int adRuns = 0;
    public AudioClip audioClick;
    public Texture2D audioButton;
    public Texture2D audioButtonMuted;
    public Texture2D backButton;

    //public GUIStyle fontLevelSelect;
    public Texture2D buttonLevel;
    public Texture2D buttonLevelLocked;
    private float fadeAlpha = 0;
    public GUIStyle levelText;
    public string levelToLoad = "";
    public AudioListener listener;
    public GUIStyle titleText;

    public AsyncOperation nextScene = null;

    //void showAd(CBLocation CBL) {
    //	Chartboost.showInterstitial (CBL);

    //	//Debug.Log ("Show ad");
    //	//if (Chartboost.didDisplayInterstitial (CBL)) //mainly for video
    //	//	AudioListener.pause = true;
    //}

    private void Awake()
    {
        //PlayerPrefs.DeleteAll (); //STRICTLY FOR DEBUGGING/TESTING PURPOSES ONLY. REMEMBER TO DISABLE/REPLACE WITH PROPER OPTION IN-GAME
        if (GameObject.FindGameObjectsWithTag("MainCamera").Length > 1) //If camera already exists at creation, destroy this copy
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        titleText.fontSize = levelText.fontSize = Screen.height / 16;
        listener.enabled = true;
        AudioListener.pause = PlayerPrefs.GetInt("muted", -1) == 1 ? true : false;
    }

    private void OnLevelWasLoaded(int level)
    {
        //Analytics.CustomEvent("Level Loaded", new Dictionary<string, object> {{ "Level Name", Application.loadedLevelName }});

        //if (Application.loadedLevel > 4 && Application.loadedLevel % 2 == 1) //changed to display every few runs
        //	showAd(CBLocation.LevelStart);
        fadeAlpha = 0;
        levelToLoad = "";
        //var sceneLevelSelect = SceneManager.LoadSceneAsync("Level Select");
        //sceneLevelSelect.allowSceneActivation = false;
        //SceneManager.LoadSceneAsync("Level " + SceneManager.GetActiveScene().buildIndex);
    }

    private void OnGUI()
    {
        //Rect buttonPosition = new Rect (Screen.width / 3.25f * i + Screen.width / 11.5f, Screen.height / 5 * j + Screen.width / 12, Screen.width / 5, Screen.width / 5);
        if (Application.loadedLevelName == "Level Select")
        {
            float lockmargin = Screen.width / 24; 

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    float posx = Screen.width / 3.25f * i + Screen.width / 11.5f;
                    float posy = Screen.height / 5 * j + Screen.width / 12;

                    //rows
                    if (GUI.Button(new Rect(posx, posy, Screen.width / 5, Screen.width / 5), buttonLevel, GUIStyle.none))
                    {
                        if (PlayerPrefs.GetInt("Current Level", 0) >= i + j * 3 || i + j * 3 + 1 == 1)
                        {
                            //Locked if previous not beaten
                            levelToLoad = "Level " + (i + j * 3 + 1);
                        }
                        AudioSource.PlayClipAtPoint(audioClick, this.transform.position, 100);
                    }


                    if (PlayerPrefs.GetInt("Current Level", 0) < i + j * 3 && i + j * 3 + 1 != 1) // draw lock if previous not beat (locked)
                        GUI.Box(new Rect(posx + lockmargin, posy + lockmargin, Screen.width / 5 - 2*lockmargin, Screen.width / 5 - 2*lockmargin), buttonLevelLocked, levelText);
                    else
                        GUI.Box(new Rect(posx, posy, Screen.width / 5, Screen.width / 5), (i + j * 3 + 1).ToString(), levelText); //, GUIStyle.none);
                }
            }


            float buttonWidth = Screen.width / 6;
            float iconMargin = buttonWidth / 6;
            if (GUI.Button(new Rect(Screen.width / 24, Screen.height - (buttonWidth + Screen.width / 24), buttonWidth, buttonWidth), buttonLevel, GUIStyle.none))
            {
                levelToLoad = "Main Menu";
                AudioSource.PlayClipAtPoint(audioClick, this.transform.position, 100);
            }
            GUI.Box(new Rect(Screen.width / 24 + iconMargin, Screen.height - (buttonWidth - iconMargin + Screen.width / 24), buttonWidth - 2 * iconMargin, buttonWidth - 2 * iconMargin), backButton, GUIStyle.none);


            var audioStatus = AudioListener.pause ? audioButtonMuted : audioButton;
            if (GUI.Button(new Rect(Screen.width - (buttonWidth + Screen.width / 24), Screen.height - (buttonWidth + Screen.width / 24), buttonWidth, buttonWidth), buttonLevel, GUIStyle.none))
            {
                AudioListener.pause = !AudioListener.pause;
                AudioSource.PlayClipAtPoint(audioClick, this.transform.position, 100);
                PlayerPrefs.SetInt("muted", -PlayerPrefs.GetInt("muted", -1));
            }
            GUI.Box(new Rect(Screen.width - (buttonWidth - iconMargin + Screen.width / 24), Screen.height - (buttonWidth - iconMargin + Screen.width / 24), buttonWidth - 2 * iconMargin, buttonWidth - 2 * iconMargin), audioStatus, levelText);
        }
        else if (Application.loadedLevelName == "Main Menu")
        {
            if (GUI.Button(new Rect(Screen.width / 10, Screen.height / 2 - Screen.height / 14, Screen.width - Screen.width / 5, Screen.height / 7), "let's play!", titleText) || GUI.Button(new Rect(0, 0, Screen.width, Screen.height), "", GUIStyle.none))
            {
                levelToLoad = "Level Select";
                AudioSource.PlayClipAtPoint(audioClick, this.transform.position, 100);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }

        if (Application.loadedLevelName.Contains("Level") && Application.loadedLevelName != "Level Select")
        {
            //if(GUI.Button (new Rect (Screen.width/24, Screen.width/24, Screen.width / 8, Screen.width / 8), buttonLevel, levelText))
            //         {
            //             fadeAlpha = 0;
            //             levelToLoad = "Level Select";
            //         }
            //         GUI.Box(new Rect(Screen.width / 24, Screen.width / 24, Screen.width / 8, Screen.width / 8), backButton, levelText);
        }

        if (Application.loadedLevelName.Contains("Level") && Application.loadedLevelName != "Level Select" && Input.GetKeyDown(KeyCode.Escape))
        {
            levelToLoad = "Level Select";
        }

        var fadeBlack = Resources.Load("white") as Texture2D;
        GUI.color = new Color(0, 0, 0, fadeAlpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeBlack);


        if (levelToLoad != "")
        {
            if (nextScene == null)
            {
                nextScene = SceneManager.LoadSceneAsync(levelToLoad);
                nextScene.allowSceneActivation = false;
            }

            fadeAlpha += 0.06f;
            if (fadeAlpha >= 1f)
            {
                nextScene.allowSceneActivation = true;
                levelToLoad = "";
                nextScene = null;
            }
        }

    }

    private void OnApplicationPause()
    {
        //if exitted, quit.
        //Time.timeScale = 0;
        //AudioListener.pause = true;
    }
}