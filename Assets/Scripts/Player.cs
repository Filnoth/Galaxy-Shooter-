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
    private float _thrusterBoost = 3f;
    public int ammo;
    private int _maxAmmo = 15;
    private bool _ammoDepleted = false;
    [SerializeField]
    private GameObject _camera;
    private int _shieldCharge;
    private float _thrustSpeed;
    private float _regularSpeed;
    /*[SerializeField]
    private GameObject _missilePrefab;*/
    private bool _tsunamiShotActive = false;
    [SerializeField]
    private GameObject _tsunamiShotPrefab;
    private bool _shieldMagActive;
    private GameObject[] _powerups;
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        ammo = _maxAmmo;
        _thrustSpeed = _speed * _thrusterBoost;
        _regularSpeed = _speed;


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
        //FireMissile();

        if (_ammoDepleted == false)
        {
            FireLaser();
        }
        _powerups = GameObject.FindGameObjectsWithTag("Powerup");
        foreach (GameObject pwrup in _powerups)
        {
            if (pwrup == null)
            {
                return;
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                pwrup.GetComponent<Powerup>().StartCollect();
            }
        }

    }
    void Movement()
    {


        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
            

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


            if (Input.GetKey(KeyCode.LeftShift))
            {
                _uiManager.ThrustEnabled();
                transform.Translate(direction * _speed * Time.deltaTime);
            }
            else 
            {
                transform.Translate(direction * _speed * Time.deltaTime);

            }
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
                else if (_tsunamiShotActive == true)

                {
                    Instantiate(_tsunamiShotPrefab, transform.position + offset, Quaternion.identity);
                }
            else
            {
                Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
            }




                _audioSource.Play();
            ammo--;
            ReloadAmmo(ammo);


              if (ammo <= 0)
            {
                _ammoDepleted = true;
            }

        }

    }

    /*void FireMissile()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(_missilePrefab, transform.position, Quaternion.identity);
        }
    }*/

    public void Damage()
    {
        if (_shieldBoostActive == true)
        {
            _shieldCharge--;
            switch(_shieldCharge)
            {
                case 2: //2 charges
                    _Shield.GetComponent<SpriteRenderer>().material.color = Color.yellow;
                    break;
                case 1: //1 charge left
                    _Shield.GetComponent<SpriteRenderer>().material.color = Color.red;
                    break;
                case 0: //no charges left
                    _shieldBoostActive = false;
                    _Shield.SetActive(false);
                    break;
            }
            return;
                
        }


        else 
        {
            _shieldCharge--;
        }
        
        _lives--;
       
        StartCoroutine(CameraShake());
        
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
            _shieldCharge = 3;
            _Shield.GetComponent<SpriteRenderer>().material.color = Color.white;
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void ReloadAmmo(int ammoCount)

    {
        ammo = ammoCount;
        _uiManager.UpdateAmmo(ammo);
        
    }

    public void AmmoBoost()

    {
        ammo = _maxAmmo;
        _ammoDepleted = false;
    }

    IEnumerator CameraShake()
    {
        Vector3 currentCamPos = Camera.main.transform.position;

        for (int i = 0; i < 25; i++)
        {
            float randomX = Random.Range(-0.5f, 0.5f);
            float randomY = Random.Range(-0.5f, 0.5f);

            Camera.main.transform.position = new Vector3(randomX, randomY, currentCamPos.z);

            yield return null;
        }

        Camera.main.transform.position = currentCamPos;
    }

    public void LifeIncrease()
    {
        if(_lives <= 2 && _lives != 0)
        {
            _lives ++;
            _uiManager.UpdateLives(_lives);
            HealthRepair();
        }

    }
    
    public void HealthRepair()
    {
        switch (_lives)
        {
            case 3:
                _leftThruster.SetActive(false);
                break;
            case 2:
                _leftThruster.SetActive(true);
                _rightThruster.SetActive(false);
                break;
            case 1:
                _rightThruster.SetActive(true);
                break;
        }
    }

    public void Thrusters(bool ActiveThrusters)
    {
        switch(ActiveThrusters)
        {
            case true:
                _speed = _thrustSpeed;
                //_thrustersActive = true;
                break;
            case false:
                _speed = _regularSpeed;
               // _thrustersActive = false;
                break;
        }
    }

    public void TsunamiShot()
    {
        _tsunamiShotActive = true;
        StartCoroutine(TsunamiShotRoutine());
    }

    IEnumerator TsunamiShotRoutine()
    {
        yield return new WaitForSeconds(5f);
        _tsunamiShotActive = false;
    }

    public void ShieldDrain()
    {
        _shieldMagActive = true;
        if (_shieldMagActive == true)
        {
            _shieldBoostActive = false;
            _shieldCharge = 0;
            _Shield.SetActive(false);
            StartCoroutine(MagDrainRoutine());
        }
    }

    IEnumerator MagDrainRoutine()
    {
        yield return new WaitForSeconds(1f);
        _shieldMagActive = false;
    }


}
