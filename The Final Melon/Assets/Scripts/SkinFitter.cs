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

        // 화면 크기 계산
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Screen.width / Screen.height;

        // 스프라이트 원래 사이즈
        Vector2 spriteSize = sr.sprite.bounds.size;

        // 스케일 계산 (화면 채우기용 -> 더 큰 쪽에 맞춰)
        float scale = Mathf.Max(screenWidth / spriteSize.x, screenHeight / spriteSize.y);

        transform.localScale = new Vector3(scale, scale, 1f);
	}

}
