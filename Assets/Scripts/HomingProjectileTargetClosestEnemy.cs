using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[RequireComponent(typeof(Rigidbody2D))]

public class HomingProjectileTargetClosestEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemies;
    [SerializeField]
    private GameObject _greenBoss;
    private GameObject _target;
    private Transform _targetImg;
    private Rigidbody2D _rigidBody;
    [SerializeField]
    private float _speed = 20f;
    

    



    
    void Start()
    {
        StartCoroutine(DestroyHomingProjectileRoutine());
        
        _rigidBody = GetComponent<Rigidbody2D>();
        _greenBoss = GameObject.FindGameObjectWithTag("Boss");
        if(_greenBoss != null)
        {
            _target = _greenBoss;
        }
        else if (_greenBoss == null)
        {
            _target = FindClosestEnemy();
        }
        
        
    }


    void FixedUpdate()
    {
        
        if (_target != null)
        {
            /*_rigidBody.velocity = transform.up * _speed;

            Vector3 direction = _target.transform.position - transform.position;

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            _rigidBody.angularVelocity = -1 * rotateAmount * _rotateSpeed;*/

            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(_target.transform.position.x, _target.transform.position.y), _speed * Time.deltaTime);

        }





    }


    void Update()
    {
        if(_target == null)
        {
            _greenBoss = GameObject.FindGameObjectWithTag("Boss");
            if (_greenBoss != null)
            {
                _target = _greenBoss;
            }
            else if (_greenBoss == null)
            {
                _target = FindClosestEnemy();
            }
        }

        if(_target != null)
        {
            _targetImg = _target.gameObject.transform.Find("target_img");
        }
        
        if (_targetImg != null)
        {
            _targetImg.gameObject.SetActive(true);

        }
    }

    public GameObject FindClosestEnemy()
    {
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach(GameObject enemy in _enemies)
        {
            Vector3 diff = enemy.transform.position - position;
            float currentDistance = diff.sqrMagnitude;
            if (currentDistance < distance)
            {
                closest = enemy;
                distance = currentDistance;
            }
        }
        return closest;

    }


    IEnumerator DestroyHomingProjectileRoutine()
    {
        yield return new WaitForSeconds(3f);
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