using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SlotTooltip : MonoBehaviour
{
    [SerializeField]
    private GameObject go_Base;

    [SerializeField]
    private Text text_ItemName;
    [SerializeField]
    private Text text_ItemDesc;
    [SerializeField]
    private Text text_ItemHowtouse;
   
    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        go_Base.SetActive(true);
        go_Base.transform.position = _pos;
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f, -go_Base.GetComponent<RectTransform>().rect.height * 0.5f, 0f);
        text_ItemName.text = _item.itemName;
        text_ItemDesc.text = _item.itemDesc;

        if (_item.itemType == Item.ItemType.Equipment)
        {
            text_ItemHowtouse.text = "우클릭 - 장착";

        }
        else if (_item.itemType == Item.ItemType.Used)
        {
            text_ItemHowtouse.text = "우클릭 - 먹기";
        }
        else
            text_ItemHowtouse.text = "";

    }

    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}
