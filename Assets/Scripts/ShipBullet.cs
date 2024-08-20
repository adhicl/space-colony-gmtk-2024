using System.Collections;
using UnityEngine;
using Zenject.Asteroids;

public class ShipBullet : MonoBehaviour
{
	public EnemyShip target;
	public float dmg;

	private float angle;

	private float timeKill = 0f;

	private void Start()
	{
		StartAim(1, target);
	}

	public void StartAim(float damage, EnemyShip iTarget)
	{
		this.target = iTarget;
		dmg = damage;

		Vector3 diffTransform = iTarget.transform.position - this.transform.position;
		angle = Mathf.Atan2(diffTransform.y, diffTransform.x) ;

		this.transform.rotation = Quaternion.Euler(0, 0, (angle * Mathf.Rad2Deg) - 90f);
	}

	// Update is called once per frame
	void Update()
	{
		float distance = Time.deltaTime * 2f;
		this.transform.position = this.transform.position + 
			new Vector3(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle));

		timeKill += Time.deltaTime;
		if (timeKill >= 5f)
		{
			DestroyMe();
		}
	}

	public void DestroyMe()
	{
		Destroy(this.gameObject);
	}
}