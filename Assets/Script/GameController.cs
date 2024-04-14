using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int time = 5;
    public AudioClip bellCount, bellStart;
    public AudioSource audioSource;
    public int maxLap;
    public Text lapText, timeResultText, highScoreText, timeRunText;
    public Image carHealthBar;
    public GameObject finishTitle, wreckedTitle;
    public ColliderController carColliderController;
    public GameData gameData;
    public bool isWrecked;

    bool isSoundPlayed, isFinished, isStarted = false;
    float timeRun;
    int checkPointCounter;
    public List<Checkpoint> checkPointPin;
    int currentLap;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
    {
        if(time <= 0 && !isFinished)
        {
            timeRun += Time.deltaTime;
        }

        if (carColliderController.carHealth <= 0)
            isWrecked = true;

        wreckedTitle.SetActive(isWrecked);

        lapText.text = currentLap + " / " + maxLap;
        timeRunText.text = timeRun.ToString("F2");
        carHealthBar.fillAmount = (float)carColliderController.carHealth / (float)carColliderController.maxCarHealth;
    }

    IEnumerator Timer()
    {
        if(time > 0 && isStarted)
        time -= 1;

        yield return new WaitForSeconds(1f);

        if (time <= 0)
        {
            yield break;
        }
            

        if (time == 4 && !isSoundPlayed)
        {
            audioSource.clip = bellCount;
            audioSource.Play();
            isSoundPlayed = true;
        }

        else if (time == 3 && !isSoundPlayed)
        {
            audioSource.clip = bellCount;
            audioSource.Play();
            isSoundPlayed = true;
        }

        else if (time == 2 && !isSoundPlayed)
        {
            audioSource.clip = bellCount;
            audioSource.Play();
            isSoundPlayed = true;
        }

        else if (time == 1 && !isSoundPlayed)
        {
            audioSource.clip = bellStart;
            audioSource.Play();
            isSoundPlayed = true;
        }

        isSoundPlayed = false;

        StartCoroutine(Timer());
    }

    public void CheckPointCount(Checkpoint checkPoint)
    {
        if (!isFinished)
        {
            checkPointPin.Add(checkPoint);
            checkPoint.GetComponent<BoxCollider>().enabled = false;
            checkPointCounter++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "RMCarDemo_Body_Col" && checkPointCounter == 5 && !isFinished)
        {
            foreach(Checkpoint i in checkPointPin)
            {
                i.GetComponent<BoxCollider>().enabled = true;
            }

            checkPointPin.Clear();
            checkPointCounter = 0;
            currentLap++;
        }

        if(currentLap >= maxLap)
        {
            print("end");
            isFinished = true;
            finishTitle.SetActive(true);
            timeResultText.text = "Your time is " + timeRun.ToString("F2");
            gameData.CheckHighScore(timeRun);
            highScoreText.text = gameData.GetHighScore().ToString("F2");
        }
    }

    public void StartBtn(GameObject button)
    {
        isStarted = true;
        button.SetActive(false);
    }

    public void RestartBtn()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
