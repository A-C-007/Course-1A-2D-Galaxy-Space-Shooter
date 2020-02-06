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
  

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        AudioSource.PlayClipAtPoint(_laserShotClip, new Vector3(0, 0, -15));

    }


    void Update()
    {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5.09f)
        {
            
            Destroy(this.gameObject);

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

            Destroy(this.gameObject);

        }
        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            Destroy(this.gameObject);

        }

    }
}
