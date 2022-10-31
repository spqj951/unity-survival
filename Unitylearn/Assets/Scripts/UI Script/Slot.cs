using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item;//획득한 아이템
    public int itemCount;
    public Image itemImage;

    [SerializeField]
    private Text text_count;

    [SerializeField]
    private GameObject go_CountImage;//이미지 있을 때만 띄우기


    private void SetColor(float _alpha)//투명도 결정하는 알파값 바꾸기
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }
    
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;//형 맞추기

        if(item.itemType !=Item.ItemType.Equipment)
        {

        go_CountImage.SetActive(true);
        text_count.text = itemCount.ToString();

        }
        else
        {
            text_count.text = "0";
            go_CountImage.SetActive(false);
       
        }

        SetColor(1);
    }

    //아이템 개수에 따른 슬롯의 크기 조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_count.text = itemCount.ToString();

        if(itemCount < -0)
        {
            ClearSlot();
        }
    }

    //슬롯 초기화
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_count.text = "0";
        go_CountImage.SetActive(false);
    }
}
