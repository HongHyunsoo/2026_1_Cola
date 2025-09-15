using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : Interactable
{
    public enum ItemType { Cola, InventoryItem }

    [Header("������ ����")]
    public ItemType itemType;

    [Header("�ݶ� Ÿ�� ����")]
    [Tooltip("ItemType�� Cola�� ��쿡�� ���˴ϴ�.")]
    public float fuelAmount = 10f; // �ݶ��̹Ƿ�, ����(fuel)��� �������� �״�� ����ص� �����ϴ�.

    [Header("�κ��丮 Ÿ�� ����")]
    [Tooltip("ItemType�� InventoryItem�� ��쿡�� ���˴ϴ�.")]
    public string itemName;
    public Sprite itemIcon;


    public override void Interact(PlayerInteraction player)
    {
        // �̸��� �ٲ� enum�� ���缭 ���ǹ� ����
        if (itemType == ItemType.Cola)
        {
            // �ݶ� Ÿ���� ���
            GameManager.instance.AddFuel(fuelAmount);
            Debug.Log(fuelAmount);
            Destroy(gameObject); // ȹ�� �� ������Ʈ �ı�
        }
        else if (itemType == ItemType.InventoryItem)
        {
            // �κ��丮 ������ Ÿ���� ���
            bool acquired = player.AddItem(itemName, itemIcon);
            if (acquired) // �κ��丮�� ���������� �߰��Ǿ��� ����
            {
                Destroy(gameObject); // ȹ�� �� ������Ʈ �ı�
            }
        }
    }
}