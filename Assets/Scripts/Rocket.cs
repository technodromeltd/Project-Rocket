using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Rocket : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody rigidbody;
    AudioSource audioSource;
    RightJoystick rightJoystick;
    [SerializeField] float thrustSpeed = 100; 
    [SerializeField] float rotationSpeed = 100;
    [SerializeField] float rotationSpeedMobile = 3;
    [SerializeField] float joystickDeadZone = 0.2f;
    [SerializeField] Canvas gameOverMenu; 
    [SerializeField] Canvas pauseMenu;
   
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip dead;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deadParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] AudioSource fxAudioSource;
    [SerializeField] float startingFuel;
    [SerializeField] float burnSpeed;
    [SerializeField] Text fuelText;
    [SerializeField] int fuelCell;
    enum State {Alive, Dead,Transcending, Pause};
    State state = State.Alive;

    bool collisionsEnabled = true;
    bool isPaused;
    float currentFuel;

    public SceneLoader sceneLoader;
    bool MOBILE_CONTROLS = false;
    Text levelTitleText;



    void Start()
    {
        levelTitleText = GameObject.FindGameObjectWithTag("LevelTitle").GetComponent<Text>();
        levelTitleText.text = "LEVEL " + sceneLoader.getLevelIndex();
        //rightJoystick = GameObject.FindGameObjectWithTag("Mobile").GetComponent<RightJoystick>();
        gameOverMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
       
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.02f;
        audioSource.Play();
        isPaused = false;
        currentFuel = startingFuel;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        MOBILE_CONTROLS = false;
#else
           INSIDE_UNITY = true;
#endif
       

    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            if (!MOBILE_CONTROLS) { 
            ThrustKeyboard();
            RotateKeyboard();
        } else { 
            ThrustMobile();
            RotateMobile();
}

    }
        else if (state == State.Dead)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                sceneLoader.RestartLevel();
            }
        }
        else if (state == State.Pause) { 
            if (Input.GetKeyDown(KeyCode.Return))
            {
                sceneLoader.RestartLevel();
            }
        }
        RespondToPause();
        UpdateFuelText();
       
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }
    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            sceneLoader.LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsEnabled = !collisionsEnabled;
        }
    }
    private void RespondToPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused == false)
            {

               
                Time.timeScale = 0; 
                audioSource.Stop();
               
                isPaused = true;
                TogglePauseMenu(isPaused);
                state = State.Pause;
            }
            else
            {
      
                Time.timeScale = 1;
                isPaused = false;
                TogglePauseMenu(isPaused);
                state = State.Alive;

            }
             
        }
    }

    private void TogglePauseMenu(bool b)
    {
        pauseMenu.gameObject.SetActive(b);
    }

    private void ThrustMobile()
    {
       
        if (CrossPlatformInputManager.GetButton("Thrust") && (currentFuel >= 0))
        {
            ApplyThrust();            
        }
        else
        {
            StopApplyingThrust();
        }
    }
     private void ThrustKeyboard()
    {
             
         if (Input.GetKey(KeyCode.Space) && (currentFuel >= 0))
        {
            ApplyThrust();            
        }
        else
        {
            StopApplyingThrust();
        }
    }

    private void StopApplyingThrust()
    {
        if (audioSource.volume == 1)
        {
            StartCoroutine(VolumeFade(audioSource, 0.02f, 0.1f));
        }

        mainEngineParticles.Stop();
    }

    private void ApplyThrust()
    {
        float thrustThisFrame = thrustSpeed * Time.deltaTime * 10;
        rigidbody.AddRelativeForce(Vector3.up * thrustThisFrame);

        if (!audioSource.isPlaying)
            audioSource.Play();
        audioSource.volume = 1;

        mainEngineParticles.Play();

        BurnFuel();
    }

    private void BurnFuel()
    {
        float burnRate = burnSpeed * Time.deltaTime;
        currentFuel -= burnRate;
       
    }

    private void UpdateFuelText()
    {
        string currentFuelString = ((int)currentFuel).ToString();
        fuelText.text = currentFuelString;
    }

    private void RotateKeyboard()
    {

        float rotationThisFrame = rotationSpeed * Time.deltaTime ;
     
       
       if (Input.GetKey(KeyCode.LeftArrow))
        { 
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
       else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
      
 
    }
    void RotateMobile()
    {
        float x = -CrossPlatformInputManager.GetAxis("Horizontal");
        //float x = -rightJoystick.GetInputDirection().x;
        //print(x);
        if (Mathf.Abs(x) < joystickDeadZone)
            return;
        x = x * rotationSpeedMobile;
        
            transform.Rotate(Vector3.forward * x);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Fuel":
                CollectFuelCell();
                Destroy(other.gameObject);
               
                break;

            case "PortalIn":
                PortalIn();
                break;

            default:
                break;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        print(collision);
        if (state != State.Alive || !collisionsEnabled) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":

                break;
            
            case "Finish":
                StartSuccessSequnce();
                break;

            default:
                StartDeadSequence();

                break;

        }

    }
    private void PortalIn()
    {
       var portalA = GameObject.FindGameObjectWithTag("PortalIn");
        var portalB = GameObject.FindGameObjectWithTag("PortalOut");
        Vector3 newVelocityDirection = rigidbody.velocity;
        var euler = portalB.transform.rotation.eulerAngles;
        var rot = Quaternion.Euler(0, 0, euler.z);
        float teleportTurnAngle = portalA.transform.parent.eulerAngles.x + portalB.transform.parent.eulerAngles.x;
       

        rigidbody.velocity = Quaternion.AngleAxis(teleportTurnAngle, Vector3.forward) * newVelocityDirection;

       

        transform.rotation = Quaternion.AngleAxis(teleportTurnAngle, Vector3.forward) * transform.rotation; ;
        transform.position = portalB.transform.position;
       
        
    }
    private  void CollectFuelCell( )
    {
        // Increase currentFuel amount, remove object from scene
        currentFuel += fuelCell;
        
    }

    private void StartSuccessSequnce()
    {
        audioSource.Stop();
        state = State.Transcending;
        fxAudioSource.clip = success;
        fxAudioSource.Play();
        successParticles.Play();
        sceneLoader.Invoke("LoadNextLevel", 1f);
    }
  

    private void StartDeadSequence()
    {
        audioSource.Stop();
        fxAudioSource.clip = dead;
        fxAudioSource.Play();
        deadParticles.Play();
        state = State.Dead;

        ShowGameOverMenu();
    }
    void SetLowVolume()
    {
        //audioSource.volume = 0.05f;
        StartCoroutine(VolumeFade(audioSource, 0.05f, 0.2f));
        // audioSource.Stop();

    }

    IEnumerator VolumeFade(AudioSource _AudioSource, float _EndVolume, float _FadeLength)
    {
       
        float _StartVolume = _AudioSource.volume;
        float _StartTime = Time.time;

        while (Time.time < _StartTime + _FadeLength)
        {
            _AudioSource.volume = _StartVolume + ((_EndVolume - _StartVolume) * ((Time.time - _StartTime) / _FadeLength));
           
            yield return null;
        }

        if (_EndVolume == 0) { _AudioSource.Stop(); }

    }

    

    void ShowGameOverMenu()
    {
        gameOverMenu.gameObject.SetActive(true);
    }

}
