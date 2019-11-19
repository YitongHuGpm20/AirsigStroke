using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using AirSig;
using HTC.UnityPlugin.Vive;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeveloperDefined : BasedGestureHandle {

    public GameObject duck;
    public Transform handTrans;
    private bool hasAmmo;
    public GameObject blackHole;
    private float accuracy;
    private Vector3 spawnPos;
    private string exactGesture;
    public Text playerHealth;
    public int pHealth = 100;
    public AudioClip spawnDuck;
    private AudioSource sourceDuck;
    public AudioSource SFXPlayer;
    public GameObject particleEffect;
    public bool playingSFX = false;
    private bool playHeartSFX = false;
    private bool playCSFX = false;
    private bool playTriangleSFX = false;
    private bool playErrorSFX = false;

    public List<AudioClip> SFX = new List<AudioClip>();

    // Callback for receiving signature/gesture progression or identification results
    AirSigManager.OnDeveloperDefinedMatch developerDefined;

    // Handling developer defined gesture match callback - This is invoked when the Mode is set to Mode.DeveloperDefined and a gesture is recorded.
    // gestureId - a serial number
    // gesture - gesture matched or null if no match. Only guesture in SetDeveloperDefinedTarget range will be verified against
    // score - the confidence level of this identification. Above 1 is generally considered a match
    void HandleOnDeveloperDefinedMatch(long gestureId, string gesture, float score) {
        textToUpdate = string.Format("<color=cyan>Gesture Match: {0} Score: {1}</color>", gesture.Trim(), score);
        //print("MY SCORE IS AWESOME" + score);
        accuracy = score;
        exactGesture = gesture.Trim();

        
    }

    // Use this for initialization
    void Awake() {
        Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
        sourceDuck = GetComponent<AudioSource>();
        // sourceDuck.enabled = false;
        // Update the display text
        AirSigManager.Mode Mode = AirSigManager.Mode.DeveloperDefined;
        textMode.text = string.Format("Mode: {0}", Mode.ToString());
        textResult.text = defaultResultText = "Pressing trigger on the right hand and write\ntriangle symbol in the air\nReleasing trigger when finish\nPress left hand trigger to shoot\nwrite heart symbol to get a shield";
        textResult.alignment = TextAnchor.UpperCenter;
        instruction.SetActive(false);
        ToggleGestureImage("All");

        // Configure AirSig by specifying target 
        developerDefined = new AirSigManager.OnDeveloperDefinedMatch(HandleOnDeveloperDefinedMatch);
        airsigManager.onDeveloperDefinedMatch += developerDefined;
        airsigManager.SetMode(Mode);
        airsigManager.SetDeveloperDefinedTarget(new List<string> { "HEART", "C", "DOWN", "Triangle"});
        airsigManager.SetClassifier("Spellcasters Mini Test", "");
        checkDbExist();

        airsigManager.SetTriggerStartKeys(
            AirSigManager.Controller.RIGHT_HAND,
            SteamVR_Controller.ButtonMask.Trigger,
            AirSigManager.PressOrTouch.PRESS);


        airsigManager.SetTriggerStartKeys(
            AirSigManager.Controller.LEFT_HAND,
            SteamVR_Controller.ButtonMask.Touchpad,
            AirSigManager.PressOrTouch.PRESS);

        hasAmmo = false;
        blackHole.SetActive(false);
        playerHealth.text = "Player Health:" + pHealth.ToString();


    }


    void OnDestroy() {
        // Unregistering callback
        airsigManager.onDeveloperDefinedMatch -= developerDefined;
    }

    void Update() {
        UpdateUIandHandleControl();
        playerHealth.text = "Player Health:" + pHealth.ToString();
        
        if (playHeartSFX)
        {
            blackHole.SetActive(true);
            SFXPlayer.clip = SFX[1];
            SFXPlayer.Play();
            playHeartSFX = false;
        }
        else if (playTriangleSFX)
        {
            particleEffect.SetActive(true);
            SFXPlayer.clip = SFX[0];
            SFXPlayer.Play();
            hasAmmo = true;
            blackHole.SetActive(false);
            playTriangleSFX = false;
            
        }
        else if (playErrorSFX)
        {
            SFXPlayer.clip = SFX[2];
            SFXPlayer.Play();
            // Reset the variable so that it doesn't play again
            playErrorSFX = false;
        }
       
        if (accuracy >= 1.1 && !hasAmmo && exactGesture == "Triangle")
        {
            particleEffect.SetActive(true);
            hasAmmo = true;
            blackHole.SetActive(false);
            sourceDuck.PlayOneShot(spawnDuck);
            
        }
       
        /*
        if (accuracy >= 1.1 && exactGesture == "HEART")
            blackHole.SetActive(true);
        */
       
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Pad) && hasAmmo)
        {
            SpawnDuck();
            hasAmmo = false;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            SpawnDuck();
            print("SPACE IS PRESSED");
        }

        if (pHealth == 0)
            SceneManager.LoadScene(1);
       
        
    }

    void SpawnDuck()
    {
        
        var tempDuck = Instantiate(duck, handTrans.position, handTrans.rotation);
        tempDuck.transform.Translate(0f, 0f, 0.02f);
        

       // Debug.Log("Duckkkkkkkkkkk");
    }
    /// <summary>
    /// Sets the variables to let Update() know to play sound effects
    /// Gets called by onDeveloperDefinedMatch
    /// </summary>
    /// <param name="gestureId"></param>
    /// <param name="exactGesture">closest matching gesture</param>
    /// <param name="accuracy">how well it matches</param>
    void PlaySFX(long gestureId, string exactGesture, float accuracy)
    {
        if(exactGesture == "HEART" && accuracy >= 1.1)
        {
            playHeartSFX = true;
           
        }
        else if(exactGesture == "Triangle" && accuracy >= 1.1 && !hasAmmo)
        {
            playTriangleSFX = true;
        }
        else
        {
            playErrorSFX = true;
        }
   
    }
    private void OnEnable()
    {
        airsigManager.onDeveloperDefinedMatch += PlaySFX;
    }
    private void OnDisable()
    {
        airsigManager.onDeveloperDefinedMatch -= PlaySFX;
    }
}