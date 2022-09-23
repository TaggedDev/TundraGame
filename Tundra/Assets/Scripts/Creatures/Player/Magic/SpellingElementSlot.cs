﻿using System;
using UnityEngine;

namespace Creatures.Player.Magic
{
    [Serializable]
    public class MagicElementSlot
    {
        [NonSerialized]
        private float _reloadCooldown;
        [SerializeField]
        private MagicElement element;
        [SerializeField]
        private int maxStoneAmount;
        [SerializeField]
        private int currentStonesAmount;
        [SerializeField]
        private float stoneReloadTime;

        public MagicElement Element { get => element; set => element=value; }

        public int MaxStoneAmount { get => maxStoneAmount; set => maxStoneAmount=value; }

        public int CurrentStonesAmount { get => currentStonesAmount; set => currentStonesAmount=value; }

        public float StoneReloadTime { get => stoneReloadTime; set => stoneReloadTime=value; }

        public float ReloadProgress => (StoneReloadTime - _reloadCooldown) / StoneReloadTime;

        public MagicElementSlot(MagicElement element, int maxAmount, float reloadTime)
        {
            Element = element;
            CurrentStonesAmount = MaxStoneAmount = maxAmount;
            StoneReloadTime = reloadTime;
        }

        public MagicElementSlot()
        {

        }

        public void UpdateReloadState(float deltaTime)
        {
            if (CurrentStonesAmount < MaxStoneAmount)
            {
                _reloadCooldown -= deltaTime;
                if (_reloadCooldown <= 0)
                {
                    CurrentStonesAmount++;
                    _reloadCooldown = StoneReloadTime;
                }
            }
        }
    }

}
