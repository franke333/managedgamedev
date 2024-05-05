using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggedItemController : MonoBehaviour
{
    [SerializeField]
    private PartItemScript _holdingPartItem;
    private Vector3 _grabOffset;
    [SerializeField]
    float _rotationForce = 10f;
    //TODO make the parts throwable
    //TODO grab the part at exact position
    //TODO make the part return to upward rotation

    public void GrabItem(PartItemScript item)
    {
        Debug.Log("GrabItem");
        item.GetComponent<Rigidbody2D>().simulated = false;
        _holdingPartItem = item;
        _grabOffset = _holdingPartItem.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _grabOffset.z = 0;
        //print offset
        Debug.Log("GrabItem offset: " + _grabOffset);
        _holdingPartItem.transform.DORotate(Vector3.zero, 0.5f);
        //DOtween move _grabOffset to middle of the part
        DOTween.To(() => _grabOffset, x => _grabOffset = x, new Vector3(-.75f,.75f,0), 0.5f);
    }

    private void MoveItem()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _holdingPartItem.transform.position = new Vector3(mousePos.x, mousePos.y, 0) + _grabOffset;
    }

    private void Update()
    {
        if (_holdingPartItem == null)
            return;
        MoveItem();
        if (Input.GetMouseButtonUp(0))
        {
            var shipyard = InventoryManager.Instance._shipYard;

            Vector2Int shipPosition = shipyard.WorldToShipPositionClosest(Camera.main.ScreenToWorldPoint(Input.mousePosition) + _grabOffset);
            if (shipyard.InBounds(shipPosition) && shipyard.CanPlacePart(_holdingPartItem.partSO, shipPosition))
            {
                shipyard.PlacePartBuilder(_holdingPartItem.partSO, shipPosition);
                InventoryManager.Instance.RemovePiece(_holdingPartItem);
                Destroy(_holdingPartItem.gameObject);
                _holdingPartItem = null;
                return;
            }
            var rb = _holdingPartItem.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.simulated = true;
            rb.velocity = Vector2.zero;
            _holdingPartItem = null;
        }

    }
}
