using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    private int _shotType;
    private Player _player;

    // Update is called once per frame
    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }
    void Update()
    {
        switch(_shotType)
        {
            case 0:
                MoveDown();
                break;
            case 1:
                MoveUp();
                break;
        }

    }

    public void MoveUp()
    {
        
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
            if (transform.position.y > 8f)
            {
                if (transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }

                Destroy(this.gameObject);
            }
          
        
    }

    public void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void RegShot()
    {
        _shotType = 0;
    }
    
    public void BackShot()
    {
        _shotType = 1;
    }

   

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            Destroy(this.gameObject);
        }

        if (other.tag == "Powerup")
        {
            other.GetComponent<Powerup>().HitByEnemy();
            Destroy(gameObject);
        }

    }
    
}
