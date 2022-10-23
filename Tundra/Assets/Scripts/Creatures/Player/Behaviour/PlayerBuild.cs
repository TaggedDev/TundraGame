using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buidling : MonoBehaviour
{
    [SerializeField] public PlaceableObject PlacableObj;

    [SerializeField] private Material _previewPlacable;
    [SerializeField] private Material _previewIsntPlacable;

    
    private void Start()
    {
        PlacableObj.GhostObject = Instantiate(PlacableObj.Object);

        PlacableObj.GhostObject.GetComponent<Collider>().enabled = false;
    }


    void Update()
    {

        //FOR DEBUG
        if (Input.GetKey(KeyCode.D))
            Time.timeScale = 0;
        //FOR DEBUG

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, 100))
            return;

        PlacableObj.DiplayGhostObject(hit.point);
        PlacableObj.AssignMaterial();
        
        if (Input.GetMouseButton(0))
        {
           if(PlacableObj.TryPlacing(hit.point, new Quaternion(0, Quaternion.LookRotation(Camera.main.transform.position - hit.point).y, 0, 
               Quaternion.LookRotation(Camera.main.transform.position - hit.point).w)))
                this.enabled = false;
        }
    }

}
