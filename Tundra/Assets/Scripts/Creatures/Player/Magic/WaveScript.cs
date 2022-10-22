using Creatures.Player.Magic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveScript : SpellScript<WaveSpell>
{
    private const float speedModifier = 5;
    private float _startSize;
    private float _lifeTime;
    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _startSize = transform.localScale.x;
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float coefficient = Time.deltaTime;
        _lifeTime += coefficient;
        if (_lifeTime >= Configuration.WaveLength)
        {
            Destroy(gameObject);
        }
        _rb.AddForce(coefficient*speedModifier*transform.forward.normalized);
        transform.localScale = new Vector3(
            (float)(_startSize * _lifeTime * Configuration.WaveStartSize), 
            transform.localScale.y, 
            (float)(_startSize * Math.Log(_lifeTime) * Configuration.WaveStartSize));
    }
}
