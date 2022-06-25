using Environment;
using UnityEngine;

namespace Player.Behaviour
{
    public class PlayerSpawner : MonoBehaviour
    {
        public void SpawnPlayer(MapGenerator mapGenerator)
        {
            float rawHeight = MeshGenerator.GetMapCenter(mapGenerator);
            transform.position = new Vector3(0, rawHeight + 3, 0);
        }
    }
}