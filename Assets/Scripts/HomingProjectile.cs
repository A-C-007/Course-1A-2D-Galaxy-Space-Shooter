using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HomingProjectile : MonoBehaviour
{
    [SerializeField]
    private GameObject _Enemy;
    private Transform _target;
    private Rigidbody2D _rigidBody;
    [SerializeField]
    private float _speed = 20f;
    [SerializeField]
    private float _rotateSpeed = 500f;

    void Start()
    {
        StartCoroutine(DestroyHomingProjectileRoutine());
        _rigidBody = GetComponent<Rigidbody2D>();
        _Enemy = GameObject.FindGameObjectWithTag("Enemy");
        if (_Enemy == null)
        {
            Debug.Log("enemy is null.");

        }
        else
        {

            Transform _targetImg = _Enemy.gameObject.transform.Find("target_img");
            if (_targetImg != null)
            {
                _targetImg.gameObject.SetActive(true);

            }
            _target = _Enemy.transform;
            
        }
    }

    
    void FixedUpdate()
    {
        if(_Enemy == null)
        {
            StartCoroutine(FindEnemyRoutine());

        }
        if(_target != null)
        {
            _rigidBody.velocity = transform.up * _speed;
            
            Vector3 direction = _target.position - transform.position;
           
            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            _rigidBody.angularVelocity = -1 * rotateAmount * _rotateSpeed;
            
        }
       
        



    }



    IEnumerator DestroyHomingProjectileRoutine()
    {
        yield return new WaitForSeconds(3f);
        if (_Enemy != null)
      { 
        Transform _targetImg = _Enemy.gameObject.transform.Find("target_img");
        if (_targetImg != null)
        {
            _targetImg.gameObject.SetActive(false);
        }
      }
        Destroy(this.gameObject);

    }
    IEnumerator FindEnemyRoutine()
    {
        Debug.Log("FindEnemyRoutine Called");

        while(_Enemy == null)
        {
            yield return new WaitForSeconds(.5f);
            _Enemy = GameObject.FindGameObjectWithTag("Enemy");
            if (_Enemy == null)
            {
                Debug.Log("enemy is null.");

            }
            else
            {
                Transform _targetImg = _Enemy.gameObject.transform.Find("target_img");
                if (_targetImg != null)
                {
                    _targetImg.gameObject.SetActive(true);

                }
                _target = _Enemy.transform;
            }
        }
    }
}
