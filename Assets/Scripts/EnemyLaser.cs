using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    [SerializeField]
    private AudioClip _laserShotClip;
    private AudioSource _audioSource;
    [SerializeField]
    private Transform _parent;
    [SerializeField]
    private Enemy _enemyScript;
    [SerializeField]
    private bool _canFireInPlayerDirection = false;
    
    private GameObject _player;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private bool _isBackwardsFiringLaser;
    [SerializeField]
    private bool _isBossLaser;

    private void Start()
    {

        
        _audioSource = GetComponent<AudioSource>();
       
            AudioSource.PlayClipAtPoint(_laserShotClip, new Vector3(0, 0, -15));
        
        

       
       _parent = transform.parent;
        if (_parent != null && _isBossLaser == false)
        {
            transform.rotation = _parent.transform.rotation;
        }
        


        if (_parent == null)
        {
            Debug.Log("parent is null");

        }

        if (_parent != null && _isBossLaser == false)
            {
            _enemyScript = _parent.GetComponent<Enemy>();
            }
        

        if (_enemyScript == null )
        {
            Debug.Log("Enemy script is null");
        }

        _player = GameObject.Find("Player");
        if (_player == null)
        {
            Debug.Log("Player is null");

        }
        
        _enemyContainer = GameObject.Find("Enemy Container");
        if(_enemyContainer == null)
        {
            Debug.Log("Enemy Container is null");

        }
        if (_enemyScript != null && _isBossLaser == false)
        {
            if (_enemyScript.CanFaceAndFireAtPlayer() == true)
            {
                transform.rotation = _parent.transform.rotation;
                transform.parent = _enemyContainer.transform;
                _canFireInPlayerDirection = true;

                if (_player != null)
                {
                    Transform nearestT = _player.transform;
                    Transform target = nearestT;
                    Vector3 vectorToTarget = target.position - transform.position;
                    float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                    Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * _speed);
                }
                
            }
        }
        if (_parent != null)
        {
            Debug.Log("Enemy lasers Parent: " + gameObject.transform.parent.name);
        }
        

        

    }


    void Update()
    {
        if(_isBackwardsFiringLaser == true)
        {
            transform.Translate(-Vector3.down * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
            
        
            
        
            
        

        if (transform.position.y <= -5.09f)
        {
            
            Destroy(this.gameObject);

        }
        if (transform.position.y >= 9f)
        {

            Destroy(this.gameObject);

        }

        if (transform.position.x <= -10f)
        {

            Destroy(this.gameObject);

        }
        if (transform.position.x >= 10f)
        {

            Destroy(this.gameObject);

        }
        /*if(_player == null)
        {
            Destroy(this.gameObject);
        }*/
        Destroy(this.gameObject, 2f);

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

            Destroy(this.gameObject);

        }
        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            Destroy(this.gameObject);

        }
        else if (other.tag == "Pickup")
        {
            Destroy(other.gameObject);

            Destroy(this.gameObject);
        }

    }

    
}
