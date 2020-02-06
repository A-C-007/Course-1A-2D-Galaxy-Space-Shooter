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
    private GameObject _greenOrbPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire;
    [SerializeField]
    private int _ammoCount = 15;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive;
    [SerializeField]
    private bool _isGreenOrbHomingMissileActive;
    [SerializeField]
    private bool _isPowerupSpeedBoostActive;
    [SerializeField]
    private bool _isShieldActive;
    [SerializeField]
    private float _powerupSpeedBoostMultiplier = 2.0f;
    [SerializeField]
    private float _lShiftThrustersSpeedMultiplier = 1.5f;
    [SerializeField]
    private float _boostedSpeed;
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private GameObject _orangeShield;
    [SerializeField]
    private GameObject _redShield;
    [SerializeField]
    private int _shieldStrength = 3;

    [SerializeField]
    private GameObject _thrusterImg;
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
    [SerializeField]
    private CameraShake _cameraShake;
    [SerializeField]
    private float _thrusterPowerLevelValue = 10f;
    [SerializeField]
    private bool _canThrustersBeUsed = true;
    
    void Start()
    {
        _canThrustersBeUsed = true;
        
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        
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

        _ammoCount = 15;
        _uIManager.UpdateAmmoCountText(_ammoCount);

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

        if (_isPowerupSpeedBoostActive == true)
        {
            _boostedSpeed = _speed * _powerupSpeedBoostMultiplier;
            transform.Translate(Vector3.right * horizontalInput * _boostedSpeed * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * _boostedSpeed * Time.deltaTime);
           
        }
        if (Input.GetKey(KeyCode.LeftShift) && _isPowerupSpeedBoostActive == false && _canThrustersBeUsed == true)
        {
            
            _boostedSpeed = _speed * _lShiftThrustersSpeedMultiplier;
            transform.Translate(Vector3.right * horizontalInput * _boostedSpeed * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * _boostedSpeed * Time.deltaTime);

            _thrusterPowerLevelValue -= Time.deltaTime * 3f;
            _uIManager.UpdateThrusterSlider(_thrusterPowerLevelValue);
            if (_thrusterPowerLevelValue <= 0)
            {
                _uIManager.ThrusterSliderColor(false);
                _canThrustersBeUsed = false;
            }
            

        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && _isPowerupSpeedBoostActive == false)
        {
            
            transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
            
        }
        else
        {
            transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
            
        }
        
        if (_canThrustersBeUsed == false)
        {
            StartCoroutine(ThrustersCoolDownRoutine());

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
        
        if (_ammoCount > 0)
        {
            _canFire = Time.time + _fireRate;

            if (_isTripleShotActive == true)
            {

                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);

            }
            else if(_isGreenOrbHomingMissileActive == true)
            {
                Instantiate(_greenOrbPrefab, transform.position + new Vector3(-0.027f, 1.314f, 0), Quaternion.identity);

            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0.002f, 1.019f, 0), Quaternion.identity);

            }

            _ammoCount--;
            _uIManager.UpdateAmmoCountText(_ammoCount);

            _audioSource.PlayOneShot(_laserSoundClip);
        }

        
    }

    public void Damage()
    {

       StartCoroutine(_cameraShake.Shake(0.2f, 0.08f));
        
        if(_isShieldActive == true)
        {
            switch (_shieldStrength)
            {
                case 3:
                    _shieldStrength--;
                    _shield.SetActive(false);
                    _orangeShield.SetActive(true);
                    _audioManager.PlayShieldHitSound();
                    break;
                case 2:
                    _shieldStrength--;
                    _orangeShield.SetActive(false);
                    _redShield.SetActive(true);
                    _audioManager.PlayShieldHitSound();
                    

                    break;
                case 1:
                    _shieldStrength--;
                    _audioManager.PlayShieldHitSound();
                    if (_shield != null)
                    {
                        _isShieldActive = false;
                        _redShield.SetActive(false);
                    }
                    break;

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
                    _audioManager.PlayEngineExplosionSound();
                    break;
                case 1:
                    _rightEngine.SetActive(true);
                    _audioManager.PlayEngineExplosionSound();

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
        _isGreenOrbHomingMissileActive = false;
        _isTripleShotActive = true;
        
            _ammoCount += 10;
        
           
        _uIManager.UpdateAmmoCountText(_ammoCount);
        StartCoroutine(TripleShotPowerDownRoutine());

        
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {

        _isPowerupSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());

    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {

        yield return new WaitForSeconds(5.0f);
        _isPowerupSpeedBoostActive = false;
    }

    public void ShieldActive()
    {
        _shieldStrength = 3;
        _isShieldActive = true;

        _orangeShield.SetActive(false);
        _redShield.SetActive(false);
        _shield.SetActive(true);
        
        

    }
    public void AmmoPowerup()
    {
        _ammoCount += 15;
        _uIManager.UpdateAmmoCountText(_ammoCount);

    }

    public void HealthPowerup()
    {
        if (_lives < 3)
        {
            _lives++;
            switch (_lives)
            {
                case 3:
                    _leftEngine.SetActive(false);
                    break;
                case 2:
                    _rightEngine.SetActive(false);
                    break;
                
            }
            _uIManager.UpdateLives(_lives);
        }
        return;
    }

    public void HomingGreenOrbPowerup()
    {
        if(_ammoCount == 0)
        {
            _ammoCount = 5;
        }
        _isTripleShotActive = false;
        _isGreenOrbHomingMissileActive = true;
        StartCoroutine(HomingGreenOrbPowerDownRoutine());
    }

    IEnumerator HomingGreenOrbPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isGreenOrbHomingMissileActive = false;
    }

    public void UpdateScore(int scoreAmountToAdd)
    {
        _score += scoreAmountToAdd;
        if (_uIManager != null)
        {

            _uIManager.UpdateScoreText(_score);
        }
        
    }

    IEnumerator ThrustersCoolDownRoutine()
    {
        yield return new WaitForSeconds(1f);
        if (_canThrustersBeUsed == false)
        {
            
            _thrusterPowerLevelValue += Time.deltaTime;
            
            _uIManager.UpdateThrusterSlider(_thrusterPowerLevelValue);
            
            if (_thrusterPowerLevelValue >= 10f)
            {
                _uIManager.ThrusterSliderColor(true);

                _canThrustersBeUsed = true;
                StopCoroutine(ThrustersCoolDownRoutine());
            }
        }
        
    }
    
}
