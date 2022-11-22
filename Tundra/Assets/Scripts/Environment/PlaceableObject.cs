using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures.Player.Inventory
{
    /// <summary>
    /// I guess it's a class of the configuration of the placeable object.
    /// </summary>
    [CreateAssetMenu(menuName = "PlaceableObject")]
    public class PlaceableObject : ScriptableObject
    {
        public GameObject Object;
        public GameObject GhostObject;

        private bool _isPlacable;

        [SerializeField] public Material _previewPlacable;
        [SerializeField] public Material _previewUnPlacable;

        /// <summary>
        /// Displays GhostObject into a provided position and rotation
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="rotation">Rotation.</param>
        public void DisplayGhostObject(Vector3 position, Quaternion rotation)
        {

            GhostObject.transform.position = position;
            GhostObject.transform.rotation = rotation;

        }


        /// <summary>
        /// Indicates if the object is placeable
        /// </summary>
        public bool IsPlaceable
        {
            get
            {
            
                Collider[] colliders = Physics.OverlapBox(GhostObject.transform.position,
                    GhostObject.transform.lossyScale/2, GhostObject.transform.rotation);

                //Lord, forgive me
                bool isOnTheGround = true;

                for(int i = 0; i < 4; i++)
                {
                    /*for square objects that works better
                    float coordsX = i == 0 || i == 3 ? -1 * GhostObject.transform.position.x / 2 : GhostObject.transform.position.x / 2;
                    float coordsZ = i == 0 || i == 1 ? -1 * GhostObject.transform.position.z / 2 : GhostObject.transform.position.z / 2;
                    but for player's convinience we'll use second method of checking*/
                    float coordsX = i == 0 || i == 2 ? GhostObject.transform.position.x :
                        GhostObject.transform.position.x + (2-i)*GhostObject.GetComponent<Renderer>().bounds.size.x / 2 ;
                    float coordsZ = i == 1 || i == 3 ? GhostObject.transform.position.z :
                        GhostObject.transform.position.z + (1 - i) * GhostObject.GetComponent<Renderer>().bounds.size.z / 2;
                    //this way i can check all corners of square object

                    isOnTheGround &= Physics.OverlapBox(new Vector3(coordsX, GhostObject.transform.position.y, coordsZ), new Vector3(0, GhostObject.GetComponent<Renderer>().bounds.size.y + 0.05f, 0) / 2).Length == 1;
                }

                _isPlacable = colliders.Length <= 1 && isOnTheGround;
                
                return _isPlacable;
            }   
        }
        public void AssignMaterial()
        {
            if (IsPlaceable)
                GhostObject.GetComponent<Renderer>().materials = new Material[] { _previewPlacable };
            else
                GhostObject.GetComponent<Renderer>().materials = new Material[] { _previewUnPlacable };
        }

        public bool TryPlacing(Vector3 position, Quaternion rotation)
        {
            if (!IsPlaceable)
                return false;
            Instantiate(Object, position, rotation);
            Destroy(GhostObject);
        
            return true;
        }
    }
}