using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro�� ����ϱ� ���� �߰�

public class PlayerInteraction : MonoBehaviour
{
    [Header("��ȣ�ۿ� ����")]
    public float interactionDistance = 3f; // ��ȣ�ۿ� ������ �Ÿ�
    public LayerMask interactionLayer;     // ��ȣ�ۿ��� ���̾�
    private Camera playerCamera;

    [Header("�ݶ� (����) ����")]
    public float maxFuel = 99999f; // �ִ� ���ᷮ
    public float currentFuel = 0f;
    public Image fuelGaugeImage; // ���� ������ UI �̹���

    [Header("�κ��丮 ����")]
    public GameObject[] inventorySlotsUI; // �κ��丮 ���� UI (���)
    public Image[] itemIconsUI;      // ������ �������� ǥ���� UI
    public Sprite emptySlotIcon;     // �� ������ �� ǥ���� �⺻ �̹���
    private string[] inventory = new string[3]; // ������ �̸��� ������ �迭
    private int selectedSlot = 0;

    [Header("UI & �ؽ�Ʈ")]
    public TextMeshProUGUI interactionText; // "EŰ�� ��ȣ�ۿ�" �ؽ�Ʈ

    void Start()
    {
        playerCamera = GetComponent<FirstPersonController>().playerCamera;
        UpdateInventoryUI();
        interactionText.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckForInteractable();
        HandleInventoryInput();
    }

    // ���濡 ��ȣ�ۿ� ������ ��ü�� �ִ��� Ȯ��
    void CheckForInteractable()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance, interactionLayer))
        {
            interactionText.gameObject.SetActive(true);
            interactionText.text = hit.collider.GetComponent<Interactable>().interactionPrompt;

            // ��ȣ�ۿ� Ű �Է� Ȯ��
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                hit.collider.GetComponent<Interactable>().Interact(this);
            }
        }
        else
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    // �κ��丮 ���� Ű �Է� ó�� (���콺 ��ũ��)
    void HandleInventoryInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            if (scroll > 0) // ���� ��ũ��
            {
                selectedSlot--;
                if (selectedSlot < 0) selectedSlot = inventory.Length - 1;
            }
            else // �Ʒ��� ��ũ��
            {
                selectedSlot++;
                if (selectedSlot > inventory.Length - 1) selectedSlot = 0;
            }
            UpdateInventoryUI();
        }
    }

    // �ݶ�(����) �߰� �Լ�
    public void AddFuel(float amount)
    {
        currentFuel += amount;
        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
        UpdateFuelGauge();
        Debug.Log("���� ȹ��! ���� ����: " + currentFuel);
    }

    // ������ �߰� �Լ�
    public bool AddItem(string itemName, Sprite icon)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (string.IsNullOrEmpty(inventory[i])) // �� ������ ã�Ҵٸ�
            {
                inventory[i] = itemName;
                itemIconsUI[i].sprite = icon;
                Debug.Log(itemName + " ������ ȹ��!");
                return true; // ȹ�� ����
            }
        }
        Debug.Log("�κ��丮�� ���� á���ϴ�!");
        return false; // ȹ�� ����
    }

    // UI ������Ʈ
    void UpdateFuelGauge()
    {
        if (fuelGaugeImage != null)
        {
            fuelGaugeImage.fillAmount = currentFuel / maxFuel;
        }
    }

    void UpdateInventoryUI()
    {
        for (int i = 0; i < inventorySlotsUI.Length; i++)
        {
            // ���õ� ���� ���̶���Ʈ ȿ��
            inventorySlotsUI[i].transform.localScale = (i == selectedSlot) ? new Vector3(1.1f, 1.1f, 1.1f) : Vector3.one;

            // ������ ������Ʈ
            if (string.IsNullOrEmpty(inventory[i]))
            {
                itemIconsUI[i].sprite = emptySlotIcon;
            }
        }
    }

    }