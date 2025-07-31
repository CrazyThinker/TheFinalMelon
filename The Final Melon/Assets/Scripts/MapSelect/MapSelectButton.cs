using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelectButton : MonoBehaviour
{
	public TextMeshProUGUI mapName;
	public GameObject image;
	public GameObject imgLock;
	public GameObject mapText;
	public TextMeshProUGUI unlockText;
	public GameObject clearText;
	public TextMeshProUGUI buttonText;

	public GameObject messagePanel;
	public TextMeshProUGUI messageText;

	// Start is called before the first frame update
	void Start()
	{
		if (image == null || mapText == null || unlockText == null || clearText == null || buttonText == null) return;

		int mapUnlock = 1, mapClear = 0;

		if (mapName.text != "Watermelon Field")
		{
			mapUnlock = PlayerPrefs.GetInt(mapName.text + " unlock", 0);
			mapClear = PlayerPrefs.GetInt(mapName.text + " clear", 0);
		}

		if(mapUnlock == 0)
		{
			image.GetComponent<UnityEngine.UI.Image>().color = new Color(127f / 255f, 127f / 255f, 127f / 255f, 191f / 255f);
			mapText.SetActive(false);
			imgLock.gameObject.SetActive(true);
			unlockText.gameObject.SetActive(true);
			buttonText.text = "UNLOCK";
		}

		if(mapClear == 1)
		{
			clearText.SetActive(true);
		}
	}

	private void UnlockMap() // �� �κ��� �������� �ڵ�
	{
		int piece1, piece2, clear1, clear2;

		switch(mapName.text)
		{
			case "Watermelon Field": // �ر� ���� ����
				break;

			case "Charlie and the Chocolate Drop": // ���� ���� 5��
				piece1 = PlayerPrefs.GetInt("Watermelon Field piece", 0);
				if(piece1 >= 5)
				{
					PlayerPrefs.SetInt("Watermelon Field piece", piece1 - 5);

					PlayerPrefs.SetInt(mapName.text + " unlock", 1);
					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
				}
				else
				{
					messageText.text = "������ ģ����� �Բ� 5���� ��� Ƽ���� ã�Ҿ��.\n���� ������ ������ ���� ���� 5���� �ʿ��ؿ�.\n���� ������ Watermelon Field�� �÷����ϸ�\n100������ 1���� ���� �� �ִ�ϴ�.";
					messagePanel.SetActive(true);
				}
				break;

			case "Candice in Candyland": // ���� ���� 54��
				piece1 = PlayerPrefs.GetInt("Watermelon Field piece", 0);
				if (piece1 >= 54)
				{
					PlayerPrefs.SetInt("Watermelon Field piece", piece1 - 54);

					PlayerPrefs.SetInt(mapName.text + " unlock", 1);
					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
				}
				else
				{
					messageText.text = "�������� ������ ���� 5�� 4���� �����,\n���� ������ ������ ���� ���� 54���� �ʿ��ؿ�.\n���� ������ Watermelon Field�� �÷����ϸ�\n100������ 1���� ���� �� �ִ�ϴ�.";
					messagePanel.SetActive(true);
				}
				break;
				
			case "The Royal Sweetdom": // ���� ���� 10�� + ���� ���� 42�� + ���ڸ� Ŭ���� + ������ Ŭ����
				piece1 = PlayerPrefs.GetInt("Charlie and the Chocolate Drop piece", 0);
				piece2 = PlayerPrefs.GetInt("Candice in Candyland piece", 0);
				clear1 = PlayerPrefs.GetInt("Charlie and the Chocolate Drop clear", 0);
				clear2 = PlayerPrefs.GetInt("Candice in Candyland clear", 0);
				if (piece1 >= 10 && piece2 >= 42 && clear1 == 1 && clear2 == 1)
				{
					PlayerPrefs.SetInt("Charlie and the Chocolate Drop piece", piece1 - 10);
					PlayerPrefs.SetInt("Candice in Candyland piece", piece2 - 42);

					PlayerPrefs.SetInt(mapName.text + " unlock", 1);
					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
				}
				else
				{
					messageText.text = "��ī ���忡�� 10���� Ȳ�� Ƽ���� ������ �ְ�,\n�̻��� ���󿡴� Rule 42��� ��Ģ�� �־��.\\n���� ������ ������ ���ݸ� ���� 10���� ���� ���� 42���� �ʿ��ؿ�.\n���ݸ� ������ Charlie and the Chocolate Drop�� �÷����ϸ�\n1000������ 1���� ���� �� �ְ�,\n���� ������ Candice in Candyland�� �÷����ϸ�\n100������ 1���� ���� �� �ִ�ϴ�.";
					messagePanel.SetActive(true);
				}
				break;
		}
	}

	public void PlayButton()
	{
		if(buttonText.text == "PLAY")
		{
			PlayerPrefs.SetString("mapName", mapName.text);

			SceneManager.LoadScene("GameScene");
		}
		else
		{
			UnlockMap();
		}
	}

	public void PrevButton()
	{
		SceneManager.LoadScene("OpeningScene");
	}

	public void OKButton()
	{
		messagePanel.SetActive(false);
	}

}
