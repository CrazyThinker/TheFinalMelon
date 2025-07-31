using System;
using UnityEngine;

public class Melon : MonoBehaviour
{
	public bool destroy = false;

	public int melonNumber = 0;
	private GameManager gameManager;

	public void Explode()
	{
		if (destroy) return;
		Destroy(gameObject);
	}

	public void SetMelon(GameManager gm, int number)
	{
		gameManager = gm;
		melonNumber = number;
	}

	public void EnableCollider(bool mode)
	{
		PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
		collider.enabled = mode;
	}

	// Update is called once per frame
	void Update()
	{
		if(transform.position.y <= -10)
		{
			gameManager.GameOver();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (destroy) return;
		if (collision.gameObject.layer == LayerMask.NameToLayer("Melon" + melonNumber))
		{
			//UnityEngine.Debug.Log(staticNumber + "Collision " + melonNumber + ", collision with " + collision.gameObject.GetComponent<Melon>().staticNumber);
			if (collision.gameObject.GetComponent<Melon>().destroy == true) return;
			collision.gameObject.GetComponent<Melon>().Explode();
			collision.gameObject.GetComponent<Melon>().destroy = true;
			Explode();
			destroy = true;
			gameManager.AddScore(melonNumber);
			if (melonNumber + 1 == 12) // 마지막 수박 터뜨림
			{
				gameManager.ExplodeFinalMelon();
			}
			else // 수박 합성
			{
				gameManager.DropMelon(gameManager.NewMelon(melonNumber + 1, (transform.position + collision.transform.position) / 2));
			}
		}
	}
}
