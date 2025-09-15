using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // ���濡 �� ������Ʈ�� ���� �� UI�� ǥ�õ� �ؽ�Ʈ
    public string interactionPrompt = "��ȣ�ۿ� (E)";

    // ��ȣ�ۿ� �Լ� (�ڽ� Ŭ�������� ������ ä���� ��)
    public abstract void Interact(PlayerInteraction player);
}

