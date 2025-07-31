using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningButton : MonoBehaviour
{
	public void PlayButton()
	{
		SceneManager.LoadScene("MapSelectScene");
	}
	/*
	public void StoreButton()
	{
		SceneManager.LoadScene("StoreScene");
	}*/

}
