using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class oServer : EntityNode
{
	[SerializeField] public int mLevelCore = 0;
	[SerializeField] public int mLevelDatabase = 0;
	[SerializeField] public int mLevelFirewall = 0;

	[SerializeField] private Button bAdd;

	[SerializeField] public int mMaxChild;

	[SerializeField] private TextMeshProUGUI m_Health;
	[SerializeField] private TextMeshProUGUI m_Child;
	[SerializeField] private TextMeshProUGUI m_Cost;
	[SerializeField] private TextMeshProUGUI m_Level;

	[SerializeField] private SpriteRenderer m_Sprite;
	[SerializeField] private Transform m_Influence;
	[SerializeField] private RectTransform m_Canvas;

	[SerializeField]
	private GameObject prefabDatabase;
	[SerializeField]
	private GameObject prefabFirewall;

	public List<GameObject> m_Children = new List<GameObject>();

	private void Start()
	{
		SetServer();
	}

	private void Update()
	{
		m_Health.text = Mathf.Round(mHealth / mMaxHealth * 100f).ToString() + "%";
	}

	private void SetServer()
	{
		if (mLevel == 0)
		{
			mHealth = 0;
			mMaxHealth = 1;
			m_Sprite.color = Color.black;

			m_Influence.transform.localScale = Vector3.one;
			m_Canvas.sizeDelta = new Vector2(2f, 2f);

			m_Level.text = "Lv." + mLevel.ToString();
			m_Cost.text = "";
			m_Child.text = "";

			bAdd.gameObject.SetActive(false);
			return;
		}
		else
		{
			bAdd.gameObject.SetActive(true);
			m_Sprite.color = Color.white;
		}

		ServerData nData = MainScene.Instance.mData.serverData[mLevel - 1];
		mMaxHealth = nData.maxHealth;
		mHealth = mMaxHealth;
		mMaxChild = nData.maxChildren;
		mCostPerTick = nData.costPerTick;

		m_Influence.transform.localScale = new Vector3(nData.widthInfluence, nData.heightInfluence, 1f);
		m_Canvas.sizeDelta = new Vector2(3f, nData.sizeCanvas);

		m_Level.text = "Lv." + mLevel.ToString();
		m_Cost.text = "$" + Mathf.Round(mCostPerTick).ToString();
		m_Child.text = m_Children.Count.ToString() + "/" + mMaxChild.ToString();
	}

	public void OpenAddNode()
	{
		MainScene.Instance.OpenMenuAdd(this);
	}

	public void OpenUpgrade()
	{
		if (mLevel == 0)
		{
			Upgrade();
		}
		else
		{
			MainScene.Instance.OpenMenuUpgrade(this);
		}
	}

	public void AddNewDatabase()
	{
		MainScene.Instance.PlayBuild();

		GameObject newDatabase = Instantiate(prefabDatabase);
		
		newDatabase.transform.position = this.transform.position;
		newDatabase.GetComponent<DatabaseNode>().Activate(this);

		m_Children.Add(newDatabase);
		MainScene.Instance.mChildren.Add(newDatabase);
		m_Child.text = m_Children.Count.ToString() + "/" + mMaxChild.ToString();
	}

	public void AddNewFirewall()
	{
		MainScene.Instance.PlayBuild();

		GameObject newFirewall = Instantiate(prefabFirewall);

		newFirewall.transform.position = this.transform.position;
		newFirewall.GetComponent<FirewallNode>().Activate(this);

		m_Children.Add(newFirewall);
		MainScene.Instance.mChildren.Add(newFirewall);
		m_Child.text = m_Children.Count.ToString() + "/" + mMaxChild.ToString();
	}

	public override void Upgrade()
	{
		MainScene.Instance.PlayUpgrade();

		mLevel++;
		if (mLevel >= MainScene.Instance.mData.serverData.Length) mLevel = MainScene.Instance.mData.serverData.Length;
		mLevelCore = mLevel;
		SetServer();
		MoveObject();
	}

	protected void MoveObject()
	{
		for ( int i = 0; i < m_Children.Count; i++)
		{
			FirewallNode node = m_Children[i].GetComponent<FirewallNode>();
			if (node != null)
			{
				node.MoveFurther();
			}
		}
	}

	public void UpgradeDatabase()
	{
		MainScene.Instance.PlayUpgrade();

		mLevelDatabase++;
		for (int i = 0; i < m_Children.Count; i++)
		{
			DatabaseNode node = m_Children[i].GetComponent<DatabaseNode>();
			if (node != null)
			{
				node.Upgrade();
			}
		}
	}

	public void UpgradeFirewall()
	{
		MainScene.Instance.PlayUpgrade();

		mLevelFirewall++;
		for (int i = 0; i < m_Children.Count; i++)
		{
			FirewallNode node = m_Children[i].GetComponent<FirewallNode>();
			if (node != null)
			{
				node.Upgrade();
			}
		}
	}

}
