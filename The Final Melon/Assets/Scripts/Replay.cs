using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Replay : MonoBehaviour
{
	public TextMeshProUGUI textMessage;

	public void replayYes()
	{
		SceneManager.LoadScene("GameScene");
	}

	public void replayNo()
	{
		UnityEngine.Application.Quit();
	}

	// Start is called before the first frame update
	void Start()
	{
		int score = 0;

		score = PlayerPrefs.GetInt("Score");

		if (textMessage != null) textMessage.SetText("���� ������ " + score + "���Դϴ�.\n�ٽ� �����Ͻðڽ��ϱ�?");
	}
}
