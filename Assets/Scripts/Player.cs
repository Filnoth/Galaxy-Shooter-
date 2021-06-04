using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    Vector3 offset = new Vector3(0f, 1.05f, 0f);
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    private bool _tripleShotActive = false;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    private bool _speedBoostActive = false;
    [SerializeField]
    private float _speedMultiplier = 2;
    private bool _shieldBoostActive = false;
    [SerializeField]
    private GameObject _Shield;

    [SerializeField]
    private int _score;
    [SerializeField]
    private GameObject _rightThruster;
    [SerializeField]
    private GameObject _leftThruster;

    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _lasersfx;
    AudioSource _audioSource;
    [SerializeField]
    private AudioClip _explosionSfx;
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();


        if (_spawnManager == null)
        {
            Debug.LogError("Spawn manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UIManager is NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError("audio source on player is NULL");
        }
        else  
        {
            _audioSource.clip = _lasersfx;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        FireLaser();
    }
    void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -4)
        {
            transform.position = new Vector3(transform.position.x, -4, 0);
        }
        if (transform.position.x > 11.3)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            if (_tripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else 
            {
                Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
            }

           
            _audioSource.Play();

        }

    }

    public void Damage()
    {
        if (_shieldBoostActive == true)
        {
            _shieldBoostActive = false;
            _Shield.SetActive(false);
            return;
        }

        _lives--;

        if (_lives == 2)
        {
            _leftThruster.SetActive(true);
        }

        else if (_lives == 1)
        {
            _rightThruster.SetActive(true);
        }
        _uiManager.UpdateLives(_lives);
        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDown());
    }

    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleShotActive = false;
    }

    public void SpeedBoost()
    {
        _speedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerdown());
    }

    IEnumerator SpeedBoostPowerdown()
    {
        yield return new WaitForSeconds(5.0f);
        _speedBoostActive = false;
        _speed /= _speedMultiplier;

    }

    public void ShieldsActive()
    {
        _shieldBoostActive = true;
        if (_shieldBoostActive == true)
        {
            _Shield.SetActive(true);
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }



}
