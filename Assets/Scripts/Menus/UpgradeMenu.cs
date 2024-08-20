using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{

	[SerializeField] private Button b_AddDatabase;
	[SerializeField] private Button b_AddFirewall;
	[SerializeField] private Button b_AddCore;

	[SerializeField] private TextMeshProUGUI t_CurCoreLevel;
	[SerializeField] private TextMeshProUGUI t_NexCoreLevel;
	[SerializeField] private TextMeshProUGUI t_CurCore;
	[SerializeField] private TextMeshProUGUI t_NexCore;
	[SerializeField] private GameObject tCoreCover;

	[SerializeField] private TextMeshProUGUI t_CurDataLevel;
	[SerializeField] private TextMeshProUGUI t_NexDataLevel;
	[SerializeField] private TextMeshProUGUI t_CurData;
	[SerializeField] private TextMeshProUGUI t_NexData;
	[SerializeField] private GameObject tDataCover;

	[SerializeField] private TextMeshProUGUI t_CurFireLevel;
	[SerializeField] private TextMeshProUGUI t_NexFireLevel;
	[SerializeField] private TextMeshProUGUI t_CurFire;
	[SerializeField] private TextMeshProUGUI t_NexFire;
	[SerializeField] private GameObject tFireCover;

	private oServer cServer;

	public void ShowMenu()
	{
		cServer = MainScene.Instance.curServer;
		UpdateUI();
	}

	protected void UpdateUI()
	{
		SO_ServerData data = MainScene.Instance.mData;

		b_AddCore.interactable = cServer.mLevelCore < 5 && cServer.mLevelCore < MainScene.Instance.maxLevelCore;
		b_AddFirewall.interactable = cServer.mLevelCore < 5 && cServer.mLevelFirewall < MainScene.Instance.maxLevelFirewall;
		b_AddDatabase.interactable = cServer.mLevelCore < 5 && cServer.mLevelDatabase < MainScene.Instance.maxLevelDatabase;

		t_CurCoreLevel.text = "Current Level: "+cServer.mLevelCore.ToString();
		t_CurCore.text = "$" + data.serverData[cServer.mLevelCore - 1].costPerTick.ToString() +
			"\n" + data.serverData[cServer.mLevelCore - 1].maxChildren.ToString();

		if (cServer.mLevelCore == 5)
		{
			tCoreCover.SetActive(true);
		}
		else
		{
			tCoreCover.SetActive(false);

			t_NexCoreLevel.text = "Next Level: " + (cServer.mLevelCore + 1).ToString();
			t_NexCore.text = "$" + data.serverData[cServer.mLevelCore].costPerTick.ToString() +
				"\n" + data.serverData[cServer.mLevelCore].maxChildren.ToString();
		}

		t_CurDataLevel.text = "Current Level: " + cServer.mLevelDatabase.ToString();
		t_CurData.text = "\n$" + (data.databaseData[cServer.mLevelDatabase - 1].costPerTick * -1).ToString();

		if (cServer.mLevelDatabase == 5)
		{
			tDataCover.SetActive(true);
		}
		else
		{
			tDataCover.SetActive(false);

			t_NexDataLevel.text = "Next Level: " + (cServer.mLevelDatabase + 1).ToString();
			t_NexData.text = "\n$" + (data.databaseData[cServer.mLevelDatabase].costPerTick * -1).ToString();
		}

		t_CurFireLevel.text = "Current Level: " + cServer.mLevelFirewall.ToString();
		t_CurFire.text = "$" + (data.firewallData[cServer.mLevelFirewall - 1].costPerTick).ToString()+
			"\n"+ data.firewallData[cServer.mLevelFirewall - 1].damagePoint;

		if (cServer.mLevelFirewall == 5)
		{
			tFireCover.SetActive(true);
		}
		else
		{
			tFireCover.SetActive(false);

			t_NexFireLevel.text = "Next Level: " + (cServer.mLevelFirewall + 1).ToString();
			t_NexFire.text = "$" + (data.firewallData[cServer.mLevelFirewall].costPerTick).ToString() +
			"\n" + data.firewallData[cServer.mLevelFirewall].damagePoint;
		}
	}

	public void UpgradeCore()
	{
		cServer.Upgrade();
		UpdateUI();
		this.gameObject.SetActive(false);
	}

	public void UpgradeDatabase()
	{
		cServer.UpgradeDatabase();
		UpdateUI();
		this.gameObject.SetActive(false);
	}

	public void UpgradeFirewall()
	{
		cServer.UpgradeFirewall();
		UpdateUI();
		this.gameObject.SetActive(false);
	}

	public void CloseMenu()
	{
		MainScene.Instance.PlayButton();
		this.gameObject.SetActive(false);
	}
}
