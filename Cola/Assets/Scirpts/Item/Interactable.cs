using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // 전방에 이 오브젝트가 보일 때 UI에 표시될 텍스트
    public string interactionPrompt = "상호작용 (E)";

    // 상호작용 함수 (자식 클래스에서 내용을 채워야 함)
    public abstract void Interact(PlayerInteraction player);
}

