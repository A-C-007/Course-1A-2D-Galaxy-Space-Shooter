using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    
    [SerializeField]  //0 = Triple Shot 1 = Speed 2 = Shields 3 = Ammo 4 = Health
    private int _powerupID;

    private AudioManager _audioManager;

    // Update is called once per frame
    private void Start()
    {
        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("The AudioManager is Null!");
        }
    }
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y <= -5.5f)
        {
            Destroy(this.gameObject);


        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {

                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.AmmoPowerup();
                        break;
                    case 4:
                        player.HealthPowerup();
                        break;
                    case 5:
                        player.HomingGreenOrbPowerup();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;

                }
                
            }
            if (_audioManager != null)
            {
                _audioManager.PlayPowerupSound();
            }
            Destroy(this.gameObject);


        }
    }
}
