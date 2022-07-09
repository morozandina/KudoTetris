using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using Tetris;

public class GameManager : MonoBehaviour
{

    [Header("Score :")]
    [SerializeField] Text scoreText;
    public static Action ChangeScore;
    private int _currentScore;
    private float _currentMoney;
    [Space]
    [Header("Pause :")]
    [SerializeField] private GameObject pauseModal;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button restartPauseButton;
    [SerializeField] private Button exitPauseButton;
    [Space]
    [Header("Lose :")]
    [SerializeField] private GameObject loseModal;
    [SerializeField] private GameObject bestScoreModal;
    [SerializeField] private Button restartLoseButton;
    [SerializeField] private Button exitLoseButton;
    [SerializeField] private Text scoreLoseText;
    [SerializeField] private Text bestScoreLoseText;
    [SerializeField] private Text moneyText;
    public static Action Lose;
    [Space]
    [Header("Sound :")]
    [SerializeField] private Button musicButton;
    [SerializeField] private Sprite activedMusic;
    [SerializeField] private Sprite disabledMusic;
    [Space]
    [Header("Transitions :")]
    public Animator levelTransition;
    public GameObject Spawner;

    private bool isIncreaseSpeed;

    private void Awake()
    {
        ChangeScore += ChangeScoreText;
        Lose += ShowLoseScreen;
    }

    private void OnDestroy()
    {
        ChangeScore -= ChangeScoreText;
        Lose -= ShowLoseScreen;
    }

    private void Start()
    {
        TetrisBlock.fallTime = .3f;
        isIncreaseSpeed = true;

        _currentScore = 0;
        _currentMoney = 0;

        SetPauseButtonsAction();
        SetLoseButtonsAction();

        StartCoroutine(IncreaseSpeedOverLifeTime());
    }

    private void ChangeScoreText()
    {
        _currentScore += 100;
        _currentMoney += 10;

        scoreText.text = _currentScore.ToString();
    }

    // Pause Buttons
    private void SetPauseButtonsAction()
    {
        pauseButton.onClick.AddListener(() =>
        {
            pauseModal.SetActive(true);
            Time.timeScale = 0;
        });

        continueButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            pauseModal.SetActive(false);
        });

        restartPauseButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
        });

        exitPauseButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            StartCoroutine(LoadLevel(0));
        });
    }

    // Lose Buttons
    private void SetLoseButtonsAction()
    {
        restartLoseButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
        });

        exitLoseButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            StartCoroutine(LoadLevel(0));
        });
    }

    // Lose Modal
    private void ShowLoseScreen()
    {
        Destroy(Spawner);
        loseModal.SetActive(true);

        SetInformationLose();
        // Time.timeScale = 0;
    }

    private void SetInformationLose()
    {
        if (_currentScore > PlayerPrefs.GetInt(SavesData.Score))
        {
            bestScoreModal.SetActive(true);
            bestScoreLoseText.text = _currentScore.ToString();
        }

        PlayerPrefs.SetInt(SavesData.Money, PlayerPrefs.GetInt(SavesData.Money, 0) + (int) _currentMoney);

        scoreLoseText.text = _currentScore.ToString();
        moneyText.text = _currentMoney.ToString();
    }

    // Music
    private void MusicButton()
    {
        musicButton.onClick.AddListener(() =>
        {
            SetMusicStatus();
        });
    }

    private void SetMusicStatus()
    {

    }

    // Scene transition
    public IEnumerator LoadLevel(int levelIndex)
    {
        levelTransition.SetTrigger("Start");
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(levelIndex);
    }

    public IEnumerator IncreaseSpeedOverLifeTime()
    {
        while (isIncreaseSpeed)
        {
            yield return new WaitForSeconds(5);
            if (TetrisBlock.fallTime > .1f)
            {
                yield return new WaitForSeconds(120);
                TetrisBlock.fallTime -= 0.02f;
            }
            else
                isIncreaseSpeed = false;
        }
    }
}
