using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
    
{
    AudioSource _audiosource;
    // Start is called before the first frame update
    void Start()
    {
        if (_audiosource == null)
        {
            Debug.LogError("Audio source on explosion is NULL");
        }

        _audiosource = GetComponent<AudioSource>();
        _audiosource.Play();
        Destroy(this.gameObject, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
