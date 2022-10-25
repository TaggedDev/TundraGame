using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures.Player.Inventory 
{
    [CreateAssetMenu(menuName = "PlacableObject")]
    public class PlaceableObject : ScriptableObject
    {
        public GameObject Object;
        public GameObject GhostObject;

        private bool _isPlacable;

        [SerializeField] public Material _previewPlacable;
        [SerializeField] public Material _previewUnPlacable;

        public void DiplayGhostObject(Vector3 position)
        {
            GhostObject.transform.position = position;
            GhostObject.transform.rotation =  new Quaternion(0, Quaternion.LookRotation(Camera.main.transform.position - position).y, 0, Quaternion.LookRotation(Camera.main.transform.position - position).w);

        }
    
        public bool IsPlacable
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

                    isOnTheGround &= Physics.OverlapBox(new Vector3(coordsX, GhostObject.transform.position.y, coordsZ), new Vector3(0, 0.05f, 0) / 2).Length == 1;
                }

                _isPlacable = colliders.Length == 1 && isOnTheGround;
                
                return _isPlacable;
            }   
        }
        public void AssignMaterial()
        {
            if (IsPlacable)
                GhostObject.GetComponent<Renderer>().materials = new Material[] { _previewPlacable };
            else
                GhostObject.GetComponent<Renderer>().materials = new Material[] { _previewUnPlacable };
        }

        public bool TryPlacing(Vector3 position, Quaternion rotation)
        {
            if (!IsPlacable)
                return false;
            Instantiate(Object, position, rotation);
            Destroy(GhostObject);
        
            return true;
        }
    }
}