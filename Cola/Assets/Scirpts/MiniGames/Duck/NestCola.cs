using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestCola : Interactable
{
    [Header("설정")]
    [Tooltip("이 콜라가 속한 둥지 관리자")]
    public DuckNest nestManager;
    [Tooltip("이 콜라를 마셨을 때 얻는 연료(L)의 양")]
    public float fuelAmount = 1.5f;

    public override void Interact(PlayerInteraction player)
    {
        GameManager.instance.AddFuel(fuelAmount);

        // 둥지 관리자에게 "나 훔쳐졌어!" 라고 보고합니다.
        if (nestManager != null)
        {
            nestManager.NotifyColaStolen();
        }

        Destroy(gameObject);
    }
}
