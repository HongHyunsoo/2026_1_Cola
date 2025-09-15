using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI ��Ҹ� ����ϱ� ���� �߰��ؾ� �մϴ�.

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("�÷��̾� ����")]
    public float walkSpeed = 6.0f;
    public float runSpeed = 10.0f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("ī�޶� ����")]
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;

    [Header("���¹̳� ����")]
    public float maxStamina = 100.0f;
    public float staminaDrainRate = 20.0f; // �ʴ� �Ҹ�
    public float staminaRegenRate = 15.0f; // �ʴ� ȸ����
    private float currentStamina;

    [Header("UI ����")]
    public Slider staminaSlider; // ���¹̳� �� UI

    // ���� ����
    private CharacterController characterController;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private float rotationX = 0;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        currentStamina = maxStamina;

        // Ŀ�� ���
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // �����̴� �ʱ� ����
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
            playerVelocity.y = -2f; // ���������� ���� �پ��ֵ���
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // �޸��� ���� ���� Ȯ��
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0;
        float speed = isRunning ? runSpeed : walkSpeed;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move.normalized * speed * Time.deltaTime);

        // ����
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        // �߷� ����
        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    void HandleCameraLook()
    {
        // ���콺 �¿� ���������� �÷��̾� ��ü ȸ��
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * lookSpeed);

        // ���콺 ���� ���������� ī�޶� ȸ��
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
            // �޸��� ���� �� ���¹̳� �Ҹ�
            currentStamina -= staminaDrainRate * Time.deltaTime;
        }
        else
        {
            // ������ �ְų� �Ȱ� ���� �� ���¹̳� ȸ��
            currentStamina += staminaRegenRate * Time.deltaTime;
        }

        // ���¹̳��� �ִ�/�ּҰ��� ���� �ʵ��� ����
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    void UpdateUI()
    {
        // UI �����̴� ������Ʈ
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
        }
    }
}

