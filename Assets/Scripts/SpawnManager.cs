using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;
    private bool _stopSpawning = false;
    private int _spawnValue;


    // Start is called before the first frame update

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while(_stopSpawning == false)
        {
            _spawnValue = Random.Range(0, 100);
            int _randomPowerUp = Random.Range(0, 7);
            
            switch (_spawnValue < 10 ? "Triple" :
                _spawnValue < 20 ? "Speed" :
                _spawnValue < 35 ? "Shields" :
                _spawnValue < 75 ? "Ammo" :
                _spawnValue < 83 ? "Health" :
                _spawnValue < 90 ? "Tsunami" : 
                _spawnValue < 100 ? "Mag" : "Null")
            {
                case "Triple":
                    _randomPowerUp = 0;
                    break;
                case "Speed":
                    _randomPowerUp = 1;
                    break;
                case "Shields":
                    _randomPowerUp = 2;
                    break;
                case "Ammo":
                    _randomPowerUp = 3;
                    break;
                case "Health":
                    _randomPowerUp = 4;
                    break;
                case "Tsunami":
                    _randomPowerUp = 5;
                    break;
                case "Mag":
                    _randomPowerUp = 6;
                    break;
                case "Null":
                    _randomPowerUp = 7;
                    break;
            }

            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newPowerup = Instantiate(_powerups[_randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

   
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }


}
