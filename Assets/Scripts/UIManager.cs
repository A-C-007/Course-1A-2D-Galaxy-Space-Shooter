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
    private Text _restartText;
    [SerializeField]
    private Text _ammoCountText;
    [SerializeField]
    private bool _isGameOver;
    [SerializeField]
    private float _gameOverFlickerRate = 0.5f;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Slider _thrustersSlider;
    [SerializeField]
    private GameObject _thrusterSliderFill;
    private Image _sliderFillImage;

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
        _sliderFillImage = _thrusterSliderFill.GetComponent<Image>();

        
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");

        }

        if (_audioManager == null)
        {
            Debug.LogError("AudioManager is NULL.");

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
        _livesImg.sprite = _livesSprites[currentLives];

    }

    public void GameOverSequence()
    {
        _isGameOver = true;
        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();

        StartCoroutine(FlashGameOverTextRoutine());
    }

    IEnumerator FlashGameOverTextRoutine()
    {
        while(_isGameOver == true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(_gameOverFlickerRate);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(_gameOverFlickerRate);

        }
            

    }

    public void UpdateThrusterSlider(float value)
    {

        _thrustersSlider.value = value;
        
    }

    public void ThrusterSliderColor(bool canThrusterBeUsed)
    {
        if (canThrusterBeUsed == true)
        {
            _sliderFillImage.color = Color.blue;
        }
        else if (canThrusterBeUsed == false)
        {
            _sliderFillImage.color = Color.red;
        }
    }
}
