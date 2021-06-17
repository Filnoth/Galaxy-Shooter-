using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int _PowerUp;
    [SerializeField]
    private AudioClip _clip;
    private bool _collectionPressed = false;
    private Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5.0f)
        {
            Destroy(this.gameObject);
        }

        if (_collectionPressed)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * 2 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            if (player != null)
            {
                switch (_PowerUp)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoost();
                        break;
                    case 2:
                        player.ShieldsActive();
                        break;
                    case 3:
                        player.AmmoBoost();
                        break;
                    case 4:
                        player.LifeIncrease();
                        break;
                    case 5:
                        player.TsunamiShot();
                        break;
                    case 6:
                        player.ShieldDrain();
                        break;
                }
                   
            }
        }
        {
            Destroy(this.gameObject);
        }
    }

    public void StartCollect()
    {
        _collectionPressed = true;
        StartCoroutine(StopCollect());
    } 

    IEnumerator StopCollect()
    {
        yield return new WaitForSeconds(3f);
        _collectionPressed = false;
    }

} 
