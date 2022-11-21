using Creatures.Player.Behaviour;
using Creatures.Player;
using Creatures.Player.Inventory;
using Creatures.Player.States;
using System.Runtime.ExceptionServices;
using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

namespace Environment.Terrain
{
    /// <summary>
    /// This class describes an entity that is spawned on map.
    /// </summary>
    public class Entity : MonoBehaviour
    {
        [Range(0, 1)] [SerializeField] private float spawnRateForLevel;


        [SerializeField]
        private BasicItemConfiguration[] lootTable;
        [SerializeField]
        private int[] dropQuantity;
        [SerializeField]
        private int[] dropChance;

        public float SpawnRateForLevel => spawnRateForLevel;
        public float HealthPoints
        {
            get
            {
                return _healthPoints;
            }
            set
            {
                _healthPoints = value;
                if (_healthPoints <= 0)
                    Break();
                else
                {
                    //Debug.Log(_originalScale.ToString() + " " + transform.localScale.ToString());
                    StartCoroutine(Shake(0.5f));
                }
                    
            }
        }

        [SerializeField]
        private const float ENTITY_VIEW_RANGE = 3000f;
        [SerializeField]
        private float _healthPoints;
        private Transform _player;
        public Vector3 _originalScale;
        private Vector2 _entityPosition;

        private void Start()
        {
            _originalScale = transform.localScale;

            //Checks LootTable Correctness
            if (lootTable.Length != dropChance.Length || dropChance.Length != dropQuantity.Length)
                throw new System.Exception("Loot Table setup is invalid");
            foreach (int a in dropChance)
            {
                if (a > 100 || a <= 0)
                    throw new System.Exception("Loot Table setup is invalid");
            }
        }

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
            for(int i = 0; i < lootTable.Length; i++)
            {
                int proc = Random.Range(0, 101);
                if(proc <= dropChance[i])
                {
                    Instantiate(lootTable[i].ItemInWorldPrefab, new Vector3(transform.position.x + Random.Range(-2, 2), transform.position.y + Random.Range(1,2), transform.position.z + Random.Range(-2, 2)), new Quaternion(Random.Range(0, 160), Random.Range(0, 160), Random.Range(0, 160), 0)).
                        GetComponent<DroppedItemBehaviour>().DroppedItemsAmount = dropQuantity[i];
                }
            }
        }
        private IEnumerator Shake(float secs)
        {
            float i;
            for ( i = 0; i < 0.03f; i += Time.deltaTime)
            {
                transform.localScale -= 0.03f * transform.localScale;
                yield return null;
            }
            while (transform.localScale.x < _originalScale[0] || transform.localScale.y < _originalScale[1] || transform.localScale.z < _originalScale[2])
            {
                var pos = transform.position;
                pos.y += 1;
                transform.localScale += 0.03f * transform.localScale;
                yield return null;
            }
            
            yield break;

        }
    }
}