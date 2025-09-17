using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro ���

public class BrokenVendingMachine : Interactable
{
    [Header("�̴ϰ��� ����")]
    [Tooltip("���� ���� �ð� (��)")]
    public float gameDuration = 10f;
    [Tooltip("������� ������ ���� (��)")]
    public float spawnInterval = 0.6f;
    [Tooltip("�̴ϰ����� �� �� �ִ� �ִ� Ƚ��")]
    public int maxUses = 3;
    private int remainingUses; // ���� Ƚ��

    [Header("������Ʈ ����")]
    [Tooltip("����� ���� ����� ������ �迭 (�ݶ�, ���̴� ��)")]
    public GameObject[] drinkPrefabs;
    [Tooltip("������� ������ ��ġ")]
    public Transform spawnPoint;
    [Tooltip("����Ǵ� ���� ����")]
    public float launchForce = 5f;

    [Header("UI ����")]
    [Tooltip("���� �ð��� ǥ���� �ؽ�Ʈ")]
    public TextMeshProUGUI timerText;

    private bool isGameActive = false;

    void Start()
    {
        remainingUses = maxUses; // ������ �� ���� Ƚ���� �ִ� Ƚ���� ����
    }

    // �÷��̾ ��ȣ�ۿ��ϸ� �� �Լ��� ȣ���
    public override void Interact(PlayerInteraction player)
    {
        // ���� ���̰ų�, ���� Ƚ���� ���ų�, ����� �������� �������� �ʾҴٸ� ���� �� ��
        if (isGameActive || remainingUses <= 0 || drinkPrefabs.Length == 0)
        {
            // ���� Ƚ���� ���ٸ� ��ȣ�ۿ� �ؽ�Ʈ�� ����
            if (remainingUses <= 0)
            {
                interactionPrompt = "���� ������ �ʴ´�.";
            }
            return;
        }

        remainingUses--; // Ƚ�� 1 ����

        // �÷��̾� ��Ʈ�ѷ��� �����ͼ� �̴ϰ����� ����
        StartCoroutine(StartMiniGame(player.GetComponent<FirstPersonController>()));
    }

    private IEnumerator StartMiniGame(FirstPersonController playerController)
    {
        isGameActive = true;
        timerText.gameObject.SetActive(true);

        float remainingTime = gameDuration;

        while (remainingTime > 0)
        {
            remainingTime -= spawnInterval;
            timerText.text = "���� �ð�: " + remainingTime.ToString("F1");

            int randomIndex = Random.Range(0, drinkPrefabs.Length);
            GameObject newDrink = Instantiate(drinkPrefabs[randomIndex], spawnPoint.position, Random.rotation);

            Rigidbody rb = newDrink.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 launchDirection = spawnPoint.up + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.3f, 0.3f));
                rb.AddForce(launchDirection.normalized * launchForce, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(spawnInterval);
        }

        // ���� ����
        timerText.gameObject.SetActive(false);
        isGameActive = false;

        // ������ ���� ��, ���� Ƚ���� 0�̸� ��ȣ�ۿ� �ؽ�Ʈ ������Ʈ
        if (remainingUses <= 0)
        {
            interactionPrompt = "������ ���峵��.";
        }
    }
}