using Creatures.Player.Magic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script to control Wave spell.
/// </summary>
public class WaveScript : SpellScript<WaveSpell>
{
    private const float SpeedModifier = 5;
    private float _startSize;
    private float _lifeTime;
    private Rigidbody _rb;

    private void Start()
    {
        _startSize = transform.localScale.x;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float coefficient = Time.deltaTime;
        _lifeTime += coefficient;
        if (_lifeTime >= Configuration.WaveLength)
        {
            Destroy(gameObject);
        }
        _rb.AddForce(coefficient*SpeedModifier*transform.forward.normalized);
        transform.localScale = new Vector3(
            (float)(_startSize * _lifeTime * Configuration.WaveStartSize), 
            transform.localScale.y, 
            (float)(_startSize * Math.Log(_lifeTime) * Configuration.WaveStartSize));
    }
}
