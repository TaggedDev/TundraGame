using UnityEngine;

namespace Creatures.Animals.Behaviour
{
    public class AnimalMovement : MonoBehaviour
    {
        [SerializeField] private float searchRadius;
        [SerializeField] private int playerLayerIndex;
        
        private Transform player;

        private void Update()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchRadius, 1 << playerLayerIndex);
            if (hitColliders.Length > 0)
                player = hitColliders[0].transform;
            
            
        }
    }
}
