using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class CatController : MonoBehaviour
{

    private BoxCollider2D catCollider;
    private Animator catAnimator;
    public GameManager _gameManager;
    public AudioMixer MainMixer;
    private string catPurrVolume = "Purr25895649Volume";
    private string catGrowlVolume = "Growl25895649Volume";
    private string catAngryVolume = "Angry25895649Volume";
    public AudioSource[] catPurrSounds;
    public AudioSource[] catAngrySounds;
    public AudioSource[] catGrowlSounds;
    private AudioSource currentPurrSound;
    private AudioSource currentAngrySound;
    private AudioSource currentGrowlSound;
    private float petTime = 0.0f;
    private int clawCount = 0;
    private bool isPurring = false;
    private bool isClawing = false;
    private bool isGrowling = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        catCollider = GetComponent<BoxCollider2D>();
        catAnimator = GetComponent<Animator>();
        catAnimator.speed = 0;
        Debug.Log("Cat Controller Started");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameMaster.GetGameState() == Game.State.RUNNING)
        {
            catAnimator.speed = 1;
            //Debug.Log("CatCollider: OnTriggerEnter");
            if (Mathf.Abs(Input.GetAxis("Mouse X")) > _gameManager.speedLimit ||
                Mathf.Abs(Input.GetAxis("Mouse Y")) > _gameManager.speedLimit)
            {
                Debug.Log("TOO FAST!  Faster than limit of " + _gameManager.speedLimit);
                clawCount++;
                StartCoroutine(PlayGrowlSound());
                if (clawCount >= _gameManager.clawThreshold)
                {
                    _gameManager.ClawedSequence();
                    StartCoroutine(PlayAngrySound());
                }
            }

            if (!isGrowling && !isClawing)
            {
                StartCoroutine(PlayPurrSound());
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (GameMaster.GetGameState() == Game.State.RUNNING)
        {
            if (Mathf.Abs(Input.GetAxis("Mouse X")) > _gameManager.speedLimit ||
                Mathf.Abs(Input.GetAxis("Mouse Y")) > _gameManager.speedLimit)
            {
                Debug.Log("TOO FAST!  Faster than limit of " + _gameManager.speedLimit);
                clawCount++;
                StartCoroutine(PlayGrowlSound());
                if (clawCount >= _gameManager.clawThreshold)
                {
                    _gameManager.ClawedSequence();
                    StartCoroutine(PlayAngrySound());
                }
            }
            else
            {
                petTime += Time.deltaTime;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (GameMaster.GetGameState() == Game.State.RUNNING)
        {
            catAnimator.speed = 0;
            if (petTime >= _gameManager.requiredPetTime)
            {
                _gameManager.addToScore(1);
                petTime = 0.0f;
            }
        }
    }

    private IEnumerator PlayPurrSound()
    {
        isPurring = true;
        currentPurrSound = catPurrSounds[Random.Range(0, catPurrSounds.Length)];
        currentPurrSound.Play();
        yield return new WaitForSeconds(1.0f);
        isPurring = false;
    }

    private IEnumerator PlayGrowlSound()
    {
        isGrowling = true;
        currentGrowlSound = catGrowlSounds[Random.Range(0, catGrowlSounds.Length)];
        StartCoroutine(FadeMixerGroup.StartFade(MainMixer, catPurrVolume, 0.1f, 0));
        currentGrowlSound.Play();
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(FadeMixerGroup.StartFade(MainMixer, catPurrVolume, 0.1f, 1));
        isGrowling = false;
    }

    private IEnumerator PlayAngrySound()
    {
        isClawing = true;
        currentAngrySound = catAngrySounds[Random.Range(0, catAngrySounds.Length)];
        StartCoroutine(FadeMixerGroup.StartFade(MainMixer, catPurrVolume, 0.1f, 0));
        StartCoroutine(FadeMixerGroup.StartFade(MainMixer, catGrowlVolume, 0.1f, 0));
        currentAngrySound.Play();
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(FadeMixerGroup.StartFade(MainMixer, catPurrVolume, 0.1f, 1));
        StartCoroutine(FadeMixerGroup.StartFade(MainMixer, catGrowlVolume, 0.1f, 1));
        isClawing = false;
    }
    
}
