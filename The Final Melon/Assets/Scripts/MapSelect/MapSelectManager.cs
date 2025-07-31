using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MapSelectManager : MonoBehaviour
{
	// Update is called once per frame
	void Update()
	{
		if (Keyboard.current.escapeKey.wasPressedThisFrame)
		{
			SceneManager.LoadScene("OpeningScene");
		}
	}
}
