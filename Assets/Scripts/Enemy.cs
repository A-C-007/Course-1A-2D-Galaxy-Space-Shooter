using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private float _speed = 4;
    
    private Player _player;

    private Animator _anim;
    private AudioManager _audioManager;
    private Collider2D _enemyCollider;
    [SerializeField]
    private GameObject _explosionPrefab;
    private bool _isColliding = false;
    
    [SerializeField]
    private GameObject _dualEnemyLasersPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("PLAYER IS NULL");

        }
        _anim = GetComponent<Animator>();
        if(_anim == null)
        {
            Debug.LogError("Animator is Null.");


        }
        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("The AudioManager is Null!");
        }

        _enemyCollider = this.gameObject.GetComponent<Collider2D>();

        StartCoroutine(FireEnemyLasersRoutine());

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5.5f)
        {
            float randomX = Random.Range(-9.42f, 9.42f);
            transform.position = new Vector3(randomX, 6.93f, 0);

        }
            
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit: " + other.name);

        if (_isColliding == false)
        {
            if (other.tag == "Player")
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
            else if (other.tag == "Laser")
            {
                _isColliding = true;
                Destroy(this.gameObject.GetComponent<Collider2D>());
                Destroy(other.gameObject);
                if (_player != null)
                {

                    _player.UpdateScore(10);
                }
                _speed = 0;
                //_anim.SetTrigger("OnEnemyDeath");
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
        else return;


    }

    IEnumerator FireEnemyLasersRoutine()
    {
        


        while (true)
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
