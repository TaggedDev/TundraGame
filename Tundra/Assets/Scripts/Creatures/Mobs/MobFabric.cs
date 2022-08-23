using System.Collections.Generic;
using Creatures.Mobs.Fox;
using UnityEngine;

namespace Creatures.Mobs
{
    public class MobFabric : MonoBehaviour
    {
        [SerializeField] private Mob[] _mobsList;
        private Queue<Mob> _mobsPool;

        private void Start()
        {
            _mobsPool = new Queue<Mob>();
            InstantiateMobs();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
                SpawnNextMob();
        }

        private void InstantiateMobs()
        {
            foreach (var mobObject in _mobsList)
            {
                Mob mob = Instantiate(mobObject);
                mob.transform.gameObject.SetActive(false);
                _mobsPool.Enqueue(mob);
            }
        }
        
        private void SpawnNextMob()
        {
            if (_mobsPool.Count == 0)
                return;
            
            var mob = _mobsPool.Dequeue();
            mob.gameObject.SetActive(true);
        }
        
        
    }
}