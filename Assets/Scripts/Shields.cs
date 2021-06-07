using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shields : MonoBehaviour
{
    private int _charges = 3;
    private int _maxCharge;
    private SpriteRenderer _shieldRenderer;
    private Color _full, _twoLeft, _oneleft;

    // Start is called before the first frame update
    void Start()
    {
        _maxCharge = _charges;
        _shieldRenderer = GetComponent<SpriteRenderer>();
        _full = _shieldRenderer.color;
        _twoLeft = Color.yellow;
        _oneleft = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
