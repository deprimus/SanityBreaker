using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool paused;
    public static bool started;

    public static Ball ball;
    public static Player player;
    private static new Camera camera;

    private static GameObject pauseMenu;
    private static GameObject startMenu;

    public static GameObject crizatusCanvas;
    public static GameObject nervosusCanvas;

    private static readonly bool PAUSE_ALLOWED = false;

    void Start()
    {
        paused = false;
        started = false;

        Time.timeScale = 0f;

        ball = FindObjectOfType<Ball>();
        player = FindObjectOfType<Player>();
        camera = FindObjectOfType<Camera>();

        SetupWalls();

        pauseMenu = GameObject.FindGameObjectsWithTag("PauseMenu")[0];
        startMenu = GameObject.FindGameObjectsWithTag("StartMenu")[0];

        crizatusCanvas = GameObject.FindGameObjectsWithTag("CrizatusCanvas")[0];
        nervosusCanvas = GameObject.FindGameObjectsWithTag("NervosusCanvas")[0];

        pauseMenu.SetActive(false);
        startMenu.SetActive(true);

        crizatusCanvas.SetActive(false);
        nervosusCanvas.SetActive(false);
    }

    void Update()
    {
        if(started)
        {
            if(!paused && (Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(0)))
                Reset();
            else
            {
                if (PAUSE_ALLOWED && (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)))
                    TogglePause();
                if (Input.GetMouseButtonDown(1))
                {
                    player.isAI = !player.isAI;
                    SoundManager.Play(SoundManager.Clip.SELECTION);
                }
            }
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
                ToggleStarted();
        }
    }

    void SetupWalls()
    {
        GameObject leftWall = GameObject.FindGameObjectsWithTag("WallLeft")[0];
        GameObject rightWall = GameObject.FindGameObjectsWithTag("WallRight")[0];
        GameObject topWall = GameObject.FindGameObjectsWithTag("WallTop")[0];
        GameObject bottomWall = GameObject.FindGameObjectsWithTag("WallBottom")[0];

        Transform transform = leftWall.GetComponent<Transform>();
        SpriteRenderer renderer = leftWall.GetComponent<SpriteRenderer>();

        transform.position = new Vector3(-camera.orthographicSize * camera.aspect - renderer.bounds.size.x / 2, transform.position.y, transform.position.z);

        transform = rightWall.GetComponent<Transform>();
        renderer = rightWall.GetComponent<SpriteRenderer>();

        transform.position = new Vector3(camera.orthographicSize * camera.aspect + renderer.bounds.size.x / 2, transform.position.y, transform.position.z);

        transform = topWall.GetComponent<Transform>();
        renderer = topWall.GetComponent<SpriteRenderer>();

        transform.position = new Vector3(camera.orthographicSize + renderer.bounds.size.y / 2, transform.position.y, transform.position.z);

        transform = bottomWall.GetComponent<Transform>();
        renderer = bottomWall.GetComponent<SpriteRenderer>();

        transform.position = new Vector3(-camera.orthographicSize - renderer.bounds.size.y / 2, transform.position.y, transform.position.z);
    }

    public static void Reset()
    {
        SoundManager.Play(SoundManager.Clip.RESET);

        BrickManager.Reset();
        DeprimusManager.Reset();

        ball.Reset();
        player.Reset();

        crizatusCanvas.SetActive(false);
        nervosusCanvas.SetActive(false);
    }

    public static void TogglePause()
    {
        SoundManager.Play(SoundManager.Clip.SELECTION);

        paused = !paused;
        pauseMenu.SetActive(paused);
        Time.timeScale = paused ? 0f : 1f;
    }

    public static void ToggleStarted()
    {
        SoundManager.Play(SoundManager.Clip.SELECTION);

        started = !started;
        startMenu.SetActive(!started);
        Time.timeScale = started ? 1f : 0f;
    }
}
