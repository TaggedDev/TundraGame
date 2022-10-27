﻿using Creatures.Player.Behaviour;
using GUI.GameplayGUI;
using UnityEngine;

namespace Creatures.Player.States
{
    public class WalkPlayerState : BasicPlayerState
    {

        private const float speed = 1f;

        protected override float StarvingConsumptionCoefficient => 2f;

        protected override float StaminaConsumption => 0f;

        protected override float SpeedCoefficient => speed * (PlayerBehaviour.IsOverweight ? 0.5f : 1f);

        protected override float WarmConsumptionCoefficient => 2f;

        public WalkPlayerState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher,

            PlayerProperties playerProperties, PlayerInventory inventory, Canvas escapeCanvas)
            : base(playerMovement, switcher, playerProperties, inventory, escapeCanvas)
        {
        }

        public override void Start()
        {

            //_playerMovement.Animator.SetFloat("Speed", .5f);
            //PlayerMovement.Speed = PlayerBehaviour.IsOverweight ? 2f : 3f;
        }

        public override void Stop()
        {

        }


        protected override void StaminaIsOver()
        { }

    }
}
