using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro�� ����ϹǷ� UI.Text�� �ƴ��� Ȯ���ϼ���.

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("���� �ð� ����")]
    [Tooltip("���� ���� �ð� (24�ð� ����)")]
    public int startTimeInHours = 4;
    [Tooltip("�� ���� ���� �ð� (���� �ð� ����)")]
    public int gameDurationInHours = 12;

    [Header("UI ����")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI warningText; // ��� ������ ǥ���� �ؽ�Ʈ

    // ���� ����
    private float totalRealTimeSeconds; // �� ���� ���� �ð�(��)
    private float currentTime; // ���� ���� ���� �ð�(��)
    private bool warningShown = false; // ��� �� ���� ǥ�õǵ��� �ϴ� �÷���

    [Header("����(�ݶ�) ����")]
    public float maxFuel = 100f; // �ִ뷮�� ������ ���� ���ֵܵ� �����ϴ�.
    public float currentFuel = 0f;
    // ���� �� �κ��� Image���� TextMeshProUGUI�� �����մϴ� ����
    public TextMeshProUGUI fuelText; // ����: public Image fuelGaugeImage;
    // ���� ������ UI�� PlayerInteraction�� �ƴ� GameManager�� ���� �����ؾ� �մϴ�.
    // public Image fuelGaugeImage; <- �� �κ��� PlayerInteraction���� �����ϰ� ���⿡ �����ؾ� �մϴ�.

    [Header("���� ����")]
    private bool isPlayerInSafeArea = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // ���� �ð� 1�� = ���� �ð� 1��
        // �� ���� �ð�(12�ð�) = 12 * 60 = 720 ���� �� = 720 ���� ��
        totalRealTimeSeconds = gameDurationInHours * 60f;
        currentTime = totalRealTimeSeconds;

        if (warningText != null)
        {
            warningText.gameObject.SetActive(false); // ������ �� ��� ���� �����
        }

        UpdateFuelUI(); // ���� ���� �� ���� UI�� �ʱ�ȭ
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            // ���� �ð� 60�� ������ �� ��� ���� ǥ��
            if (currentTime <= 60f && !warningShown)
            {
                ShowWarning();
                warningShown = true;
            }
        }
        else
        {
            currentTime = 0;
            EndGame();
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            // ������ ���� �ð�(��) = ������ ���� �ð�(��)
            float elapsedGameMinutes = totalRealTimeSeconds - currentTime;

            // ���� ���� �ð� ���
            float totalMinutesFromStart = (startTimeInHours * 60) + elapsedGameMinutes;
            int currentHour = Mathf.FloorToInt(totalMinutesFromStart / 60);
            int currentMinute = Mathf.FloorToInt(totalMinutesFromStart % 60);

            timerText.text = string.Format("{0:00}:{1:00}", currentHour, currentMinute);
        }
    }

    void ShowWarning()
    {
        if (warningText != null)
        {
            warningText.gameObject.SetActive(true);
            warningText.text = "���� �߻� 1�� ��! ��ǥ �������� �̵��ϼ���!";
            // ���⿡ �߰��� ���带 ����ϰų�, �ؽ�Ʈ�� �����̴� ȿ���� ���� ���� �ֽ��ϴ�.
        }
    }

    private void EndGame()
    {
        this.enabled = false;
        if (isPlayerInSafeArea)
        {
            SceneManager.LoadScene("EndingScene");
        }
        else
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }

    public void SetPlayerSafeAreaStatus(bool status)
    {
        isPlayerInSafeArea = status;
    }

    public void AddFuel(float amount)
    {
        currentFuel += amount;
        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
        UpdateFuelUI(); // �̸��� �ٲ� �Լ��� ȣ���մϴ�.
    }

    void UpdateFuelUI()
    {
        if (fuelText != null)
        {
            // �Ҽ��� ��° �ڸ����� ǥ���ϰ� �ڿ� " L"�� ���Դϴ�.
            fuelText.text = currentFuel.ToString("F2") + " L";
        }
    }
}
