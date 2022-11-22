using System.Collections;
using System.Globalization;
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
        [SerializeField] private PlayerSpawner playerObject;
        [SerializeField] private EntityRenderer entityRenderer;
        [SerializeField] private Canvas UserInterface;

        private string[] worldData;
        private Rigidbody _playerRigidbody;
        
        /*
         * 1. Terrain generation
         * 2. Surface objects generation
         * 3. Light sources
         * 4. Player spawn
         * 5. UI Initialisation
         */

        /// <summary>
        /// Returns the Game scene to default settings
        /// </summary>
        public void UnloadCurrentScene()
        {
            // Destroy all mobs
            // Destroy all chunks in Nature
            // Reset mapdatacount variables
            // Reset Day/Night
            // Reset player position to 0;150;0

            foreach (Transform child in chunksGenerator.transform)
                Destroy(child.gameObject);

            mapGenerator.mapDataCount = 0;
            mapGenerator.meshDataCount = 0;
            
            entityRenderer.gameObject.SetActive(false);
            mapGenerator.gameObject.SetActive(false);
            chunksGenerator.gameObject.SetActive(false);
            
            _playerRigidbody.useGravity = false;
            //playerHolder.transform.position = new Vector3(0, 150, 0);
        }
        
        private void Start()
        {
            worldData = WorldConstants.WorldData.Split('\n');
            
            _playerRigidbody = playerObject.GetComponent<Rigidbody>();
            _playerRigidbody.useGravity = false;

            playerObject.transform.position = GetPlayerStartPosition();
            
            mapGenerator.gameObject.SetActive(true);
            chunksGenerator.gameObject.SetActive(true);
            StartCoroutine(InstantiateWorld());
        }
        
        /// <summary>
        /// Turns on objects following the order of world loading 
        /// </summary>
        private IEnumerator InstantiateWorld()
        {
            // Wait for world render
            yield return new WaitUntil(() => mapGenerator.mapDataCount == 9 && mapGenerator.meshDataCount == 9);
            
            // Spawn player and set it's parameter
            playerObject.SpawnPlayer();
            _playerRigidbody.useGravity = true;
            
            // Turn on UI
            UserInterface.gameObject.SetActive(true);
            
            // Enable entity generation
            entityRenderer.gameObject.SetActive(true);
            
            // Enable mob fabrics 
            /*foreach (var fabric in fabrics)
                fabric.transform.gameObject.SetActive(true);*/
        }

        /// <summary>
        /// Gets a start position of a player based on save or new world settings
        /// </summary>
        /// <returns>Start position of a player</returns>
        private Vector3 GetPlayerStartPosition()
        {
            Vector3 startPosition = new Vector3(0, 100, 0);
            // If there is any saved data - read XYZ coordinates
            if (!string.IsNullOrEmpty(WorldConstants.WorldData))
                startPosition = GetSavedPlayerCoords();
            // Or just return the default 0;150;0
            return startPosition;
        }
        
        /// <summary>
        /// Spawns player on saved coordinates or at 0;0;0
        /// </summary>
        private Vector3 GetSavedPlayerCoords()
        {
            // Otherwise, read the saved data and set player spawn
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
            
            return result;
        }
    }
}