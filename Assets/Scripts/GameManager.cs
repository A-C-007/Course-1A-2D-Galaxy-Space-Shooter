using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private bool _isGameOver;
    Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _isGameOver = false;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
      
    }

    // Update is called once per frame
    void Update()
    {
        
            if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
            {
                SceneManager.LoadScene(1); //current game scene


            }

            if (Input.GetKeyDown(KeyCode.Escape))
        {

            Application.Quit();

        }
        
    }

    public void GameOver()
    {
        _isGameOver = true;
        if(_player != null)
        {
            _player.enabled = false;
        }
    }


}
