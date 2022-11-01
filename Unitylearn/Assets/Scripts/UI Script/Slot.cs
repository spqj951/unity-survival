using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;//ȹ���� ������
    public int itemCount;
    public Image itemImage;

    [SerializeField]
    private Text text_count;

    [SerializeField]
    private GameObject go_CountImage;//�̹��� ���� ���� ����

    private SlotTooltip theSlot;

     void Start()
    {
        theSlot = FindObjectOfType<SlotTooltip>();
    }
    
    private void SetColor(float _alpha)//���� �����ϴ� ���İ� �ٲٱ�
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }
    
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;//�� ���߱�

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

    //������ ������ ���� ������ ũ�� ����
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_count.text = itemCount.ToString();

        if(itemCount < -0)
        {
            ClearSlot();
        }
    }

    //���� �ʱ�ȭ
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_count.text = "0";
        go_CountImage.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        theSlot.HideToolTip();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      
        if (item != null)
        {
            theSlot.ShowToolTip(item, transform.position);
        }
    }
}
