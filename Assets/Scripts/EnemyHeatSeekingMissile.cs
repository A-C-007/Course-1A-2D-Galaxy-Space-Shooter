using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class EnemyHeatSeekingMissile : MonoBehaviour
{
    
    [SerializeField]
    private GameObject _player;
    private GameObject _target;
    [SerializeField]
    private Transform _targetImg;
    
    private Rigidbody2D _rigidBody;
    [SerializeField]
    private float _speed = 20f;
    [SerializeField]
    private float _rotateSpeed = 500f;
    private GameObject _greenBoss;
    private Enemy _bossScript;





    void Start()
    {
        StartCoroutine(DestroyHomingProjectileRoutine());

        _rigidBody = GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
        {
            _target = _player;
        }
        else if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }
        _greenBoss = GameObject.FindGameObjectWithTag("Boss");
        if( _greenBoss != null)
        {
            _bossScript = _greenBoss.GetComponent<Enemy>();
        }

    }


    void FixedUpdate()
    {

        if (_target != null)
        {
            _rigidBody.velocity = -transform.up * _speed;

            Vector3 direction = _target.transform.position - transform.position;

            float rotateAmount = Vector3.Cross(direction, -transform.up).z;

            _rigidBody.angularVelocity = -1 * rotateAmount * _rotateSpeed;

            //transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(_target.transform.position.x, _target.transform.position.y), _speed * Time.deltaTime);

        }





    }


    void Update()
    {
        if (_target == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player != null)
            {
                _target = _player;
            }
            
        }

        if (_target != null)
        {
            _targetImg = _target.gameObject.transform.Find("target_img");
        }

        if (_targetImg != null)
        {
            _targetImg.gameObject.SetActive(true);

        }

        if (_bossScript != null)
        {
            if(_bossScript.isBossSpinning() == true)
            {
                Destroy(this.gameObject);

            }
            
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enemy Laser Hit: " + other.name);


        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {

                player.Damage();
            }
            if (_targetImg != null)
            {
                _targetImg.gameObject.SetActive(false);
            }
            Destroy(this.gameObject);

        }
        else if (other.tag == "Laser")
        {
            if (_targetImg != null)
            {
                _targetImg.gameObject.SetActive(false);
            }
            Destroy(other.gameObject);
            
            Destroy(this.gameObject);

        }
    }



        IEnumerator DestroyHomingProjectileRoutine()
    {
        yield return new WaitForSeconds(5f);
        if (_target != null)
        {

            if (_targetImg != null)
            {
                _targetImg.gameObject.SetActive(false);
            }
        }
        Destroy(this.gameObject);

    }
}
