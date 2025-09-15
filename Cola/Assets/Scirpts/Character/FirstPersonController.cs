using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 요소를 사용하기 위해 추가해야 합니다.

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("플레이어 설정")]
    public float walkSpeed = 6.0f;
    public float runSpeed = 10.0f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("카메라 설정")]
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;

    [Header("스태미나 설정")]
    public float maxStamina = 100.0f;
    public float staminaDrainRate = 20.0f; // 초당 소모량
    public float staminaRegenRate = 15.0f; // 초당 회복량
    private float currentStamina;

    [Header("UI 설정")]
    public Slider staminaSlider; // 스태미나 바 UI

    // 내부 변수
    private CharacterController characterController;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private float rotationX = 0;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        currentStamina = maxStamina;

        // 커서 잠금
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 슬라이더 초기 설정
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = maxStamina;
        }
    }

    void Update()
    {
        HandleMovement();
        HandleCameraLook();
        HandleStamina();
        UpdateUI();
    }

    void HandleMovement()
    {
        isGrounded = characterController.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f; // 안정적으로 땅에 붙어있도록
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // 달리기 가능 여부 확인
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0;
        float speed = isRunning ? runSpeed : walkSpeed;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move.normalized * speed * Time.deltaTime);

        // 점프
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        // 중력 적용
        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    void HandleCameraLook()
    {
        // 마우스 좌우 움직임으로 플레이어 전체 회전
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * lookSpeed);

        // 마우스 상하 움직임으로 카메라만 회전
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    void HandleStamina()
    {
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = characterController.velocity.magnitude > 0.1f;

        if (isRunning && isMoving)
        {
            // 달리고 있을 때 스태미나 소모
            currentStamina -= staminaDrainRate * Time.deltaTime;
        }
        else
        {
            // 가만히 있거나 걷고 있을 때 스태미나 회복
            currentStamina += staminaRegenRate * Time.deltaTime;
        }

        // 스태미나가 최대/최소값을 넘지 않도록 제한
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    void UpdateUI()
    {
        // UI 슬라이더 업데이트
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
        }
    }
}

