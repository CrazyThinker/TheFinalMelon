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

	private void UnlockMap() // 이 부분은 수동으로 코딩
	{
		int piece1, piece2, clear1, clear2;

		switch(mapName.text)
		{
			case "Watermelon Field": // 해금 조건 없음
				break;

			case "Charlie and the Chocolate Drop": // 수박 조각 5개
				piece1 = PlayerPrefs.GetInt("Watermelon Field piece", 0);
				if(piece1 >= 5)
				{
					PlayerPrefs.SetInt("Watermelon Field piece", piece1 - 5);

					PlayerPrefs.SetInt(mapName.text + " unlock", 1);
					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
				}
				else
				{
					messageText.text = "찰리는 친구들과 함께 5장의 골든 티켓을 찾았어요.\n다음 맵으로 가려면 수박 조각 5개가 필요해요.\n수박 조각은 Watermelon Field를 플레이하며\n100점마다 1개씩 얻을 수 있답니다.";
					messagePanel.SetActive(true);
				}
				break;

			case "Candice in Candyland": // 수박 조각 54개
				piece1 = PlayerPrefs.GetInt("Watermelon Field piece", 0);
				if (piece1 >= 54)
				{
					PlayerPrefs.SetInt("Watermelon Field piece", piece1 - 54);

					PlayerPrefs.SetInt(mapName.text + " unlock", 1);
					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
				}
				else
				{
					messageText.text = "엘리스가 모험을 떠난 5월 4일을 기념해,\n다음 맵으로 가려면 수박 조각 54개가 필요해요.\n수박 조각은 Watermelon Field를 플레이하며\n100점마다 1개씩 얻을 수 있답니다.";
					messagePanel.SetActive(true);
				}
				break;
				
			case "The Royal Sweetdom": // 초코 조각 10개 + 사탕 조각 42개 + 초코맵 클리어 + 사탕맵 클리어
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
					messageText.text = "웡카 공장에는 10개의 황금 티켓이 숨겨져 있고,\n이상한 나라에는 Rule 42라는 규칙이 있어요.\\n다음 맵으로 가려면 초콜릿 조각 10개와 사탕 조각 42개가 필요해요.\n초콜릿 조각은 Charlie and the Chocolate Drop를 플레이하며\n1000점마다 1개씩 얻을 수 있고,\n사탕 조각은 Candice in Candyland를 플레이하며\n100점마다 1개씩 얻을 수 있답니다.";
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
