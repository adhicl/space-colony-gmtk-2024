using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI t_Health;
	[SerializeField] TextMeshProUGUI t_Level;
	[SerializeField] Transform t_Ship;

	public oServer target;
	public GameObject prefabRedBullet;

	private float angle;

	private float m_Health;
	private float m_MaxHealth;
	private int m_level;
	private float damage;

	// Use this for initialization
	public void InitShip(int lvl, oServer iTarget)
	{
		m_level = lvl;
		target = iTarget;

		PirateData nData = MainScene.Instance.mData.pirateData[m_level - 1];

		m_Health = nData.health;
		m_MaxHealth = nData.health;
		damage = nData.damagePoint;

		Vector3 targetTransform = target.transform.position;
		Vector3 diffPosition = targetTransform - this.transform.position;
		angle = Mathf.Atan2(diffPosition.y, diffPosition.x);

		this.t_Ship.transform.rotation = Quaternion.Euler(0, 0, (angle * Mathf.Rad2Deg) - 90f);

		t_Health.text = Mathf.Round(m_Health / m_MaxHealth).ToString() + "%";
		t_Level.text = "Lv."+m_level.ToString();

		timer = 1f;
	}

	private float timer = 0f;

	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime * 0.3f;
		if (timer >= 1f)
		{
			timer = 0f;
			ShootBullet();
		}

		t_Health.text = Mathf.Round(m_Health / m_MaxHealth * 100f).ToString() + "%";
	}

	private void ShootBullet()
	{
		MainScene.Instance.PlayLaser();
		GameObject nBullet = Instantiate(prefabRedBullet);
		nBullet.transform.position = this.transform.position;
		nBullet.GetComponent<EnemyBullet>().StartAim(damage, target);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "BulletShip")
		{
			ShipBullet blt = collision.gameObject.GetComponent<ShipBullet>();

			this.m_Health -= blt.dmg;
			blt.DestroyMe();

			if (m_Health <= 0f)
			{
				MainScene.Instance.PlayExplosion();
				MainScene.Instance.DestroyEnemyShip(this.gameObject);
			}
		}
	}
}