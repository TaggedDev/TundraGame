using System.Collections;
using Environment;
using Player.Behaviour;
using UnityEngine;

namespace System
{
    public class LoadOrder : MonoBehaviour
    {
        [SerializeField] private MapGenerator mapGenerator;
        [SerializeField] private PlayerSpawner playerHolder;
        private EndlessTerrain _chunks;
        
        /*
         * 1. Terrain generation
         * 2. Surface objects generation
         * 3. Light sources
         * 4. Player spawn
         */
        
        private void Start()
        {
            _chunks = mapGenerator.GetComponent<EndlessTerrain>();
            mapGenerator.gameObject.SetActive(true);
            StartCoroutine(InstantiateWorld());
        }

        private IEnumerator InstantiateWorld()
        {
            yield return new WaitUntil(() => mapGenerator.mapDataCount == 81 && mapGenerator.meshDataCount == 77);
            playerHolder.gameObject.SetActive(true);
            playerHolder.SpawnPlayer(mapGenerator);
        }
    }
}