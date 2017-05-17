using UnityEngine;
using UnityEngine.SceneManagement;

public class Draw : MonoBehaviour
{
    private readonly float maxInk = 200;
    public Texture2D clearButton;
    public AudioClip audioClick;
    public AudioClip audioDraw;
    public float fadeAlpha = 0;


    //GUI
    public Texture2D drawButton;
    private bool drawing = true; //drawing or erasing (true or false)


    public GameObject drawnDot;
    public Texture2D eraseButton;
    public GameObject eraserDot;
    public Texture2D inkBar;
    public Texture2D inkContainer;
    private float inkUsed;
    public Texture2D levelsButton;
    public Texture2D playButton;
    private GameObject player;
    private Vector3 prevPos = new Vector3(-1, -1, -1); //init
    public Texture2D stopButton;
    public Texture2D tutorial;
    public Texture2D button;
    public Texture2D buttonBack;

    public GameObject MainCamera;

    private void Start()
    {
        MainCamera = GameObject.Find("Main Camera");
    }

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
            player = GameObject.FindGameObjectWithTag("Player");

        if (Input.GetMouseButton(0) && (inkUsed < maxInk || !drawing) && !player)
        {
            //MainCamera.GetComponent<AudioSource>().Stop();

            // && !player) { //drawing, goo remains, and not playing.
            var mousePos = Input.mousePosition;
            mousePos.z = 10;
            var pos = Camera.main.ScreenToWorldPoint(mousePos);

            if (prevPos == new Vector3(-1, -1, -1))
            {
                //if first touch
                if (drawing)
                {
                    Instantiate(drawnDot, pos, Quaternion.identity);
                    inkUsed++;
                }
                else
                {
                    Instantiate(eraserDot, pos, Quaternion.identity);
                }

                prevPos = pos;
            }
            else
            {
                //MainCamera.GetComponents<AudioSource>()[1].Play();

                var distance = Vector2.Distance(pos, prevPos);
                if (distance > 0.1)
                {
                    float total;
                    for (var i = total = Mathf.CeilToInt(distance / 0.1f); i > 0; i--)
                    {
                        var drawPos = Vector2.Lerp(prevPos, pos, i / total);
                        if (drawing)
                        {
                            if (inkUsed < maxInk)
                            {
                                Instantiate(drawnDot, new Vector3(drawPos.x, drawPos.y, 0), Quaternion.identity);
                                inkUsed++;
                            }
                            else
                                break;
                        }
                        else
                        {
                            Instantiate(eraserDot, new Vector3(drawPos.x, drawPos.y, 0), Quaternion.identity);
                        }
                    }
                    prevPos = pos;
                }
            }
        }
        else
        {
            prevPos = new Vector3(-1, -1, -1);
            //MainCamera.GetComponents<AudioSource>()[1].Stop();
        }
    }

    public void addInk(int inkToAdd)
    {
        inkUsed -= inkToAdd;
    }

    private void OnGUI()
    {
        if (Application.loadedLevelName == "Level 1")
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tutorial);

        float buttonWidth = Screen.width/8;

        //return to levels button
        //if(GUI.Button (new Rect (Screen.width/24, Screen.width/24, Screen.width / buttonWidth, Screen.width / buttonWidth), levelsButton, GUIStyle.none))
        //		Application.LoadLevel("Level Select");

        //Draw/Erase Buttons 
        var editButton = drawing ? drawButton : eraseButton;
        if (GUI.Button(new Rect(Screen.width - (2 * (buttonWidth + Screen.width / 24)), Screen.width / 24, buttonWidth, buttonWidth), editButton, GUIStyle.none))
        {
            drawing = !drawing;
            MainCamera.GetComponent<AudioSource>().PlayOneShot(audioClick);
        }

        //Play/Stop Buttons
        if (!player)
        {
            //Not playing - no player
            if (GUI.Button(new Rect(Screen.width - (buttonWidth + Screen.width / 24), Screen.width / 24, buttonWidth, buttonWidth), playButton, GUIStyle.none))
            {
                var spawnPos = GameObject.Find("Spawn Point").transform.position;
                spawnPos.y = 1.25f; // += new Vector3(0, 3, 0);
                Instantiate(Resources.Load("Player"), spawnPos, Quaternion.identity); //create player
                Camera.main.GetComponent<LevelSelect>().adRuns++;
                MainCamera.GetComponent<AudioSource>().PlayOneShot(audioClick);
            }
        }
        else if (player)
        {
            //Playing - player exists
            if (GUI.Button(new Rect(Screen.width - (buttonWidth + Screen.width / 24), Screen.width / 24, buttonWidth, buttonWidth), stopButton, GUIStyle.none))
            {
                DestroyObject(player.gameObject);
                MainCamera.GetComponent<AudioSource>().PlayOneShot(audioClick);
            }
        }

        float iconMargin = buttonWidth / 6;
        if (GUI.Button(new Rect(Screen.width / 24, Screen.width / 24, buttonWidth, buttonWidth), button, GUIStyle.none))
        {
            GameObject.Find("Main Camera").GetComponent<LevelSelect>().levelToLoad = "Level Select";
            MainCamera.GetComponent<AudioSource>().PlayOneShot(audioClick);
        }
        GUI.Box(new Rect(Screen.width / 24 + iconMargin, Screen.width / 24 + iconMargin, buttonWidth - 2*iconMargin, buttonWidth - 2*iconMargin), buttonBack, GUIStyle.none);

        inkUsed = Mathf.Clamp(inkUsed, 0, maxInk);

        float barWidth = Screen.width / 8;
        GUI.DrawTexture(new Rect(Screen.width / 24 + (buttonWidth - barWidth)/2, Screen.width / 12 + buttonWidth, barWidth,
                Screen.height / 8), inkContainer, ScaleMode.ScaleToFit);
        GUI.color = new Color(1, 1, 1);
        GUI.DrawTexture(new Rect(Screen.width / 24 + (buttonWidth - barWidth) / 2, Screen.width / 12 + buttonWidth, barWidth,
                (Screen.height / 8) * (maxInk-inkUsed)/maxInk), inkBar, ScaleMode.ScaleToFit);
    }
}