using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class DatabaseNode : EntityNode
{
	[SerializeField] private TextMeshProUGUI m_Cost;
	[SerializeField] private TextMeshProUGUI m_Level;

	private oServer parentServer;

	private Vector3 stayMove;

	public void Activate(oServer iServer)
	{
		parentServer = iServer;
		SetServer();

		int level = parentServer.mLevelCore;
		float initialAngle = Random.Range(0, 360);
		float distance = Random.Range(0.15f, 0.6f) + (level * 0.35f);
		float x = distance * Mathf.Cos(Mathf.Deg2Rad * initialAngle);
		float y = distance * Mathf.Sin(Mathf.Deg2Rad * initialAngle);
		stayMove = new Vector3(x, y, 0) + parentServer.transform.position;
		targetMove = stayMove;
	}

	private float moveTimer = 0f;
	private Vector3 targetMove;
	private void Update()
	{
		if (moveTimer < 1f)
		{
			moveTimer += Time.deltaTime * 0.005f;
			this.transform.position = Vector3.Lerp(this.transform.position, targetMove, moveTimer);
		}
	}

	public override void Upgrade()
	{
		base.Upgrade();
		SetServer();
	}

	private void SetServer()
	{
		mLevel = parentServer.mLevelDatabase;

		if (mLevel < 1) mLevel = 1;

		DatabaseData nData = MainScene.Instance.mData.databaseData[mLevel - 1];
		mCostPerTick = nData.costPerTick;

		m_Level.text = "Lv." + mLevel.ToString();
		m_Cost.text = "$" + Mathf.Round(mCostPerTick * -1).ToString();
	}
}
