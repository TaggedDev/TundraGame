﻿using Creatures.Player.Inventory;
using System.Runtime.ExceptionServices;
using UnityEngine;

namespace Environment.Terrain
{
    /// <summary>
    /// This class describes an entity that is spawned on map.
    /// </summary>
    public class Entity : MonoBehaviour
    {
        [Range(0, 1)] [SerializeField] private float spawnRateForLevel;


        [SerializeField]
        private BasicItemConfiguration[] LootTable;
        [SerializeField]
        private int[] DropQuantity;
        [SerializeField]
        private int[] DropChance;

        public float SpawnRateForLevel => spawnRateForLevel;
        public float Hp
        {
            get
            {
                return _hp;
            }
            set
            {
                _hp = value;
                if (_hp <= 0)
                    Break();
            }
        }

        [SerializeField]
        private const float ENTITY_VIEW_RANGE = 3000f;
        [SerializeField]
        private float _hp;
        private Transform _player;
        private Vector2 _entityPosition;


        /// <summary>
        /// Due Entities are spawned as Initialise() function, there is no built-in constructor for this method.
        /// Call this function right after prop is initialised.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="player"></param>
        public void Initialise(Vector3 position, Transform player)
        {
            _entityPosition = new Vector2(position.x, position.z);
            _player = player;
            transform.localScale /= WorldConstants.Scale;
            transform.localScale *= Random.Range(0.8f, 1.2f);
            transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            transform.position = position;
            transform.gameObject.layer = 10; // Environment layer 

            //Checks LootTable Correctness
            if (LootTable.Length != DropChance.Length || DropChance.Length != DropQuantity.Length)
                throw new System.Exception("Loot Table setup is invalid");
            foreach(int a in DropChance)
            {
                if (a > 100 || a <= 0)
                    throw new System.Exception("Loot Table setup is invalid");
            }
        }
        

        /// <summary>
        /// Updates current entity's visibility.
        /// </summary>
        public void UpdateSelf()
        {
            Vector2 playerPosition = new Vector2(_player.position.x, _player.position.z);
            if ((playerPosition - _entityPosition).sqrMagnitude <= ENTITY_VIEW_RANGE)
            {
                gameObject.SetActive(true);
                return;
            }
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Breaks an entity. Drops items
        /// </summary>
        public void Break()
        {
            
            MonoBehaviour.Destroy(gameObject);
            for(int i = 0; i < LootTable.Length; i++)
            {
                int proc = Random.Range(0, 101);
                if(proc <= DropChance[i])
                {
                    Instantiate(LootTable[i].ItemInWorldPrefab, new Vector3(transform.position.x + Random.Range(-2, 2), transform.position.y + Random.Range(1,2), transform.position.z + Random.Range(-2, 2)), new Quaternion(Random.Range(0, 160), Random.Range(0, 160), Random.Range(0, 160), 0)).
                        GetComponent<DroppedItemBehaviour>().DroppedItemsAmount = DropQuantity[i];
                }
            }
        }
    }
}