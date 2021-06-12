﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WideLaser : MonoBehaviour
{
    private float _speed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        transform.localScale += new Vector3 (3, 0, 0) * Time.deltaTime;
        if (transform.position.y > 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }
}
