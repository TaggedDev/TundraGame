using System.Collections;
using Creatures.Mobs;
using Creatures.Player.Behaviour;
using Environment;
using Environment.Terrain;
using UnityEngine;

namespace System
{
    public class LoadOrder : MonoBehaviour
    {
        [SerializeField] private MapGenerator mapGenerator;
        [SerializeField] private ChunksGenerator chunksGenerator;
        [SerializeField] private PlayerSpawner playerHolder;
        [SerializeField] private EntityGenerator entityGenerator;
        [SerializeField] private MobFabric[] fabrics;

        /*
         * 1. Terrain generation
         * 2. Surface objects generation
         * 3. Light sources
         * 4. Player spawn
         */
        
        private void Start()
        {
            mapGenerator.gameObject.SetActive(true);
            chunksGenerator.gameObject.SetActive(true);
            StartCoroutine(InstantiateWorld());
        }

        private IEnumerator InstantiateWorld()
        {
            yield return new WaitUntil(() => mapGenerator.mapDataCount == 9 && mapGenerator.meshDataCount == 9);
            playerHolder.gameObject.SetActive(true);
            playerHolder.SpawnPlayer();
            entityGenerator.gameObject.SetActive(true);
            foreach (var fabric in fabrics)
                fabric.transform.gameObject.SetActive(true);
        }
    }
}