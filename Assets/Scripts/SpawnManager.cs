using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;

    private bool _stopSpawning = false;

    void Start()
    {
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EnemySpawnRoutine()
    {
        yield return new WaitForSeconds(1f);

        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            float randomX = Random.Range(-9.42f, 9.42f);
            
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(randomX, 6.93f, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            
        }
        

    }

    public void StopEnemySpawnRoutine()
    {
        _stopSpawning = true;
    }

    IEnumerator PowerupSpawnRoutine()
    {
        yield return new WaitForSeconds(1f);
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(Random.Range(3f, 7f));
            float randomX = Random.Range(-9.42f, 9.42f);
            Instantiate(_powerups[Random.Range(0, _powerups.Length)], new Vector3(randomX, 6.93f, 0), Quaternion.identity);
            

        }
    }

    public void StartSpawning()
    {
        
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerupSpawnRoutine());
    }
}
