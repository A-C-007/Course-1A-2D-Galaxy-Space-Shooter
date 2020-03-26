using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private float _speed = 10;
    [SerializeField]
    private float _spinnerRotateSpeed = 50.0f;
    [SerializeField]
    private float _spinnerFireRate = .2f; // waitforseconds before firing
    private GameObject _playerMain;
    private Player _player;

    private Animator _anim;
    private AudioManager _audioManager;
    [SerializeField]
    private SpawnManager _spawnManager;
    [SerializeField]
    private UIManager _uIManager;
    private Collider2D _enemyCollider;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private GameObject _bossExplosionPrefab;
    private bool _isColliding = false;

    [SerializeField]
    private GameObject _dualEnemyLasersPrefab;
    [SerializeField]
    private GameObject _backwardsLasers;
    [SerializeField]
    private GameObject _bossQuadLasers;
    [SerializeField]
    private float _turnSpeed = 3.0f;
    [SerializeField]
    private float _orbitSpeed = 5f;
    [SerializeField]
    private bool _canOrbit = false;
    [SerializeField]
    private bool _canFaceAndFireAtPlayer = false;
    [SerializeField]
    private int _bossHealth = 10;
    [SerializeField]
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private Transform _targetImg;
    [SerializeField]
    private GameObject _bossDamage;
    [SerializeField]
    private bool _canFireAtPickups = false;
    [SerializeField]
    private bool _canFireBackwards = false;
    private int _shieldRandomizerNumber;
    private int _diagonalDirectionRanomizerNumber;
    [SerializeField]
    private GameObject _heatSeekingMissilePrefab;

    [SerializeField]
    private float _enemyAggressionDistance = 7.5f;
    private bool _enemyAggressionTriggered = false;
    [SerializeField]
    private float _aggressionSpeed = 8f;
    [SerializeField]
    private float _laserDistance = 7f;
    [SerializeField]
    private bool _canDodgeLaser = false;
    [SerializeField]
    private float _dodgeSpeed = 8f;
    [SerializeField]
    private float _distanceToMoveFromLaser;
    [SerializeField]
    private bool _didPlayerRamShield;
    [SerializeField]
    private float _moveX;
    private float _randMoveX;
    [SerializeField]
    private float rand;
    private bool _switchDirection;

    private Vector3 _bossCenterWayPoint, _bossLeftWayPoint, _bossRightWayPoint;
    [SerializeField]
    private bool _bossSideToSideMoving;
    [SerializeField]
    private float _bossSideToSideSpeed;
    [SerializeField]
    private bool _isBossSpinning;


    private Vector3 _centerOfScreen = new Vector3(0, 0, 0);

    private float _randomPos;

    public enum EnemyType
    {
        Regular,
        Orbital,
        Diagonal,
        Kamikaze,
        Spinner,
        ZigZag,
        Heat_Seeking_Missile,
        Avoider,
        Boss
    }

    [SerializeField]
    private EnemyType _selectedEnemyType;

    public enum BossMovementType
    {
        Begin,
        SideToSide,
        Spin
    }

    [SerializeField]
    private BossMovementType _selectedBossMovementType;

    // Start is called before the first frame update
    void Start()
    {
        _randomPos = Random.Range(0f, 4f);
        _playerMain = GameObject.Find("Player");
        _player = _playerMain.GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_player == null)
        {
            Debug.LogError("PLAYER IS NULL");

        }
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animator is Null.");


        }
        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("The AudioManager is Null!");
        }
        if (_spawnManager == null)
        {
            Debug.Log("spawn manager is null");

        }
        if (_uIManager == null)
        {
            Debug.Log("UImanager is null");

        }
        _enemyCollider = this.gameObject.GetComponent<Collider2D>();

        StartCoroutine(FireEnemyLasersRoutine());
        if(_canFireAtPickups == true)
        {
            StartCoroutine(FireAtPickupsRoutine());
        }
        
        if(_canFireBackwards == true)
        {
            StartCoroutine(FireBackawardsAtPlayerRoutine());
        }


        _diagonalDirectionRanomizerNumber = Random.Range(1, 10);
        
        _shieldRandomizerNumber = Random.Range(1, 5); //make shields on random on start
        if(_shieldRandomizerNumber == 3)
        {
            if (_selectedEnemyType != EnemyType.Boss)
            {
                _isShieldActive = true;
            }
            
        }
        else
        {
            _isShieldActive = false;
        }

        if (_isShieldActive == true)
        {
            _shield.SetActive(true);
        }

        _targetImg = transform.Find("target_img");

        _bossCenterWayPoint = new Vector3(0, 3.28f, 0);
        _bossLeftWayPoint = new Vector3(-8f, 3.28f, 0);
        _bossRightWayPoint = new Vector3(8f, 3.28f, 0);
        _bossSideToSideMoving = false;


    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyAggressionTriggered == false)
        {
            switch (_selectedEnemyType)
            {

                case EnemyType.Regular:
                    RegularEnemyMovement();
                    break;
                case EnemyType.Orbital:
                    OrbitalEnemyMovement();

                    break;
                case EnemyType.Diagonal:
                    DiagonalEnemyMovement();
                    break;
                case EnemyType.Kamikaze:
                    KamikazeEnemyMovement();

                    break;
                case EnemyType.Spinner:
                    SpinnerEnemyMovement();
                    break;
                case EnemyType.Boss:
                    BossMovement();
                    break;
                case EnemyType.ZigZag:
                    Debug.Log("EnemyType ZigZag selected");
                    break;
                case EnemyType.Heat_Seeking_Missile:
                    RegularEnemyMovement();
                    break;
                case EnemyType.Avoider:
                    Debug.Log("avoidermovement called");
                    AvoiderEnemyMovement();
                    break;

            }
        }
        



        if (transform.position.y <= -5.5f && _canOrbit == false)
        {
            float randomX = Random.Range(-9.42f, 9.42f);
            transform.position = new Vector3(randomX, 6.93f, 0);

        }

        if (transform.position.y <= -5.5f && _player == null)
        {
            Destroy(this.gameObject);
        }

        if (transform.position.y >= 9f && _player == null)
        {

            Destroy(this.gameObject);

        }

        if (transform.position.x <= -12f && _player == null)
        {

            Destroy(this.gameObject);

        }
        if (transform.position.x >= 12f && _player == null)
        {

            Destroy(this.gameObject);

        }


        if (_player == null)
        {
            StopCoroutine(FireEnemyLasersRoutine());
            Destroy(this.gameObject, 5f);

        }



        EnemyAggression();
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit: " + other.name);

        if (_isColliding == false)
        {
            if (other.tag == "Player")
            {

                if (this.transform.tag != "Boss")
                {
                    if (_isShieldActive == true)
                    {
                        _didPlayerRamShield = true;
                        _isColliding = true;
                        
                        if (_audioManager != null)
                        {
                            _audioManager.PlayShieldHitSound();
                        }
                        _shield.SetActive(false);
                        _isShieldActive = false;
                        _isColliding = false;
                    }
                    else
                    {
                        _isColliding = true;
                        Destroy(this.gameObject.GetComponent<Collider2D>());




                        Player player = other.transform.GetComponent<Player>();

                        if (player != null)
                        {

                            player.Damage();
                        }
                        _speed = 0;
                        //_anim.SetTrigger("OnEnemyDeath");
                        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                        if (_audioManager != null)
                        {
                            _audioManager.PlayExplosionSound();
                        }

                        //Destroy(this.gameObject, 2.05f);
                        Destroy(this.gameObject);
                    }


                    
                }
            }
            else if (other.tag == "Laser" || other.tag == "GreenOrb")

            {
                
                
                if (transform.tag == "Boss")
                {
                    _isColliding = true;
                    if (_player != null)
                    {

                        _player.UpdateScore(10);
                    }
                    if(_isBossSpinning != true)
                    {
                        GameObject bossDamage = Instantiate(_bossDamage, other.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
                        bossDamage.transform.parent = this.transform;
                    }
                    else if(_isBossSpinning == true)
                    {
                        GameObject bossDamage = Instantiate(_bossDamage, transform.position, Quaternion.identity);
                        bossDamage.transform.parent = this.transform;
                    }
                    if (_audioManager != null)
                    {
                        _audioManager.PlayEngineExplosionSound();
                    }
                    if(_targetImg != null)
                    {
                        _targetImg.gameObject.SetActive(false);
                    }
                    Destroy(other.gameObject);
                    BossDamage();
                    if (_targetImg != null)
                    {
                        _targetImg.gameObject.SetActive(false);
                    }

                }
                else
                {

                    if (_isShieldActive == true)
                    {
                        
                        _isColliding = true;
                        Destroy(other.gameObject);
                        _shield.SetActive(false);
                        _isShieldActive = false;
                        if (_targetImg != null)
                        {
                            _targetImg.gameObject.SetActive(false);
                        }
                        if (_audioManager != null)
                        {
                            _audioManager.PlayShieldHitSound();
                        }
                        
                        _isColliding = false;
                    }
                    else
                    {
                        _isColliding = true;
                        Destroy(this.gameObject.GetComponent<Collider2D>());
                        if (_player != null)
                        {

                            _player.UpdateScore(10);
                        }
                        Destroy(other.gameObject);
                        _speed = 0;
                        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                        if (_audioManager != null)
                        {
                            _audioManager.PlayExplosionSound();
                        }
                        //Destroy(this.gameObject.GetComponent<Collider2D>());
                        //Destroy(this.gameObject, 2.05f);

                        Destroy(this.gameObject);
                    }
                    
                }
                


                //_anim.SetTrigger("OnEnemyDeath");




            }

        }
        else return;


    }

    IEnumerator FireEnemyLasersRoutine()
    {



        while (_player !=null)
        {
            
                if (_canFaceAndFireAtPlayer == true && (_selectedEnemyType == EnemyType.Orbital || _selectedEnemyType == EnemyType.Kamikaze))
                {
                    yield return new WaitForSeconds(Random.Range(1f, 3f));
                    if (_enemyCollider != null)
                    {
                        GameObject newEnemyLasers = Instantiate(_dualEnemyLasersPrefab, transform.position, Quaternion.identity);
                        newEnemyLasers.transform.parent = this.transform;
                    }
                }
                else if (_selectedEnemyType == EnemyType.Spinner)
                {
                    yield return new WaitForSeconds(_spinnerFireRate);
                    if (_enemyCollider != null && transform.position.y < _randomPos)
                    {
                        GameObject newEnemyLasers = Instantiate(_dualEnemyLasersPrefab, transform.position, Quaternion.identity);
                        newEnemyLasers.transform.parent = this.transform;
                    }
                }
            else if (_selectedEnemyType == EnemyType.Heat_Seeking_Missile)
            {
                yield return new WaitForSeconds(Random.Range(2f, 6f));
                if (_enemyCollider != null && transform.position.y < _randomPos)
                {
                    GameObject newHeatSeekingMissile = Instantiate(_heatSeekingMissilePrefab, transform.position, Quaternion.identity);
                    newHeatSeekingMissile.transform.parent = this.transform;
                }
            }
            else if (_selectedEnemyType == EnemyType.Boss)
            {
                
                if (_enemyCollider != null && _selectedBossMovementType == BossMovementType.Begin)
                {
                    yield return new WaitForSeconds(Random.Range(1f, 3f));
                    Instantiate(_bossQuadLasers, transform.position, Quaternion.identity);
                    
                }
                else if(_enemyCollider != null && _selectedBossMovementType == BossMovementType.Spin)
                {
                    yield return new WaitForSeconds(_spinnerFireRate);
                    GameObject newBossLasers = Instantiate(_bossQuadLasers, transform.position, Quaternion.identity);
                    newBossLasers.transform.parent = this.transform;
                }
                else if (_enemyCollider != null && _selectedBossMovementType == BossMovementType.SideToSide)
                {
                    yield return new WaitForSeconds(Random.Range(3f, 4f));
                    
                        GameObject newHeatSeekingMissile = Instantiate(_heatSeekingMissilePrefab, transform.position, Quaternion.identity);
                        newHeatSeekingMissile.transform.parent = transform.parent;
                    
                }
            }
            
            else
                {
                    yield return new WaitForSeconds(Random.Range(2f, 6f));
                    if (_enemyCollider != null)
                    {
                        GameObject newEnemyLasers = Instantiate(_dualEnemyLasersPrefab, transform.position, Quaternion.identity);
                        newEnemyLasers.transform.parent = transform.parent;
                    }
                }

            
        }

    }

    IEnumerator FireAtPickupsRoutine()
    {
        
        while (true)
        {
            if (_selectedEnemyType != EnemyType.Boss)
            {
                
                var direction = transform.TransformDirection(-Vector2.up) * 10;
                var origin = transform.position;
                RaycastHit2D hit = Physics2D.Raycast(origin, -Vector2.up);
                Debug.DrawRay(origin, direction, Color.green);
                if (hit)
                {
                    if (hit.collider.tag == "Pickup")
                    {
                        _canFireAtPickups = true;
                        Debug.Log("Raycast hit: " + hit.collider.tag);
                        if (_enemyCollider != null)
                        {

                            GameObject fireAtPickupLasers = Instantiate(_dualEnemyLasersPrefab, transform.position, Quaternion.identity);
                            fireAtPickupLasers.transform.parent = this.transform;

                            

                        }
                        




                    }
                    

                }
                
            }
            
            yield return new WaitForSeconds(0.3f);
        }
        

        
        
        
    }

    IEnumerator FireBackawardsAtPlayerRoutine()
    {
        while (true)
        {
            if (_selectedEnemyType != EnemyType.Boss && _selectedEnemyType != EnemyType.Spinner)
            {

                var direction = transform.TransformDirection(Vector2.up) * 20;
                var origin = transform.position;
                RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.up, 20f);
                Debug.DrawRay(origin, direction, Color.green);
                if (hit)
                {
                    if (hit.collider.tag == "Player")
                    {
                        
                        Debug.Log("Raycast hit: " + hit.collider.tag);
                        if (_enemyCollider != null)
                        {

                            GameObject fireBackwarsAtPlayerLasers = Instantiate(_backwardsLasers, transform.position, Quaternion.identity);
                            fireBackwarsAtPlayerLasers.transform.parent = this.transform;



                        }
                        



                    }
                   

                }

            }
            
            yield return new WaitForSeconds(0.3f);
        }
    }
    
    
    void RegularEnemyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);


    }
    

    void OrbitalEnemyMovement()
    {
        if(_player != null)
        {
            if (transform.position.y > 3.7)
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
            }



            if (transform.position.y < 4f || Vector3.Distance(_playerMain.transform.position, transform.position) < 6f);
                
            {
                _canOrbit = true; 
            }
                if (_canOrbit == true)
            {


                
                Vector3 vectorToTarget = _playerMain.transform.position - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 10);
                transform.RotateAround(_playerMain.transform.position, Vector3.forward, _orbitSpeed);
            }
                

        }

        if(_player == null)
        {
            _canOrbit = false;
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        
    }

    void DiagonalEnemyMovement()
    {
        if (_diagonalDirectionRanomizerNumber <= 5) 
        {
            transform.Translate(new Vector3(0.5f, -1, 0) * _speed * Time.deltaTime); //right and down
        }
        else if(_diagonalDirectionRanomizerNumber >= 6) 
        {
            transform.Translate(new Vector3(-0.5f, -1, 0) * _speed * Time.deltaTime); //left and down
        }
        
        
    }

    void KamikazeEnemyMovement()
    {

        if (_player != null)
        {
            
            var relativePos = _player.transform.position - transform.position;
            var angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg + 90;
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rotation;

            
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            
        }

        if (_player == null)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }


    }

    void BossMovement()
    {
        if (_selectedBossMovementType == BossMovementType.Begin)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            if (transform.position.y <= 3.28f)
            {
                _speed = 0f;
            }

            GameObject laser = GameObject.FindGameObjectWithTag("Laser");


            if (laser != null)
            {
                Debug.Log("laser is not null for avoiding");
                float distance = Mathf.Infinity;
                Vector3 position = transform.position;

                Vector3 diff = laser.transform.position - position;
                float currentDistance = diff.sqrMagnitude;
                distance = currentDistance;
                //Debug.Log(" laser sqrMagnitude is : " + distance.ToString());
                if (currentDistance < _laserDistance)
                {
                    if (_canDodgeLaser == true)
                    {


                        switch (_switchDirection)
                        {
                            case true:
                                _moveX = transform.position.x + _distanceToMoveFromLaser;

                                break;
                            case false:
                                _moveX = transform.position.x - _distanceToMoveFromLaser;

                                break;

                        }
                        _switchDirection = !_switchDirection;


                        _canDodgeLaser = false;
                    }

                    transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(_moveX, transform.position.y), _dodgeSpeed * Time.deltaTime);




                }


            }
            else if (laser == null)
            {
                _canDodgeLaser = true;
            }

            if(_bossHealth == 8)
            {
                _selectedBossMovementType = BossMovementType.SideToSide;
            }
        }


        else if(_selectedBossMovementType == BossMovementType.SideToSide)
        {

            
             
            if (_bossSideToSideMoving == false)
            {
                transform.position = Vector2.MoveTowards(transform.position, _bossRightWayPoint, _bossSideToSideSpeed * Time.deltaTime);
                
            }
            else if(_bossSideToSideMoving == true)
            {
                transform.position = Vector2.MoveTowards(transform.position, _bossLeftWayPoint, _bossSideToSideSpeed * Time.deltaTime);
                
            }
            if (transform.position == _bossLeftWayPoint)
            {
                _bossSideToSideMoving = false;
            }
            else if (transform.position == _bossRightWayPoint)
            {
                _bossSideToSideMoving = true;
            }




            if (_bossHealth == 4)
            {
                _isBossSpinning = true;
                _selectedBossMovementType = BossMovementType.Spin;
            }
        }


        else if(_selectedBossMovementType == BossMovementType.Spin)
        {
            
            if (_player == null)
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
            }


            if (_player != null)
            {
                transform.Rotate(Vector3.forward * _spinnerRotateSpeed * Time.deltaTime);
            }


        }
        

    }

    void AvoiderEnemyMovement()
    {
        
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        
            
        
       GameObject laser = GameObject.FindGameObjectWithTag("Laser");
        
       
        if (laser != null)
        {
            Debug.Log("laser is not null for avoiding");
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;

            Vector3 diff = laser.transform.position - position;
            float currentDistance = diff.sqrMagnitude;
            distance = currentDistance;
            //Debug.Log(" laser sqrMagnitude is : " + distance.ToString());
            if (currentDistance < _laserDistance)
            {
                if (_canDodgeLaser == true)
                {


                    switch (_switchDirection)
                    {
                        case true:
                            _moveX = transform.position.x + _distanceToMoveFromLaser;
                            
                            break;
                        case false:
                            _moveX = transform.position.x - _distanceToMoveFromLaser;
                            
                            break;

                    }
                    _switchDirection = !_switchDirection;
                    
                    
                    _canDodgeLaser = false;
                }
                
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(_moveX, transform.position.y), _dodgeSpeed * Time.deltaTime);


                

            }
            

        }
        else if (laser == null)
        {
            _canDodgeLaser = true;
        }
        
        
    }

    public bool CanFaceAndFireAtPlayer()
    {
        return _canFaceAndFireAtPlayer;
    }

    void SpinnerEnemyMovement()
    {

        

        if (transform.position.y > _randomPos || _player ==  null)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
            

        if (transform.position.y < _randomPos && _player != null)
        {
            transform.Rotate(Vector3.forward * _spinnerRotateSpeed * Time.deltaTime);
        }
    }

    void BossDamage()
    {
        _bossHealth--;
        _isColliding = false;
        if(_bossHealth <= 0)
        {
            if (_spawnManager != null)
            {
                _spawnManager.StopEnemySpawnRoutine();
            }
            if (_uIManager != null)
            {
                _uIManager.YouWinSequence();
            }
            Instantiate(_bossExplosionPrefab, transform.position, Quaternion.identity);
            if (_audioManager != null)
            {
                _audioManager.PlayExplosionSound();
            }
            Destroy(this.gameObject);

        }
    }


    void EnemyAggression()
    {
        //if player != null
        //if player is within small distance
        //movetowards player with increased speed to ram player
        if (_shield != null && _selectedEnemyType != EnemyType.Boss)
        {
            if (_playerMain != null && _shield.activeSelf == false && _didPlayerRamShield == false)
            {


                float distance = Mathf.Infinity;
                Vector3 position = transform.position;

                Vector3 diff = _playerMain.transform.position - position;
                float currentDistance = diff.sqrMagnitude;
                distance = currentDistance;
                //Debug.Log("sqrMagnitude is : " + distance.ToString());
                if (currentDistance < _enemyAggressionDistance)
                {
                    _enemyAggressionTriggered = true;
                    var relativePos = _player.transform.position - transform.position;
                    var angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg + 90;
                    var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    transform.rotation = rotation;


                    transform.Translate(Vector3.down * _aggressionSpeed * Time.deltaTime);
                }

            }

            else if (_playerMain == null)
            {
                _enemyAggressionTriggered = false;
            }
        }






    }

    public bool isBossSpinning()
    {
        return _isBossSpinning;
    }

}
