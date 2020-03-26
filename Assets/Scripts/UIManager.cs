using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _youWinText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _ammoCountText;
    [SerializeField]
    private Text _wave2, _wave3, _boss;
    [SerializeField]
    private bool _isGameOver, _youWin;
    
    [SerializeField]
    private float _flickerRate = 0.5f;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Slider _thrustersSlider;
    [SerializeField]
    private GameObject _thrusterSliderFill;
    private Image _thrustersSliderFillImage;
    [SerializeField]
    private Slider _ammoSlider;
    [SerializeField]
    private GameObject _ammoSliderFill;
    [SerializeField]
    private Image _ammoSliderFillImage;

    [SerializeField]
    private SpawnManager _spawnManager;

    private GameManager _gameManager;
    private AudioManager _audioManager;
    //handle to text
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _isGameOver = false;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        _thrustersSliderFillImage = _thrusterSliderFill.GetComponent<Image>();
        
        
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");

        }

        if (_audioManager == null)
        {
            Debug.LogError("AudioManager is NULL.");

        }

        if (_ammoSliderFillImage == null)
        {
            Debug.LogError("Ammo Slider Fill Image is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateScoreText(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateAmmoCountText(int ammoCount)
    {
        _ammoCountText.text = "Ammo: " + ammoCount;
        if (ammoCount == 0)
        {
            _ammoCountText.color = Color.red;
            _audioManager.PlayWarningSound();
        }
        else
        {
            _ammoCountText.color = Color.white;
        }
    }

    public void UpdateLives(int currentLives)
    {
        if (currentLives < 0)
        {
            currentLives = 0;
        }
        _livesImg.sprite = _livesSprites[currentLives];

    }

    public void GameOverSequence()
    {
        _isGameOver = true;
        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();

        StartCoroutine(FlashGameOverTextRoutine());
    }
    public void YouWinSequence()
    {
        _youWin = true;
        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();

        StartCoroutine(FlashYouWinTextRoutine());
    }
    IEnumerator FlashGameOverTextRoutine()
    {
        while(_isGameOver == true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(_flickerRate);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(_flickerRate);

        }
            

    }
    IEnumerator FlashYouWinTextRoutine()
    {
        _gameManager.GameOver();
        while (_youWin == true)
        {
            _youWinText.gameObject.SetActive(true);
            yield return new WaitForSeconds(_flickerRate);
            _youWinText.gameObject.SetActive(false);
            yield return new WaitForSeconds(_flickerRate);

        }


    }


    IEnumerator Wave2TextRoutine()
    {
        _wave2.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        _wave2.gameObject.SetActive(false);
        _spawnManager.StartSpawning(false);
    }

    IEnumerator Wave3TextRoutine()
    {
        _wave3.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        _wave3.gameObject.SetActive(false);
        _spawnManager.StartSpawning(false);
    }

    IEnumerator BossWaveTextRoutine()
    {
        _boss.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        _boss.gameObject.SetActive(false);
        _spawnManager.StartSpawning(true);
    }

    public void UpdateThrusterSlider(float value)
    {

        _thrustersSlider.value = value;
        
    }

    public void ThrusterSliderColor(bool canThrusterBeUsed)
    {
        if (canThrusterBeUsed == true)
        {
            _thrustersSliderFillImage.color = Color.blue;
        }
        else if (canThrusterBeUsed == false)
        {
            _thrustersSliderFillImage.color = Color.red;
        }
    }

    public void UpdateAmmoCountSlider(int value, int maxValue)
    {
        _ammoSlider.value = value;

        if (value == 0)
        {
            _ammoSliderFillImage.color = Color.red;
        }
        else if (value == maxValue)
        {
            _ammoSliderFillImage.color = Color.blue;
        }
        else if(value < 100 && value > 0)
        {
            _ammoSliderFillImage.color = Color.cyan;
        }

        _ammoSlider.maxValue = maxValue;
    }

    public void DisplayWaveText(int w)
    {
        switch (w)
        {
            case 2:
                StartCoroutine(Wave2TextRoutine());
                break;
            case 3:
                StartCoroutine(Wave3TextRoutine());
                break;
            case 4:
                StartCoroutine(BossWaveTextRoutine());
                break;
        }
    }
    

    
}
