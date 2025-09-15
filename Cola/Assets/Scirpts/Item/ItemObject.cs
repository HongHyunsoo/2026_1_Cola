using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : Interactable
{
    public enum ItemType { Cola, InventoryItem }

    [Header("아이템 설정")]
    public ItemType itemType;

    [Header("콜라 타입 설정")]
    [Tooltip("ItemType이 Cola일 경우에만 사용됩니다.")]
    public float fuelAmount = 10f; // 콜라이므로, 연료(fuel)라는 변수명은 그대로 사용해도 좋습니다.

    [Header("인벤토리 타입 설정")]
    [Tooltip("ItemType이 InventoryItem일 경우에만 사용됩니다.")]
    public string itemName;
    public Sprite itemIcon;


    public override void Interact(PlayerInteraction player)
    {
        // 이름이 바뀐 enum에 맞춰서 조건문 수정
        if (itemType == ItemType.Cola)
        {
            // 콜라 타입일 경우
            GameManager.instance.AddFuel(fuelAmount);
            Debug.Log(fuelAmount);
            Destroy(gameObject); // 획득 후 오브젝트 파괴
        }
        else if (itemType == ItemType.InventoryItem)
        {
            // 인벤토리 아이템 타입일 경우
            bool acquired = player.AddItem(itemName, itemIcon);
            if (acquired) // 인벤토리에 성공적으로 추가되었을 때만
            {
                Destroy(gameObject); // 획득 후 오브젝트 파괴
            }
        }
    }
}