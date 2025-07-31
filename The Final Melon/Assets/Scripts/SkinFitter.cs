using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinFitter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null) return;

        // ȭ�� ũ�� ���
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Screen.width / Screen.height;

        // ��������Ʈ ���� ������
        Vector2 spriteSize = sr.sprite.bounds.size;

        // ������ ��� (ȭ�� ä���� -> �� ū �ʿ� ����)
        float scale = Mathf.Max(screenWidth / spriteSize.x, screenHeight / spriteSize.y);

        transform.localScale = new Vector3(scale, scale, 1f);
	}

}
