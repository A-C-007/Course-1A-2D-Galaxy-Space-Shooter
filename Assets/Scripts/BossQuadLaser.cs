using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossQuadLaser : MonoBehaviour
{

    [SerializeField]
    private Transform _parent;
    [SerializeField]
    private Enemy _enemyScript;
    [SerializeField]
    private bool _canFireInPlayerDirection = false;

    private GameObject _player;
    [SerializeField]
    private GameObject _enemyContainer;
    // Start is called before the first frame update
    void Start()
    {
        _parent = transform.parent;
        if (_parent != null)
        {
            transform.rotation = _parent.transform.rotation;
        }



        if (_parent == null)
        {
            Debug.Log("parent is null");

        }

        if (_parent != null )
        {
            _enemyScript = _parent.GetComponent<Enemy>();
        }


        if (_enemyScript == null)
        {
            Debug.Log("Enemy script is null");
        }

        _player = GameObject.Find("Player");
        if (_player == null)
        {
            Debug.Log("Player is null");

        }

        _enemyContainer = GameObject.Find("Enemy Container");
        if (_enemyContainer == null)
        {
            Debug.Log("Enemy Container is null");

        }
        if (_enemyScript != null )
        {
            //if (_enemyScript.CanFaceAndFireAtPlayer() == true)
            //{
                transform.rotation = _parent.transform.rotation;
                transform.parent = _enemyContainer.transform;
                //_canFireInPlayerDirection = true;

                //if (_player != null)
                //{
                //    Transform nearestT = _player.transform;
                //    Transform target = nearestT;
                //    Vector3 vectorToTarget = target.position - transform.position;
                //    float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                //    Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                //    transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * _speed);
                //}

           // }
        }
        if (_parent != null)
        {
            Debug.Log("Boss lasers Parent: " + gameObject.transform.parent.name);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
