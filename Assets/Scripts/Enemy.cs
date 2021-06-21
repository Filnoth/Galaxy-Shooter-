﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 0f;
    private float _verticalSpeed = 2f;

    private Player _player;

    private Animator _anim;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _enemyLaser;
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    [SerializeField]
    private GameObject _shields;
    private bool _isShieldsActive = false;
    private int _shieldChance;
    private int _shieldPower;
    private int _moveType;
    private bool _rightMovement = false;
    private int _enemyType;
    private float _detectionRange = 6f;
    private int _ramSpeed = 6;
    [SerializeField]
    private GameObject _backLaser;
    Vector3 offset = new Vector3(0, 3f, 0);



    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }
        _anim = gameObject.GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL.");
        }
        _isShieldsActive = false;
        _shields.SetActive(false);
        ShieldCheck();
        _moveType = Random.Range(0, 2);
        _enemyType = 2;
        Debug.Log(_enemyType);
        Physics.IgnoreLayerCollision(9, 9);

    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
        EnemyType();
    }

    void EnemyMovement()
    {
         switch(_moveType)
         {
             case 0:
                 StraightMovement();
                 break;
             case 1:
                 ZigZagMovement();
                 break;
         }

        if (transform.position.y < -7f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }

    }

    void StraightMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }
    
    void ZigZagMovement()
    {
        if (_rightMovement == true)
        {
            transform.Translate(new Vector3(1, 0, 0) * _speed * Time.deltaTime);
            transform.Translate(new Vector3(0, -1, 0) * _verticalSpeed * Time.deltaTime);
        }

        if (transform.position.x >= 6.5f)
        {
            _rightMovement = false;
        }

        if (_rightMovement == false)
        {
            transform.Translate(new Vector3(-1, 0, 0) * _speed * Time.deltaTime);
            transform.Translate(new Vector3(0, -1, 0) * _verticalSpeed * Time.deltaTime);

            if (transform.position.x <= -6.5f)
            {
                _rightMovement = true;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isShieldsActive == false)
         {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _ramSpeed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.1f);
         }

        if (other.tag == "Player" && _isShieldsActive == true)
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _shieldPower--;
            _shields.SetActive(false);
            StartCoroutine(ShieldChangeDelay());
            return;
        }

        if (other.tag == "Laser" && _isShieldsActive == true)
        {
            Destroy(other.gameObject);
            _shieldPower--;
            _shields.SetActive(false);
            StartCoroutine(ShieldChangeDelay());
            return;
        }

        if (other.tag == "Laser" && _isShieldsActive == false)
        {
            Destroy(other.gameObject);
            _speed = 0;
            _ramSpeed = 0;
            _audioSource.Play();
            _anim.SetTrigger("OnEnemyDeath");
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.1f);
        }

        if (other.tag == "WideLaser")
        {
            _speed = 0;
            _ramSpeed = 0;
            _audioSource.Play();
            _anim.SetTrigger("OnEnemyDeath");
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.1f);
        }

        if (_player != null)
        {
            _player.AddScore(10);

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _ramSpeed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.1f);
        }

        if(other.tag == "Powerup")
        {
            return;
        }

        if (other.tag == "Enemy")
        {
            return;
        }

    }

    private void ShieldIsActive()
    {
        _shieldPower = 1;
        _isShieldsActive = true;
        _shields.SetActive(true);
    }

    private void ShieldCheck()
    {
        _shieldChance = Random.Range(0, 4);
        Debug.Log(_shieldChance);
        if (_shieldChance == 0)
        {
            ShieldIsActive();
        }
    }

    IEnumerator ShieldChangeDelay()
    {
        yield return new WaitForSeconds(0.5f);
        _isShieldsActive = false;
    }

    void EnemyType()
    {
        switch(_enemyType)
        {
            case 0:
                BaseEnemy();
                break;
            case 1:
                RamEnemy();
                break;
            case 2:
                BackwardsEnemy();
                break;
        }
    }

    void BaseEnemy()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(4f, 6f);
            _canFire = Time.time + _fireRate;
            GameObject enemylaser = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
            Laser[] lasers = enemylaser.GetComponentsInChildren<Laser>();
           for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void RamEnemy()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(4f, 6f);
            _canFire = Time.time + _fireRate;
            GameObject enemylaser = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
            Laser[] lasers = enemylaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
        if (Vector3.Distance(_player.transform.position, transform.position) < _detectionRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _ramSpeed * Time.deltaTime);
        }
    }

    void BackwardsEnemy()
    {
        if (transform.position.y < _player.transform.position.y && Time.time > _canFire)
        {
            _fireRate = Random.Range(4f, 6f);
            _canFire = Time.time + _fireRate;
            GameObject enemylaser = Instantiate(_backLaser, transform.position + offset, Quaternion.identity);
            EnemyLaser[] lasers = enemylaser.GetComponentsInChildren<EnemyLaser>();
            

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].BackShot();
            }
            
        }

        else if (Time.time > _canFire)
        {
            _fireRate = Random.Range(4f, 6f);
            _canFire = Time.time + _fireRate;
            GameObject enemylaser = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
            Laser[] lasers = enemylaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }

    }
}
    