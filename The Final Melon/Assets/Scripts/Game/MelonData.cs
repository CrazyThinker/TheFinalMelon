using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MelonData
{
	public int melonNumber;
	public Vector3 position;
	public Vector3 velocity;
	public float rotation;
}
[System.Serializable]
public class MelonDataListWrapper
{
	public List<MelonData> list;
	public int cursorMelon, nextMelon;
	public int score;
}

public class MelonManager
{
	// 저장
	public static string SaveMelons(GameManager gm)
	{
		Melon[] allMelons = GameObject.FindObjectsOfType<Melon>();
		List<MelonData> melonList = new List<MelonData>();

		foreach (Melon melon in allMelons)
		{
			if (melon == gm.GetCursorMelon()) continue;
			Rigidbody2D rb = melon.GetComponent<Rigidbody2D>();

			MelonData data = new MelonData
			{
				melonNumber = melon.melonNumber,
				position = melon.transform.position,
				velocity = rb.velocity,
				rotation = melon.transform.eulerAngles.z
			};

			melonList.Add(data);
		}

		if (melonList.Count == 0) return "";
		string json = JsonUtility.ToJson(new MelonDataListWrapper { list = melonList, cursorMelon = gm.GetCursorMelon().melonNumber, nextMelon = gm.GetNextMelonNumber(), score = gm.GetScore() });

		return json;
	}

	// 로드
	public static bool LoadMelons(GameManager gm, string json)
	{
		if (!string.IsNullOrEmpty(json))
		{
			var wrapper = JsonUtility.FromJson<MelonDataListWrapper>(json);

			foreach (MelonData data in wrapper.list)
			{
				Melon newMelon = gm.NewMelon(data.melonNumber, data.position);
				newMelon.transform.eulerAngles = new Vector3(0, 0, data.rotation);
				Rigidbody2D rb = newMelon.GetComponent<Rigidbody2D>();
				rb.velocity = data.velocity;

				gm.DropMelon(newMelon);
			}

			gm.SetCursorMelon(gm.NewMelon(wrapper.cursorMelon, Vector3.zero));
			gm.SetNextMelonNumber(wrapper.nextMelon);
			gm.SetScore(wrapper.score);

			gm.ShowNextMelon();
			gm.ShowScore();

			return true;
		}
		else
		{
			return false;
		}
	}
}