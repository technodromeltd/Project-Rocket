using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    Camera mainCamera;
     Canvas winScreen;
    ParticleSystem winParticles;
    AudioSource audioSource;
    AudioSource musicAudioSource;
    Animator anim;
    // Start is called before the first frame update
     void Awake()
    {
        gameObject.SetActive(false);
        musicAudioSource = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();



    }
    public void ShowWinScreen()
    {
        gameObject.SetActive(true);
        StartCoroutine(ZoomAtTarget(70));
        musicAudioSource.Stop();
        audioSource = GetComponent<AudioSource>();
        winParticles = GetComponentInChildren<ParticleSystem>();
       
        
        anim = GetComponentInChildren<Animator>();
       
       
        anim.Play("EnlargeAnimation");
        winParticles.Play();
        audioSource.Play();
        StartCoroutine(FadeToBlack());
    }

    private IEnumerator FadeToBlack()
    {

        Image panel = GetComponentInChildren<Image>();
        var startColor = panel.color;
        print("coroutine started waiting");
        yield return new WaitForSeconds(10);
        for (int i = 1; i < 101;i++)
        {

            startColor.a = (i/100f);
           
            panel.color = startColor;
            audioSource.volume = (100f -i)/100;
            yield return new WaitForSeconds(0.03f);

        }

       
    }

    IEnumerator ZoomAtTarget(int steps)
    {
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        print(playerObject.transform.position);
        var playerPos = playerObject.transform;
        print(playerPos);
        float distance = Vector3.Distance(playerPos.position, mainCamera.transform.position);
        for   (int i = 1; i < steps; i++)
        {
            Vector3 direction = playerPos.position - mainCamera.transform.position;
            Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
            mainCamera.transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 1 * Time.deltaTime);
           float fracLeft = Vector3.Distance(playerPos.position, mainCamera.transform.position);

            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, playerPos.position, 0.2f);
            yield return new WaitForSeconds(0.03f);

        }

    }
}
