using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestCola : Interactable
{
    [Header("����")]
    [Tooltip("�� �ݶ� ���� ���� ������")]
    public DuckNest nestManager;
    [Tooltip("�� �ݶ� ������ �� ��� ����(L)�� ��")]
    public float fuelAmount = 1.5f;

    public override void Interact(PlayerInteraction player)
    {
        GameManager.instance.AddFuel(fuelAmount);

        // ���� �����ڿ��� "�� ��������!" ��� �����մϴ�.
        if (nestManager != null)
        {
            nestManager.NotifyColaStolen();
        }

        Destroy(gameObject);
    }
}
