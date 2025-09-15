using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndingSceneController : MonoBehaviour
{
    [Header("UI ����")]
    public TextMeshProUGUI finalFuelText;
    public GameObject resultPanel; // ��� UI�� ��� �ִ� �г�

    [Header("�÷��̾� ����")]
    public GameObject playerObject; // ���� ������ ������ �÷��̾�

    void Start()
    {
        // ��� UI�� ��� �����, �÷��̾� ������ Ȱ��ȭ
        resultPanel.SetActive(false);
        playerObject.SetActive(true);

        // Ŀ���� �ٽ� ��� (�÷��̾ �������� �ϹǷ�)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // ���⿡ �����̳� �ܰ��ΰ��� ��ȣ�ۿ� ������ �߰��� �� �ֽ��ϴ�.
        // ���� ���, Ư�� ������Ʈ�� �ٰ��� ��ȣ�ۿ��ϸ� ���� ����� �����ִ� ���
        // ������ �����ϰ� 3�� �Ŀ� ����� ��Ÿ������ �غ��ڽ��ϴ�.
        Invoke("ShowFinalResult", 3f);
    }

    // ���� ����� �����ִ� �Լ�
    void ShowFinalResult()
    {
        // �÷��̾� ������ ��Ȱ��ȭ�ϰ� Ŀ���� ǰ
        playerObject.GetComponent<FirstPersonController>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // ��� �г� Ȱ��ȭ
        resultPanel.SetActive(true);

        // �ı����� �ʰ� �Ѿ�� GameManager �ν��Ͻ��� ã���ϴ�.
        if (GameManager.instance != null)
        {
            // GameManager���� ���� ���� ���� ������ UI�� ǥ���մϴ�.
            float finalFuel = GameManager.instance.currentFuel;
            finalFuelText.text = "���� ���� ����: " + finalFuel.ToString("F2") + " L"; // F2�� �Ҽ��� ��° �ڸ����� ǥ��
        }
        else
        {
            finalFuelText.text = "�����͸� �ҷ����� �� �����߽��ϴ�.";
        }
    }
}
