using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameManager : GAWGameManager
{
    public Transform handTransform;
    public Rigidbody2D handRigidbody;
    public TMP_Text scoreText;
    private Camera mainCamera;
    public AudioSource[] musicTracks;
    private AudioSource musicToPlay;
    public float speedLimit { get; private set; }
    public float requiredPetTime { get; private set; }
    private int score;
    private int scoreToWin;
    public int clawThreshold { get; private set; }
    private bool isBeingClawed = false;

    public override void OnGameLoad()
    {
        AssignMusicToPlay();
        mainCamera = Camera.main;
        switch (GameMaster.GetDifficulty())
        {
            case GameMaster.Difficulty.VERY_EASY:
                speedLimit = 2.7f; //0.9f;
                requiredPetTime = 0.1f;
                scoreToWin = 12;
                clawThreshold = 3;
                break;
            case GameMaster.Difficulty.EASY:
                speedLimit = 2.5f; //0.75f;
                requiredPetTime = 0.2f;
                scoreToWin = 11;
                clawThreshold = 3;
                break;
            case GameMaster.Difficulty.NORMAL:
                speedLimit = 2.3f; //0.6f;
                requiredPetTime = 0.3f;
                scoreToWin = 10;
                clawThreshold = 2;
                break;
            case GameMaster.Difficulty.HARD:
                speedLimit = 2.1f; //0.45f;
                requiredPetTime = 0.4f;
                scoreToWin = 9;
                clawThreshold = 2;
                break;
            case GameMaster.Difficulty.VERY_HARD:
                speedLimit = 1.9f; //0.3f;
                requiredPetTime = 0.5f;
                scoreToWin = 8;
                clawThreshold = 1;
                break;
            default:
                Debug.Log("GameMaster.Difficulty is not one of the five presets.");
                break;
        }
        score = 0;
        scoreText.text = "Pets Needed: " + scoreToWin.ToString();
    }

    public override void OnGameStart()
    {
        musicToPlay.Play();
    }

    public override void OnGameSucceeded()
    {
        
    }

    public override void OnGameFailed()
    {
        
    }

    private void Update()
    {
        //handTransform.position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        FollowMousePosition();
        //getAxisText.text = Mathf.Abs(Input.GetAxis("Mouse X")).ToString();
        //getAxisText.text = handRigidbody.linearVelocityX.ToString();
    }

    private void FollowMousePosition()
    {
        handTransform.position = GetWorldPositionFromMouse();
    }

    private Vector2 GetWorldPositionFromMouse()
    {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    public void addToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = "Pets Needed: " + (scoreToWin - score).ToString();
        if (score >= scoreToWin)
        {
            GameMaster.GameSucceeded();
        }
    }

    public IEnumerator GetClawed()
    {
        isBeingClawed = true;
        handTransform.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        //play angry cat sound
        //play angry cat animation
        yield return new WaitForSeconds(2);
        Cursor.lockState = CursorLockMode.None;
        handTransform.gameObject.SetActive(true);
        isBeingClawed = false;
        yield return null;
    }

    public void ClawedSequence()
    {
        if (!isBeingClawed)
        {
            StartCoroutine(GetClawed());
        }
    }

    private void AssignMusicToPlay()
    {
        switch (GameMaster.GetTotalTime())
        {
            case 20:
                musicToPlay = musicTracks[0];
                break;
            case 17.5f:
                musicToPlay = musicTracks[1];
                break;
            case 15:
                musicToPlay = musicTracks[2];
                break;
            case 12.5f:
                musicToPlay = musicTracks[3];
                break;
            case 10:
                musicToPlay = musicTracks[4];
                break;
            default:
                musicToPlay = musicTracks[0];
                Debug.Log("GameMaster.GetTotalTime() returned " + GameMaster.GetTotalTime() +
                          " which was not an expected value.");
                break;
        }
    }

}
