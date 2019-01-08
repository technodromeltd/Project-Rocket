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
    float rotationSpeedMobile = 200;
    [SerializeField] float joystickDeadZone = 0.2f;
    [SerializeField] Canvas gameOverMenu;
    [SerializeField] Canvas pauseMenu;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip dead;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip teleport;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deadParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] AudioSource fxAudioSource;




    FuelController fuelController;
    float currentFuel;

    enum State { Alive, Dead, Transcending, Pause, Teleporting };
    State state = State.Alive;

    bool collisionsEnabled = true;
    bool isPaused;


    public SceneLoader sceneLoader;
    bool MOBILE_CONTROLS = true;
    Text levelTitleText;


    private float timePressStarted;
    private bool longPressTriggered = false;
    public float durationThreshold = 0.1f;


    void Start()
    {
        fuelController = FindObjectOfType<FuelController>();
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


#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        MOBILE_CONTROLS = false;
#else
          MOBILE_CONTROLS = true;
#endif


    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            if (!MOBILE_CONTROLS)
            {
                ThrustKeyboard();
                RotateKeyboard();
            }
            else
            {
                TwoTapControls();
                // RotateMobile();
            }

        }
        else if (state == State.Dead)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                sceneLoader.RestartLevel();
            }
        }
        else if (state == State.Pause)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                sceneLoader.RestartLevel();
            }
        }
        RespondToPause();


        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
        currentFuel = fuelController.getCurrentFuel();
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

    private void TwoTapControls()
    {
        float rotationThisFrame = rotationSpeedMobile * Time.deltaTime;
        if (CrossPlatformInputManager.GetButton("RightThrust") && CrossPlatformInputManager.GetButton("LeftThrust") && currentFuel > 0)
        {
            ApplyThrust();
        }
        else
        {
            StopApplyingThrust();
            if (CrossPlatformInputManager.GetButton("RightThrust"))
            {
                if (!longPressTriggered)
                {
                    timePressStarted = Time.time;
                    longPressTriggered = true;
                }

                if (Time.time - timePressStarted > durationThreshold)
                {
                    transform.Rotate(-Vector3.forward * rotationThisFrame);
                }
            }
            else if (CrossPlatformInputManager.GetButton("LeftThrust"))
            {
                if (!longPressTriggered)
                    timePressStarted = Time.time;
                longPressTriggered = true;
                if (Time.time - timePressStarted > durationThreshold)
                {
                    transform.Rotate(Vector3.forward * rotationThisFrame);
                }
            }
            else
            {
                longPressTriggered = false;
            }
        }

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

        fuelController.BurnFuel();
    }




    private void RotateKeyboard()
    {

        float rotationThisFrame = rotationSpeed * Time.deltaTime;
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
        if (state != State.Alive) { return; }
        switch (other.gameObject.tag)
        {
            case "Fuel":
                fuelController.CollectFuelCell();
                Destroy(other.gameObject);

                break;

            case "Portal":
               
                TeleportRocket(other);
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
    private void TeleportRocket(Collider senderPortal)
    {
        fxAudioSource.clip = teleport;
        fxAudioSource.Play();
        StartCoroutine(DisableCollisionsWhileTeleporting());
        GameObject targetPortal = senderPortal.gameObject.GetComponent<PortalController>().getTargetPortal();
        var portalA = senderPortal.gameObject;
        var portalB = targetPortal.gameObject;

        Vector3 newVelocityDirection = new Vector3(rigidbody.velocity.y, rigidbody.velocity.x, rigidbody.velocity.z);
        float xAngle = transform.rotation.eulerAngles.x;
        print(rigidbody.velocity);
        var euler = portalB.transform.rotation.eulerAngles;
        var rot = Quaternion.Euler(0, 0, euler.z);
        float teleportTurnAngle = portalA.transform.eulerAngles.x + portalB.transform.eulerAngles.x;

        rigidbody.velocity = Quaternion.AngleAxis(teleportTurnAngle, Vector3.forward) * newVelocityDirection;
        
        transform.rotation = Quaternion.AngleAxis(teleportTurnAngle, Vector3.forward) * transform.rotation; ;
        transform.position = portalB.transform.position;
        print("Teleportin with teleportTurnAngle:" + teleportTurnAngle);


    }
    IEnumerator DisableCollisionsWhileTeleporting()
    {
        state = State.Teleporting;
        print("Teleporting, untouchable");
        yield return new WaitForSeconds(0.2f);
        print("Teleportin DONE");
        state = State.Alive;
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
