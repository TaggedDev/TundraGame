using Creatures.Player.Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures.Player.States
{

    public class BuildingState : BasicPlayerState
    {
        private const float speed = .7f; //ain't it supposed to be set somwhere in 1 place for everyone?
        public BuildingState(PlayerMovement playerMovement, IPlayerStateSwitcher switcher, PlayerProperties playerProperties) : base(playerMovement, switcher, playerProperties)
        {
            //so?
        }

        protected override float StarvingConsumptionCoefficient => 1f;

        protected override float StaminaConsumption => -.5f; //а за overweight мы стамину не едим? (в Valheim такое было)

        protected override float SpeedCoefficient => speed * (PlayerBehaviour.IsOverweight ? 0.5f : 1f);

        protected override float WarmConsumptionCoefficient => 1f;


        public override void Start()
        {
            Debug.Log("Got Building State");
        }

        public override void Stop()
        {
            Debug.Log("Lost Building State");
        }

        protected override void StaminaIsOver()
        {
            //nothing should happen
            return;
        }
    }
}