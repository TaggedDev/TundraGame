using System.Collections;
using Environment;
using Environment.Objects;
using Player.Behaviour;
using UnityEngine;

namespace System
{
    public class LoadOrder : MonoBehaviour
    {
        [SerializeField] private MapGenerator mapGenerator;
        [SerializeField] private PlayerSpawner playerHolder;
        [SerializeField] private ObjectsGenerator objectsGenerator;

        /*
         * 1. Terrain generation
         * 2. Surface objects generation
         * 3. Light sources
         * 4. Player spawn
         */
        
        private void Start()
        {
            mapGenerator.gameObject.SetActive(true);
            StartCoroutine(InstantiateWorld());
        }

        private IEnumerator InstantiateWorld()
        {
            yield return new WaitUntil(() => mapGenerator.mapDataCount == 81 && mapGenerator.meshDataCount == 77);
            playerHolder.gameObject.SetActive(true);
            playerHolder.SpawnPlayer();
            objectsGenerator.gameObject.SetActive(true);
            //StartCoroutine(InstantiateObjects());
        }

        /*private IEnumerator InstantiateObjects()
        {
            
            yield return 
        }*/
    }
}