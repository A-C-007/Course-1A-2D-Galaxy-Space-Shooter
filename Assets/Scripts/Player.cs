using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private Transform _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive;
    [SerializeField]
    private bool _isSpeedBoostActive;
    [SerializeField]
    private bool _isShieldActive;
    [SerializeField]
    private float _speedBoostMultiplier = 2.0f;
    [SerializeField]
    private float _boostedSpeed;
    private Transform _shield;

    [SerializeField]
    private int _score;

    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private GameObject _rightEngine;

    private UIManager _uIManager;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _laserSoundClip;

    private AudioManager _audioManager;

    void Start()
    {
        
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _shield = gameObject.transform.Find("Shield");
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is NULL!");
        } 

        if (_uIManager == null)
        {
            Debug.LogError("The UIManager is NULL!");
        }
       
        if(_audioSource == null)
        {
            Debug.LogError("The AudioSource on the player is NULL!");

        }

        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("The AudioManager is Null!");
        }
    }

    
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
        

        
        
    }

    void CalculateMovement()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (_isSpeedBoostActive == true)
        {
            _boostedSpeed = _speed * _speedBoostMultiplier;
            transform.Translate(Vector3.right * horizontalInput * _boostedSpeed * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * _boostedSpeed * Time.deltaTime);
            
        }
        else
        {
            transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        }
        


        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0));


        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {

            transform.position = new Vector3(11.3f, transform.position.y, 0);

        }
    }

    void FireLaser()
    {
        
            _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {

            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);

        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0.002f, 1.019f, 0), Quaternion.identity);
        }


        _audioSource.PlayOneShot(_laserSoundClip);


        
    }

    public void Damage()
    {
        if(_isShieldActive == true)
        {
            _isShieldActive = false;
            if (_shield != null)
            {
                _shield.transform.gameObject.SetActive(false);
            }
            return;
        }
        else
        {
            _lives--;
            switch (_lives)
            {
                case 2:
                    _leftEngine.SetActive(true);
                    break;
                case 1:
                    _rightEngine.SetActive(true);
                    break;
            }
            _uIManager.UpdateLives(_lives);

        }
        
        if (_lives < 1)
        {
            if (_spawnManager != null)
            {
                _spawnManager.StopEnemySpawnRoutine();

            }
            if (_audioManager != null)
            {
                _audioManager.PlayExplosionSound();
            }
            
            _uIManager.GameOverSequence();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {

        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());

        
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {

        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());

    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {

        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        if (_shield != null)
        {
            _shield.transform.gameObject.SetActive(true);
        }
        

    }

    public void UpdateScore(int scoreAmountToAdd)
    {
        _score += scoreAmountToAdd;
        if (_uIManager != null)
        {

            _uIManager.UpdateScoreText(_score);
        }
        
    }
    //method to add 10 to score
    //communicate to uimanager to add to score text
}
