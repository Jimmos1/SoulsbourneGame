using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //This player input manager tries to add an extra layer between character movement and player input.
    public bool IsRunning { get; private set; }
    public bool Jump { get; private set; }
    public bool StimpackUse { get; private set; }
    public bool Interaction { get; private set; }
    public bool Reload { get; private set; }

    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }

    public float MouseX { get; private set; }
    public float MouseY { get; private set; }
    public bool MouseFireDown { get; private set; }
    public bool MouseFireHold { get; private set; }
    public bool MouseAimDown { get; private set; }
    public bool MouseAimHold { get; private set; }
    public Vector2 MouseScrollDelta { get; private set; }
    public bool KeyOne { get; private set; }
    public bool KeyTwo { get; private set; }
    public bool KeyThree { get; private set; }


    //public Canvas pausemenuUI;
    //public Canvas statsUI;
    public static bool GameIsPaused = false;
    //public AudioSource SoundManager;
    //This guy controls all, also should probably make these an enumerator and use switch/case

    private void Awake()
    {
        //SoundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();

        //Resume();
    }

    private void Update()
    {
        if (!GameIsPaused)
        {
            Horizontal = Input.GetAxis("Horizontal");
            Vertical = Input.GetAxis("Vertical");

            Jump = Input.GetKey(KeyCode.Space);
            IsRunning = Input.GetKey(KeyCode.LeftShift);
            StimpackUse = Input.GetKey(KeyCode.X);
            Interaction = Input.GetKey(KeyCode.E);
            Reload = Input.GetKeyDown(KeyCode.R);

            KeyOne = Input.GetKeyDown(KeyCode.Alpha1);
            KeyTwo = Input.GetKeyDown(KeyCode.Alpha2);
            KeyThree = Input.GetKeyDown(KeyCode.Alpha3);

            MouseScrollDelta = Input.mouseScrollDelta;
            MouseX = Input.GetAxis("Mouse X");
            MouseY = Input.GetAxis("Mouse Y");
            MouseFireDown = Input.GetMouseButtonDown(0);
            MouseFireHold = Input.GetMouseButton(0);
            MouseAimDown = Input.GetMouseButtonDown(1);
            MouseAimHold = Input.GetMouseButton(1);
        }
        else if (GameIsPaused)
        {
            //Debug.Log("Am now set at Pause Menu");
        }
    }

    

    //private void LateUpdate()
    //{
    //    DisplayUsefulStats();
    //    Options();
    //}

    
    //public void Options()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        if (GameIsPaused)
    //        {
    //            Resume();
    //        }
    //        else
    //        {
    //            Pause();
    //        }
    //    }
    //}

    //public void Resume()
    //{
    //    SoundManager.UnPause();
    //    Time.timeScale = 1;
    //    pausemenuUI.enabled = false;
    //    GameIsPaused = false;
    //    Cursor.lockState = CursorLockMode.Locked; //COMMENT FOR FELLOW COWORKERS : ASSUMING ALL CODE RUNS ON FINAL BUILD IN AN EXECUTABLE , REATTAINING FOCUS IN THE GAME WINDOW IS MOST LIKELY SUPERFLUOUS
    //    Cursor.visible = false; // Jimmos comment - this doesn't work when pressing escape again when pause menu is up because escape is the key the unlocks the cursor in the first place
    //}

    //void Pause() //Might need tweaking , feed not cutting off instantly (hopefully they won't notice). Most noticable when pausing during mouse swipes. <Might not happen in Build version>
    //{
    //    pausemenuUI.enabled = true;
    //    Time.timeScale = 0;
    //    GameIsPaused = true;
    //    Cursor.lockState = CursorLockMode.None;
    //    Cursor.visible = true;
    //    SoundManager.Pause();
    //}

    //void DisplayUsefulStats()
    //{
    //    if (!GameIsPaused)
    //    {
    //        if (Input.GetKey(KeyCode.Tab))
    //        {
    //            statsUI.enabled = true;
    //        }
    //        else statsUI.enabled = false;
    //    }
    //}

    
}
