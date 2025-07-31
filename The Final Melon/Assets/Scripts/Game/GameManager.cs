using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public GameObject background;
	public GameObject pauseImage;
	public GameObject cursorLine;
	public TextMeshProUGUI textScore;
	public GameObject piece;
	public TextMeshProUGUI pieceText;
	public GameObject[] fruitList = new GameObject[11 + 1];
	public int highScore = 0;
	public SpriteRenderer nextMelonSprite;
	public GameObject pausePanel;
	public TextMeshProUGUI currentScoreText;
	public TextMeshProUGUI highScoreText;
	public GameObject gameoverPanel;
	public TextMeshProUGUI gameoverText;
	public bool isPause = false;
	public bool ignoreInputThisFrame = false;
	public string mapName;

	private float[] melonSize = new float[] { 0f, 0.173f, 0.267f, 0.360f, 0.397f, 0.511f, 0.612f, 0.644f, 0.861f, 1.028f, 1.363f, 1.417f };
	private int[] melonScore = new int[] { 0, 1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 66 };
	private float by = 3f;
	private Sprite[] melonSprite = new Sprite[11 + 1];
	private GameObject[] melonPrefab = new GameObject[11 + 1];
	private Melon cursorMelon;
	private int cursorState = 0;
	private int nextMelonNumber = 0;
	private float nextMelonTime = 0f;
	private int beforeScore = 0;
	private int score = 0;

	public void SetCursorMelon(Melon melon)
	{
		cursorMelon = melon;
	}
	public Melon GetCursorMelon()
	{
		return cursorMelon;
	}
	public void SetNextMelonNumber(int number)
	{
		nextMelonNumber = number;
	}
	public int GetNextMelonNumber()
	{
		return nextMelonNumber;
	}
	public void SetScore(int Score)
	{
		score = Score;
	}
	public int GetScore()
	{
		return score;
	}

	public void ShowScore()
	{
		textScore.SetText(score.ToString());
	}

	// 다음 멜론 표시
	public void ShowNextMelon()
	{
		if (nextMelonSprite != null)
		{
			nextMelonSprite.sprite = melonSprite[nextMelonNumber];
		}
	}

	// 새 멜론 생성
	public Melon NewMelon(int melonNumber, Vector3 position)
	{
		Melon melon;
		if (melonNumber == 0) // melonNumber이 0이면 새로운 멜론 생성
		{
			melonNumber = nextMelonNumber;
			nextMelonNumber = Random.Range(1, 5);
			ShowNextMelon();
		}
		if (position == Vector3.zero) position = new Vector3(0f, 2.0f, 0f); // position이 Vector3.zero면 가운데에 생성

		GameObject newMelonObject = Instantiate(melonPrefab[melonNumber], position, Quaternion.identity);
		melon = newMelonObject.GetComponent<Melon>();
		melon.SetMelon(this, melonNumber);

		newMelonObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * melonSize[melonNumber];
		newMelonObject.GetComponent<Rigidbody2D>().mass = melonSize[melonNumber];

		PolygonCollider2D oldCollider = newMelonObject.GetComponent<PolygonCollider2D>();
		Destroy(oldCollider);

		PolygonCollider2D newCollider = newMelonObject.AddComponent<PolygonCollider2D>();
		newCollider.isTrigger = false;

		return melon;
	}

	// 멜론 떨어드리기
	public void DropMelon(Melon melon)
	{
		melon.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
		melon.GetComponent<Collider2D>().isTrigger = false;
		melon.EnableCollider(true);
	}

	public void ShowPausePanel(bool Flag)
	{
		pausePanel.SetActive(Flag);
		currentScoreText.text = score.ToString();
		highScoreText.text = highScore.ToString();
	}

	public void PauseGame(bool Flag)
	{
		ShowPausePanel(Flag);
		if (Flag)
		{
			Time.timeScale = 0f;
			ignoreInputThisFrame = Flag;
		}
		else Time.timeScale = 1f;
		isPause = Flag;
	}

	// 멜론 정보 저장
	public void SaveMelons()
	{
		PlayerPrefs.SetString(mapName + " save", MelonManager.SaveMelons(this));
		PlayerPrefs.Save();
	}

	public void ClearMelons()
	{
		PlayerPrefs.DeleteKey(mapName + " save");
	}

	public void GameOver()
	{
		Time.timeScale = 0f;
		gameoverPanel.SetActive(true);
		gameoverText.text = "최종 점수는 " + score + "점입니다.\n" + "다시 시작하시겠습니까?";

		ClearMelons();
	}

	public void AddScore(int melonNumber)
	{
		score += melonScore[melonNumber];
		UpdateScore();
	}

	public void UpdateScore()
	{
		int exchangeScore = 100;

		// 몇 점마다 조각을 얻을지
		switch (mapName)
		{
			case "Watermelon Field":
			case "Candice in Candyland":
			case "The Royal Sweetdom":
				exchangeScore = 100;
				break;
			case "Charlie and the Chocolate Drop":
				exchangeScore = 1000;
				break;
		}

		if(beforeScore / exchangeScore < score / exchangeScore)
		{
			int melonPiece;

			melonPiece = PlayerPrefs.GetInt(mapName + " piece", 0);
			melonPiece += score / exchangeScore - beforeScore / exchangeScore;
			PlayerPrefs.SetInt(mapName + " piece", melonPiece);

			beforeScore = score;

			UpdatePiece();
		}

		if (highScore < score)
		{
			highScore = score;

			PlayerPrefs.SetInt("HighScore", highScore);
		}

		ShowScore();
	}
	public void UpdatePiece()
	{
		pieceText.text = "× " + PlayerPrefs.GetInt(mapName + " piece", 0);
	}

	// 폭탄 터뜨림
	public void ExplodeFinalMelon()
	{
		PlayerPrefs.SetInt(mapName + " clear", 1);
	}

	// 맵 로딩
	public void LoadMap(string mapName)
	{
		int i;
		SpriteRenderer renderer;
		Image img;

		// 배경
		renderer = background.GetComponent<SpriteRenderer>();
		renderer.sprite = Resources.Load<Sprite>("Background/" + mapName);

		// 일시정지 버튼
		img = pauseImage.GetComponent<Image>();
		img.sprite = Resources.Load<Sprite>("Fruit/" + mapName + "/Pause");

		// 글꼴
		textScore.font = Resources.Load<TMP_FontAsset>("Fonts/" + mapName);

		// 조각
		img = piece.GetComponent<Image>();
		img.sprite = Resources.Load<Sprite>("Fruit/" + mapName + "/Piece");
		UpdatePiece();

		for (i = 1; i <= 11; i++)
		{
			// 멜론 종류
			melonSprite[i] = Resources.Load<Sprite>("Fruit/" + mapName + "/" + i);

			// 멜론 프리펩
			melonPrefab[i] = Resources.Load<GameObject>("Prefabs/Melon/Melon " + i);
			renderer = melonPrefab[i].GetComponent<SpriteRenderer>();
			renderer.sprite = Resources.Load<Sprite>("Fruit/" + mapName + "/" + i);

			// 멜론 리스트
			renderer = fruitList[i].GetComponent<SpriteRenderer>();
			renderer.sprite = Resources.Load<Sprite>("Fruit/" + mapName + "/" + i);

		}
	}

	// Start is called before the first frame update
	void Start()
	{
		Time.timeScale = 1f;

		// 맵 불러오기
		highScore = PlayerPrefs.GetInt("HighScore", 0);
		mapName = PlayerPrefs.GetString("mapName", "Watermelon Field");
		LoadMap(mapName);

		if(MelonManager.LoadMelons(this, PlayerPrefs.GetString(mapName + " save", ""))) // 저장 데이터 있음
		{
			cursorState = 1;
			PauseGame(true);
		}
		else // 저장 데이터 없음
		{
			nextMelonNumber = Random.Range(1, 5);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if(!isPause) // 이전 버튼 누르면
		{
			if (Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				PauseGame(true);
				SaveMelons();
			}
		}
		if (isPause) return;
		else if(ignoreInputThisFrame) // 1 프레임 딜레이
		{
			ignoreInputThisFrame = false;
			return;
		}
		if(cursorState == 0)
		{
			cursorMelon = NewMelon(0, Vector3.zero);

			cursorState = 1;
		}
		else if(cursorState == 1)
		{
			if (cursorMelon == null)
			{
				cursorState = 0;
				return;
			}
			bool mousePressed = Input.GetMouseButton(0);
			bool mouseReleased = Input.GetMouseButtonUp(0);

			if (mouseReleased)
			{
				cursorLine.SetActive(false);

				Vector3 mousePos = Input.mousePosition;
				mousePos.z = Camera.main.nearClipPlane;
				Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

				if (worldPos.x < -by / 2.0f - 1.0f || worldPos.x > by / 2.0f + 1.0f || worldPos.y < -2.7f - 1.0f || worldPos.y > 2.3f + 1.0f) return;

				DropMelon(cursorMelon);

				cursorState = 2;
				nextMelonTime = Time.time + 0.5f;
			}
			else if(mousePressed)
			{
				cursorLine.SetActive(true);

				Vector3 mousePos = Input.mousePosition;
				mousePos.z = Camera.main.nearClipPlane;
				Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

				if (worldPos.x < -by / 2.0f - 1.0f || worldPos.x > by / 2.0f + 1.0f || worldPos.y < -2.7f - 1.0f || worldPos.y > 2.3f + 1.0f)
				{
					cursorLine.SetActive(false);
					return;
				}

				Vector3 pos = cursorMelon.transform.position;
				float radius = cursorMelon.transform.localScale.x;
				float size = radius / 2.0f;

				pos.x = Mathf.Clamp(worldPos.x, -by / 2.0f + size + 0.03f, by / 2.0f - size - 0.03f);

				cursorMelon.transform.position = pos;
				cursorLine.transform.position = new Vector3(pos.x, -0.125f, 1f);
			}
		}
		else if(nextMelonTime <= Time.time)
		{
			cursorLine.transform.position = new Vector3(0f, -0.125f, 1f);
			cursorState = 0;
		}
	}

	void OnApplicationPause(bool pauseStatus)
	{
		if(!isPause)
		{
			PauseGame(true);
			SaveMelons();
		}
	}

}
