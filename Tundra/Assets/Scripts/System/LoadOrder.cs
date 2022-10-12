using System.Collections;
using System.Collections.Generic;
using System.Globalization;
//using Creatures.Mobs;
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
        [SerializeField] private EntityRenderer entityRenderer;

        private string[] worldData;
        /*[SerializeField] private MobFabric[] fabrics;*/

        /*
         * 1. Terrain generation
         * 2. Surface objects generation
         * 3. Light sources
         * 4. Player spawn
         */
        
        private void Start()
        {
            worldData = WorldConstants.WorldData.Split('\n');
            mapGenerator.gameObject.SetActive(true);
            chunksGenerator.gameObject.SetActive(true);
            StartCoroutine(InstantiateWorld());
        }

        // Turns on objects following the order of world loading
        private IEnumerator InstantiateWorld()
        {
            yield return new WaitUntil(() => mapGenerator.mapDataCount == 9 && mapGenerator.meshDataCount == 9);
            SetUpPlayer();
            entityRenderer.gameObject.SetActive(true);
            /*foreach (var fabric in fabrics)
                fabric.transform.gameObject.SetActive(true);*/
        }

        private void SetUpPlayer()
        {
            playerHolder.gameObject.SetActive(true);
            
            string savedPosition = worldData[2].Split(':')[1];
            if (savedPosition.StartsWith ("(") && savedPosition.EndsWith (")")) 
            {
                savedPosition = savedPosition.Substring(1, savedPosition.Length-2);
            }
            else
            {
                throw new Exception("Vector3 was in incorrect format");
            }
      
            // split the items
            string[] sArray = savedPosition.Split(',');
      
            // store as a Vector3
            Vector3 result = new Vector3(
                float.Parse(sArray[0], CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(sArray[1], CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(sArray[2], CultureInfo.InvariantCulture.NumberFormat));
            
            playerHolder.transform.position = result;
            playerHolder.SpawnPlayer();
            
        }
    }
}