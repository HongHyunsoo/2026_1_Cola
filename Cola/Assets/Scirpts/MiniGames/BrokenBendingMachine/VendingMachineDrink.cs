using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interactable을 상속받도록 클래스 선언을 수정합니다.
public class VendingMachineDrink : Interactable
{
    public enum DrinkType { Cola, NotCola }
    [Tooltip("이 음료수의 종류를 선택하세요.")]
    public DrinkType drinkType;

    [Tooltip("콜라일 경우, 채워줄 연료(L)의 양")]
    public float fuelAmount = 0.25f;

    void Start()
    {
        // 5초 뒤에 자동으로 파괴 (어딘가에 끼는 현상 방지)
        Destroy(gameObject, 5f);
    }

    // 플레이어가 조준하고 E키를 누르면 이 함수가 호출됩니다.
    public override void Interact(PlayerInteraction player)
    {
        if (drinkType == DrinkType.Cola)
        {
            // 콜라라면 연료 추가
            GameManager.instance.AddFuel(fuelAmount);
        }

        // 상호작용에 성공했으므로 즉시 파괴
        Destroy(gameObject);
    }

    // 다른 Collider와 충돌했을 때 호출됨 (바닥에 닿으면 사라지는 기능)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            Destroy(gameObject);
        }
    }
}
