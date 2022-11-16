using System;
using System.Collections.Generic;
using Creatures.Player.Behaviour;
using Environment;
using GUI.BestiaryGUI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Creatures.Mobs
{
    public class MobFabric : MonoBehaviour
    {
        [SerializeField] private Mob[] mobsList;
        [SerializeField] private PlayerMovement player;
        [SerializeField] private BestiaryPanel bestiaryPanel;
        
        private Queue<Mob> _mobsPool;
        private const int TERRAIN_LAYER_MASK = 10; 

        private void Start()
        {
            _mobsPool = new Queue<Mob>();
            InstantiateMobs();
        }

        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.P))
                SpawnNextMob();
        }

        private void InstantiateMobs()
        {
            foreach (var mobObject in mobsList)
            {
                Mob mob = Instantiate(mobObject, transform);
                mob.transform.gameObject.SetActive(false);
                mob.Initialise(this, player.transform, bestiaryPanel);
                _mobsPool.Enqueue(mob);
            }
        }
        
        private void SpawnNextMob()
        {
            if (_mobsPool.Count == 0)
                return;
            
            var mob = _mobsPool.Dequeue();
            var pos = GetMobSpawnPosition();
            mob.SpawnSelf(pos);
        }

        /// <summary>
        /// Generates the spawn position for mob
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">Called when mob spawn position is too far from rendered world</exception>
        private Vector3 GetMobSpawnPosition()
        {
            float angle = Random.Range(1, 360);
            const float threshold = 3; //square root of sqr entity update threshold divided by 2
            Vector2 position = new Vector2(Mathf.Cos(angle) * threshold, Mathf.Sin(angle) * threshold);

            if (!Physics.Raycast(new Vector3(position.x, 500, position.y), Vector3.down, out RaycastHit info,
                    Mathf.Infinity, 1 << TERRAIN_LAYER_MASK)) 
                throw new Exception($"Unexpected mob spawn in {position.x}; {position.y}");
            
            return new Vector3(position.x, info.point.y, position.y);
        }

        public void ReturnToPool(Mob mob)
        {
            _mobsPool.Enqueue(mob);
            mob.gameObject.SetActive(false);
        }
    }
}