using System;
using UnityEngine;

namespace Creatures.Player.Behaviour
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private LayerMask terrainLayer;

        public void SpawnPlayer()
        {
            Debug.DrawRay(transform.position, Vector3.down * 150);
            if (!Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, terrainLayer))
            {
                throw new Exception("Player spawn ray didn't hit the surface. Check if gameobject of terrain has" +
                                    "collider, be sure layermask is selected and attached.");
            }
            transform.position = hit.point;
        }
    }
}