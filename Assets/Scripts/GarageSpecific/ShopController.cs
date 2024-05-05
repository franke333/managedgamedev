using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : SingletonClass<ShopController>
{
    private PartSO[] _shopInventory;
    [SerializeField]
    private ShopItemUIScript[] _shopItemSlots;

    public Sprite EmptySlotIcon;

    private int _sumOfWeights;
    private IReadOnlyList<PartSO> _allParts;

    private void Start()
    {
        _shopInventory = new PartSO[3];
        FillShop();

    }

    private PartSO ChooseRandomItem()
    {
        float randomValue = Random.Range(0, _sumOfWeights);
        foreach (var part in _allParts)
        {
            if (randomValue < part.rarity)
                return part;
            randomValue -= part.rarity;

        }
        Debug.LogError("No part was chosen");
        return null;
    }

    public void FillShop()
    {
        if(_allParts == null)
        {
            _allParts = PartsCollectionManager.Instance.GetParts();
            _sumOfWeights = 0;
            foreach (var part in _allParts)
                _sumOfWeights += part.rarity;
        }
        for (int i = 0; i < 3; i++)
        {
            _shopInventory[i] = ChooseRandomItem();
            _shopItemSlots[i].SetPart(_shopInventory[i]);
        }


    }

    public void RefreshShopButtons()
    {
        foreach (var item in _shopItemSlots)
            item.RefreshButton();
    }
}
