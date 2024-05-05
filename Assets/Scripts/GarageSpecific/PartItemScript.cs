using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartItemScript : MonoBehaviour
{
    public PartSO partSO;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = partSO.sprite;
    }

    private void OnMouseDown()
    {
        InventoryManager.Instance.draggedItemController.GrabItem(this);
    }

    //TODO tooltip

    //TODO change collider to match the shape
}
