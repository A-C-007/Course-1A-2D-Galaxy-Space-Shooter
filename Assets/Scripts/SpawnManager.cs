using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private GameObject[] _rarePowerups; // set to spawn random range between 30-60 seconds
    [SerializeField]
    private GameObject[] _Enemies;

    [SerializeField]
    private GameObject _ammoPowerup;
        
    [SerializeField]
    private GameObject _greenBoss;
    private bool _canSpawnBoss;
    [SerializeField]
    private int _wave2BeginScoreAmount, _wave3BeginScoreAmount, _bossWaveBeginScoreAmount; //must be in increments of 10 for it to work with current score system of 10 points per hit
        
    [SerializeField]
    private UIManager _uIManager;
    [SerializeField]
    private AudioManager _audioManager;
    
    [SerializeField]
    private bool _stopSpawning = true;
    [SerializeField]
    private bool _canStartBossWaveRoutine;

   

    public enum Wave
    {
        Wave1,
        Wave2,
        Wave3,
        Boss
    }

    [SerializeField]
    private Wave _wave;
    

    void Start()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerupSpawnRoutine());
        StartCoroutine(RarePowerupSpawnRoutine());
        StartCoroutine(BossSpawnRoutine());
        StartCoroutine(AmmoSpawnRoutine());


    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    IEnumerator EnemySpawnRoutine()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            while (_stopSpawning == false && _wave == Wave.Wave1)
            {
                yield return new WaitForSeconds(Random.Range(1f, 5f));
                float randomX = Random.Range(-9.42f, 9.42f);

                GameObject newEnemy = Instantiate(_Enemies[Random.Range(0, _Enemies.Length)], new Vector3(randomX, 6.93f, 0), Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;

            }

            while (_stopSpawning == false && _wave == Wave.Wave2)
            {
                Debug.Log("Wave 2 Reached");

                yield return new WaitForSeconds(Random.Range(1f, 5f));
                float randomX = Random.Range(-9.42f, 9.42f);

                GameObject newEnemy = Instantiate(_Enemies[Random.Range(0, _Enemies.Length)], new Vector3(randomX, 6.93f, 0), Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;

            }

            while (_stopSpawning == false && _wave == Wave.Wave3)
            {
                Debug.Log("Wave 3 Reached");
                yield return new WaitForSeconds(Random.Range(1f, 5f));
                float randomX = Random.Range(-9.42f, 9.42f);

                GameObject newEnemy = Instantiate(_Enemies[Random.Range(0, _Enemies.Length)], new Vector3(randomX, 6.93f, 0), Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;

            }
            yield return new WaitForSeconds(1f);
        }
        
        

        

    }

    IEnumerator BossSpawnRoutine()
    {
        
        yield return new WaitForSeconds(2f);
        
        
        while (true)
        {
            yield return new WaitForSeconds(2f);
            GameObject[] _enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if ((_enemies.Length < 1 || _enemies == null) && _canStartBossWaveRoutine == true)
            {
                _audioManager.PlayEnemyArrivingSoundSound();
                StartWaveRoutine(4);
                _canStartBossWaveRoutine = false;
            }
            if (_stopSpawning == false && _canSpawnBoss == true && _wave == Wave.Boss)
            {
                Debug.Log("Boss Reached");
                
                yield return new WaitForSeconds(2f);


                GameObject greenBoss = Instantiate(_greenBoss, new Vector3(0, 6.93f, 0), Quaternion.identity);
                greenBoss.transform.parent = _enemyContainer.transform;
                _canSpawnBoss = false;
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(1f);
        }
      
    }

    public void StopEnemySpawnRoutine()
    {
        _stopSpawning = true;
    }

    IEnumerator PowerupSpawnRoutine()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            if (_stopSpawning == false)
            {
                yield return new WaitForSeconds(Random.Range(5f, 7f));
                float randomX = Random.Range(-9.42f, 9.42f);
                Instantiate(_powerups[Random.Range(0, _powerups.Length)], new Vector3(randomX, 6.93f, 0), Quaternion.identity);

            }

            yield return new WaitForSeconds(1f);
        }
        
    }
    IEnumerator AmmoSpawnRoutine()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            if (_stopSpawning == false)
            {
                yield return new WaitForSeconds(Random.Range(10f, 20f));
                float randomX = Random.Range(-9.42f, 9.42f);
                Instantiate(_ammoPowerup, new Vector3(randomX, 6.93f, 0), Quaternion.identity);

            }

            yield return new WaitForSeconds(1f);
        }

    }
    IEnumerator RarePowerupSpawnRoutine()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            if(_stopSpawning == false)
            {
                yield return new WaitForSeconds(Random.Range(40f, 60f));
                float randomX = Random.Range(-9.42f, 9.42f);
                Instantiate(_rarePowerups[Random.Range(0, _rarePowerups.Length)], new Vector3(randomX, 6.93f, 0), Quaternion.identity);

            }

            yield return new WaitForSeconds(1f);
        }
    }

    public void StartSpawning(bool canSpawnBoss)
    {if (canSpawnBoss == false)
        {
            _stopSpawning = false;

            

        }
        else
        {
            _stopSpawning = false;
            _canSpawnBoss = true;
            
        }
        
    }

    public void WaveReached(int score)
    {
        var currentTime = Time.time.ToString("");
        if (score == _wave2BeginScoreAmount)
        {
            StartWaveRoutine(2);
            _wave = Wave.Wave2;
        }
        else if (score == _wave3BeginScoreAmount)
        {
            StartWaveRoutine(3);
            _wave = Wave.Wave3;
        }
        else if (score == _bossWaveBeginScoreAmount)
        {
            _stopSpawning = true;
            _wave = Wave.Boss;
            _canStartBossWaveRoutine = true;
        }
        
    }

    void StartWaveRoutine(int waveNumber)
    {
        _stopSpawning = true;
        
            _uIManager.DisplayWaveText(waveNumber);
        
        
        
    }
}
