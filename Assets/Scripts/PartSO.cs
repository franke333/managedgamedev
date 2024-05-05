using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Part", menuName = "Part", order = 1)]
public class PartSO : ScriptableObject
{
    //unique identifier
    public string _name;

    // SHOP STATS
    public string description;
    public int price = 1;
    public int rarity = 1;

    public char ID;


    public int _maxHealth = 1;
    
    public Sprite sprite;

    //TODO enumerate shape

    [SerializeField]
    public bool[] shape = new bool[PartController.maxWidth * PartController.maxHeight]
    {
        false, true, false,
        true, true, true,
        false, true, false
    };
}
