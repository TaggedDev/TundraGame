using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotsChanger : MonoBehaviour
{
    private int slotId = 1;

    public float mouseScrollCoefficient = 10f;

    public int SelectedInventorySlot
    {
        get
        {
            return slotId;
        }
        set
        {
            GameObject slot = GameObject.Find("InventorySlot" + slotId);
            SetSlotActive(slot, false);
            if (value < 1) value = 1;
            if (value > 9) value = 9;
            slotId = value;
            slot = GameObject.Find("InventorySlot" + slotId);
            SetSlotActive(slot, true);
        }
    }

    private void SetSlotActive(GameObject slot, bool state)
    {
        slot.transform.Find("SelectedSlot").gameObject.SetActive(state);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectedInventorySlot = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectedInventorySlot = 2; 
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectedInventorySlot = 3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectedInventorySlot = 4; 
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectedInventorySlot = 5;
        if (Input.GetKeyDown(KeyCode.Alpha6)) SelectedInventorySlot = 6; 
        if (Input.GetKeyDown(KeyCode.Alpha7)) SelectedInventorySlot = 7; 
        if (Input.GetKeyDown(KeyCode.Alpha8)) SelectedInventorySlot = 8; 
        if (Input.GetKeyDown(KeyCode.Alpha9)) SelectedInventorySlot = 9;
        float wheel = Input.GetAxis("Mouse ScrollWheel") * mouseScrollCoefficient;
        if (SelectedInventorySlot + wheel > 9) SelectedInventorySlot = 1;
        else if (SelectedInventorySlot + wheel < 1) SelectedInventorySlot = 9;
        else SelectedInventorySlot += (int)Mathf.Round(wheel);
    }
}
