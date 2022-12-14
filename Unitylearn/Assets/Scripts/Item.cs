using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Item", menuName= "New Item/item")]
public class Item : ScriptableObject
{
    public string itemName;
    [TextArea]//엔터를 칠수 있다.
    public string itemDesc;//아이템 설명
    public ItemType itemType;
    public Sprite itemImage;
    public GameObject itemPrefab;

    public string weaponType;

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }
   
}
