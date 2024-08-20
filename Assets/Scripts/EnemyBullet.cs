using System.Collections;
using UnityEngine;
using Zenject.Asteroids;

public class EnemyBullet : MonoBehaviour
{
	public oServer target;
	public float dmg;

	private float angle;

	private float timeKill = 0f;

	public void StartAim(float damage, oServer iTarget)
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