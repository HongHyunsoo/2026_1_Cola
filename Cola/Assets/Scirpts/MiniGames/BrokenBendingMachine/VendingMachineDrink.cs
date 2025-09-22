using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interactable�� ��ӹ޵��� Ŭ���� ������ �����մϴ�.
public class VendingMachineDrink : Interactable
{
    public enum DrinkType { Cola, NotCola }
    [Tooltip("�� ������� ������ �����ϼ���.")]
    public DrinkType drinkType;

    [Tooltip("�ݶ��� ���, ä���� ����(L)�� ��")]
    public float fuelAmount = 0.25f;

    void Start()
    {
        // 5�� �ڿ� �ڵ����� �ı� (��򰡿� ���� ���� ����)
        Destroy(gameObject, 5f);
    }

    // �÷��̾ �����ϰ� EŰ�� ������ �� �Լ��� ȣ��˴ϴ�.
    public override void Interact(PlayerInteraction player)
    {
        if (drinkType == DrinkType.Cola)
        {
            // �ݶ��� ���� �߰�
            GameManager.instance.AddFuel(fuelAmount);
        }

        // ��ȣ�ۿ뿡 ���������Ƿ� ��� �ı�
        Destroy(gameObject);
    }

    // �ٸ� Collider�� �浹���� �� ȣ��� (�ٴڿ� ������ ������� ���)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            Destroy(gameObject);
        }
    }
}
