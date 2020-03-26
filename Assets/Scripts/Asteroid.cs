using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 30.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    // Start is called before the first frame update
    private SpawnManager _spawnManager;
    private AudioManager _audioManager;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is Null.");

        }

        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("The AudioManager is Null!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward *_rotateSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            if (_audioManager != null)
            {
                _audioManager.PlayExplosionSound();
            }
            Destroy(this.gameObject.GetComponent<Collider2D>());
            _spawnManager.StartSpawning(false);
            Destroy(this.gameObject, 1f);


        }

    }
}
