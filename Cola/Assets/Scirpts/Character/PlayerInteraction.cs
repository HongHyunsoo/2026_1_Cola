using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro를 사용하기 위해 추가

public class PlayerInteraction : MonoBehaviour
{
    [Header("상호작용 설정")]
    public float interactionDistance = 3f; // 상호작용 가능한 거리
    public LayerMask interactionLayer;     // 상호작용할 레이어
    private Camera playerCamera;

    [Header("콜라 (연료) 설정")]
    public float maxFuel = 99999f; // 최대 연료량
    public float currentFuel = 0f;
    public Image fuelGaugeImage; // 연료 게이지 UI 이미지

    [Header("인벤토리 설정")]
    public GameObject[] inventorySlotsUI; // 인벤토리 슬롯 UI (배경)
    public Image[] itemIconsUI;      // 아이템 아이콘을 표시할 UI
    public Sprite emptySlotIcon;     // 빈 슬롯일 때 표시할 기본 이미지
    private string[] inventory = new string[3]; // 아이템 이름을 저장할 배열
    private int selectedSlot = 0;

    [Header("UI & 텍스트")]
    public TextMeshProUGUI interactionText; // "E키로 상호작용" 텍스트

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

    // 전방에 상호작용 가능한 물체가 있는지 확인
    void CheckForInteractable()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance, interactionLayer))
        {
            interactionText.gameObject.SetActive(true);
            interactionText.text = hit.collider.GetComponent<Interactable>().interactionPrompt;

            // 상호작용 키 입력 확인
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

    // 인벤토리 관련 키 입력 처리 (마우스 스크롤)
    void HandleInventoryInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            if (scroll > 0) // 위로 스크롤
            {
                selectedSlot--;
                if (selectedSlot < 0) selectedSlot = inventory.Length - 1;
            }
            else // 아래로 스크롤
            {
                selectedSlot++;
                if (selectedSlot > inventory.Length - 1) selectedSlot = 0;
            }
            UpdateInventoryUI();
        }
    }

    // 콜라(연료) 추가 함수
    public void AddFuel(float amount)
    {
        currentFuel += amount;
        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
        UpdateFuelGauge();
        Debug.Log("연료 획득! 현재 연료: " + currentFuel);
    }

    // 아이템 추가 함수
    public bool AddItem(string itemName, Sprite icon)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (string.IsNullOrEmpty(inventory[i])) // 빈 슬롯을 찾았다면
            {
                inventory[i] = itemName;
                itemIconsUI[i].sprite = icon;
                Debug.Log(itemName + " 아이템 획득!");
                return true; // 획득 성공
            }
        }
        Debug.Log("인벤토리가 가득 찼습니다!");
        return false; // 획득 실패
    }

    // UI 업데이트
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
            // 선택된 슬롯 하이라이트 효과
            inventorySlotsUI[i].transform.localScale = (i == selectedSlot) ? new Vector3(1.1f, 1.1f, 1.1f) : Vector3.one;

            // 아이콘 업데이트
            if (string.IsNullOrEmpty(inventory[i]))
            {
                itemIconsUI[i].sprite = emptySlotIcon;
            }
        }
    }

    }