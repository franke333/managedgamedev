using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : SingletonClass<InventoryManager>
{
    public ShipBuilderController _shipYard;
    [SerializeField]
    GameObject _itemSpawnPoint, _itemPartPrefab;

    // TODO load from save
    [SerializeField]
    List<PartSO> _partsInInventory;
    List<PartItemScript> _itemPartsInInventory;

    public DraggedItemController draggedItemController;

    [SerializeField]
    float _returnPieceDist = 50f;

    private void SpawnMissingPieces()
    {
        var spawnedItems = _itemPartsInInventory.Select(x => x.partSO).ToList();
        // partsInInventory without _itemPartsInInventory
        Vector3 size = _shipYard.transform.lossyScale;
        int i = 0;

        //copy
        var missingParts = _partsInInventory.ToList();

        foreach (var part in spawnedItems)
        {
            missingParts.Remove(part);
        }

        foreach (var part in missingParts)
        {
            Vector3 position = _itemSpawnPoint.transform.position + Vector3.up * 3f * 1.1f * size.y * i++;
            SpawnPiece(part, position);
        }
    }

    private PartItemScript SpawnPiece(PartSO part, Vector3 pos)
    {
        var item = Instantiate(_itemPartPrefab, pos, Quaternion.identity);
        item.GetComponent<PartItemScript>().partSO = part;
        item.transform.localScale = _shipYard.transform.localScale;
        _itemPartsInInventory.Add(item.GetComponent<PartItemScript>());
        return item.GetComponent<PartItemScript>();
    }

    public void RemovePiece(PartItemScript part)
    {
        _itemPartsInInventory.Remove(part);
        Destroy(part.gameObject);
    }

    public PartItemScript AddPiece(PartSO part)
    {
        _partsInInventory.Add(part);
        return SpawnPiece(part, _itemSpawnPoint.transform.position);
    }

    private void ReturnFallinsPieces()
    {
        foreach (var part in _itemPartsInInventory)
        {
            if(Vector3.Distance(part.transform.position,_itemSpawnPoint.transform.position) > _returnPieceDist)
            {
                part.transform.position = _itemSpawnPoint.transform.position;
                part.GetComponent<Rigidbody2D>().velocity = Vector2.up * Random.Range(-1.0f,3.0f);
            }
        }
    }

    private void Start()
    {
        _itemPartsInInventory = new List<PartItemScript>();
        draggedItemController = GetComponent<DraggedItemController>() ?? transform.AddComponent<DraggedItemController>();
        SpawnMissingPieces();
    }

    private void Update()
    {
        ReturnFallinsPieces();
    }
}
