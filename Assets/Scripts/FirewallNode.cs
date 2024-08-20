using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using static UnityEngine.GraphicsBuffer;

public class FirewallNode : EntityNode
{
	[SerializeField] private TextMeshProUGUI m_Cost;
	[SerializeField] private TextMeshProUGUI m_Level;

	public float mAttack;
	public Transform ship;

	private oServer parentServer;

	private Vector3 stayMove;
	private float initialAngle;

	public void Activate(oServer iServer)
	{
		parentServer = iServer;
		SetServer();

		int level = parentServer.mLevelCore;
		initialAngle = Random.Range(0, 360);
		float distance = (0.8f + (level * 0.25f));
		float x = distance * Mathf.Cos(Mathf.Deg2Rad * initialAngle);
		float y = distance * Mathf.Sin(Mathf.Deg2Rad * initialAngle);
		stayMove = new Vector3(x, y, 0) + parentServer.transform.position;
		targetMove = stayMove;
		ship.transform.rotation = Quaternion.Euler(0, 0, initialAngle - 90f);

	}

	private float shootTimer = 0f;
	private float moveTimer = 0f;
	private Vector3 targetMove;
	private void Update()
	{
		shootTimer += Time.deltaTime * 0.3f;
		if (moveTimer < 1f)
		{
			moveTimer += Time.deltaTime * 0.005f;
			/*
			Vector3 diffMove = targetMove - this.transform.position;
			float angle = Mathf.Atan2(diffMove.y, diffMove.x) * Mathf.Rad2Deg;
			ship.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
			//*/
			this.transform.position = Vector3.Lerp(this.transform.position, targetMove, moveTimer);
		}
		if (shootTimer > 1f)
		{
			shootTimer = 0f;
			if (curTargetShip == null)
			{
				SearchTarget();
			}
			else
			{
				ShootBullet();
			}
		}
	}

	public override void Upgrade()
	{
		base.Upgrade();
		SetServer();
	}

	public void MoveFurther()
	{
		int level = parentServer.mLevelCore;
		float distance = (0.8f + (level * 0.25f));
		float x = distance * Mathf.Cos(Mathf.Deg2Rad * initialAngle);
		float y = distance * Mathf.Sin(Mathf.Deg2Rad * initialAngle);
		stayMove = new Vector3(x, y, 0) + parentServer.transform.position;
		targetMove = stayMove;

		moveTimer = 0f;
	}

	private void SetServer()
	{
		mLevel = MainScene.Instance.curServer.mLevelFirewall;

		if (mLevel < 1) mLevel = 1;

		FirewallData nData = MainScene.Instance.mData.firewallData[mLevel - 1];
		mCostPerTick = nData.costPerTick;
		mAttack = nData.damagePoint;

		m_Level.text = "Lv." + mLevel.ToString();
		m_Cost.text = "$" + Mathf.Round(mCostPerTick).ToString();
	}

	public void SetTargetShip(EnemyShip iShip)
	{
		curTargetShip = iShip;
	}

	protected void SearchTarget()
	{
		if (MainScene.Instance.mEnemy.Count <= 0) return;
		int whichServe = Random.Range(0, MainScene.Instance.mEnemy.Count - 1);		
		if (MainScene.Instance.mEnemy[whichServe])
		{
			curTargetShip = MainScene.Instance.mEnemy[whichServe].GetComponent<EnemyShip>();
			ShootBullet();
		}
		else
		{
			curTargetShip = null;
		}
	}

	public EnemyShip curTargetShip;
	public GameObject prefabRedBullet;
	private void ShootBullet()
	{
		if (curTargetShip != null)
		{
			MainScene.Instance.PlayLaser();
			GameObject nBullet = Instantiate(prefabRedBullet);
			nBullet.transform.position = this.transform.position;
			nBullet.GetComponent<ShipBullet>().StartAim(mAttack, curTargetShip);

			Vector3 targetTransform = curTargetShip.transform.position;
			Vector3 diffPosition = targetTransform - this.transform.position;
			float angle = Mathf.Atan2(diffPosition.y, diffPosition.x);

			this.ship.transform.rotation = Quaternion.Euler(0, 0, (angle * Mathf.Rad2Deg) - 90f);
		}
	}

}
