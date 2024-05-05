using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUIScript : MonoBehaviour
{
    [SerializeField]
    Image icon;
    [SerializeField]
    TMP_Text nameText;
    [SerializeField] 
    TMP_Text descriptionText;
    [SerializeField]
    TMP_Text priceText;
    [SerializeField]
    Button buyButton;

    private PartSO _part;


    private void Start()
    {
        buyButton.onClick.AddListener(() =>
        {
            if(_part == null)
            {
                Debug.LogError("Part is null");
                return;
            }
            if(PlayerContentScript.Instance.gold < _part.price)
            {
                Debug.LogError("Not enough gold");
                return;
            }
            PlayerContentScript.Instance.gold -= _part.price;
            InventoryManager.Instance.AddPiece(_part);
            ShopController.Instance.RefreshShopButtons();
        });
    }

    public void SetPart(PartSO part)
    {
        _part = part;
        icon.sprite = part.sprite;
        nameText.text = part._name;

        descriptionText.text = part.description;
        priceText.text = "$: " + part.price.ToString();

        RefreshButton();
    }

    public void RefreshButton()
    {
        buyButton.gameObject.SetActive(_part != null && PlayerContentScript.Instance.gold >= _part.price);
    }

}
