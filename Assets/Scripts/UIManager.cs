using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    //handle ot text
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _ammoCountText;
    private Player _player;
    [SerializeField]
    private Text _outOfAmmo;
    [SerializeField]
    private Slider _thrustBar;
    private bool _isThrusting = false;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _thrustBar.value = 10;
        
    }


    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];

        if(currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(BlinkText());

    }
    IEnumerator BlinkText()
    {
        while(true)
        {
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
        }
    }

   /* public IEnumerator NoAmmo()
    {
        while(true)
        {
            _ammoCountText.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            _ammoCountText.color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }
    }*/

    public void UpdateAmmo(int ammoCount, int maxAmmo)
    
    {
        _ammoCountText.text = "Ammo: " + ammoCount.ToString() + "/" + maxAmmo;
        if (_player.ammo == 0)
        {
            _ammoCountText.color = Color.red;
        }

        else
        {
            _ammoCountText.color = Color.white;
        }
    }

    public void Update()
    {
        if(!_isThrusting)
        {
            _thrustBar.value += Time.deltaTime;
        }
        else
        {
            _thrustBar.value -= Time.deltaTime * 3;
            if(_thrustBar.value <= 0)
            {
                _isThrusting = false;
                _player.Thrusters(false);
            }
        }
    }

    public void ThrustEnabled()
    {
        
        if (_thrustBar.value >= 3)
        {
            _isThrusting = true;
            _player.Thrusters(true);
        }
    }

}
