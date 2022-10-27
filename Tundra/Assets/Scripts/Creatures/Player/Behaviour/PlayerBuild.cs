using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures.Player.Inventory;
public class PlayerBuild : MonoBehaviour
{
    public PlaceableObject PlacableObj;

    public delegate void EventHandler(object source, System.EventArgs e);

    public event EventHandler ObjectPlaced;

    private void Start()
    {
        if (PlacableObj.GhostObject == null)
        {
            PlacableObj.GhostObject = Instantiate(PlacableObj.Object);
            PlacableObj.GhostObject.GetComponent<Collider>().enabled = false;
        }
    }

    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, 100))
            return;
        Vector3 point = new Vector3(hit.point.x, hit.point.y + PlacableObj.GhostObject.GetComponent<Renderer>().bounds.extents.y, hit.point.z);
        PlacableObj.DiplayGhostObject(point, new Quaternion(0, Quaternion.LookRotation(transform.position - PlacableObj.GhostObject.transform.position).y, 0, Quaternion.LookRotation(transform.position - PlacableObj.GhostObject.transform.position).w));
        PlacableObj.AssignMaterial();
        
        if (Input.GetMouseButton(0))
        {
           if(PlacableObj.TryPlacing(point, new Quaternion(0, Quaternion.LookRotation(transform.position - point).y, 0, 
               Quaternion.LookRotation(transform.position - point).w)))
            {
                OnObjectPlaced();
                this.enabled = false;
            }
        }
    }

    private void OnDisable()
    {
        Destroy(PlacableObj.GhostObject);
    }

    protected void OnObjectPlaced()
    {
        ObjectPlaced(this, null);
    }
    private void OnEnable()
    {   
        PlacableObj.GhostObject = Instantiate(PlacableObj.Object);
        PlacableObj.GhostObject.GetComponent<Collider>().enabled = false;
    }


}
