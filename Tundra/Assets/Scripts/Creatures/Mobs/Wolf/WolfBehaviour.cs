using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Creatures.Mobs.Wolf
{
    public class WolfBehaviour : Mob, IMobStateSwitcher
    {
        private Transform player;
        private MobBasicState _currentMobState;
        private List<MobBasicState> _allMobStates;

        private void Start()
        {
            _allMobStates = new List<MobBasicState>
            {
                new WolfRoamingState(player, this, this)
            };
            _currentMobState = _allMobStates[0];
        }

        private void FixedUpdate()
        {
            _currentMobState.MoveMob();
        }
        
        public void SwitchState<T>() where T : MobBasicState
        {
            var state = _allMobStates.FirstOrDefault(s => s is T);
            _currentMobState = state;
        }

        public override void Initialise(Transform playerParameter)
        {
            player = playerParameter;
            transform.gameObject.layer = MOB_LAYER_INDEX;
        }
    }
}