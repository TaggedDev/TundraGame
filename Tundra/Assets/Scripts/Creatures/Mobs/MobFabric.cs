using System.Collections.Generic;
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

        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.P))
                SpawnNextMob();
        }

        private void InstantiateMobs()
        {
            foreach (var mobObject in _mobsList)
            {
                Mob mob = Instantiate(mobObject, transform);
                mob.transform.gameObject.SetActive(false);
                mob.Initialise(this);
                _mobsPool.Enqueue(mob);
            }
        }
        
        private void SpawnNextMob()
        {
            if (_mobsPool.Count == 0)
                return;
            
            var mob = _mobsPool.Dequeue();
            mob.SpawnSelf();
        }

        public void ReturnToPool(Mob mob)
        {
            _mobsPool.Enqueue(mob);
            mob.gameObject.SetActive(false);
        }
    }
}