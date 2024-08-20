using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LearnMenu : MonoBehaviour
{

	[SerializeField] private Button[] b_LearnCores;
	[SerializeField] private Button[] b_LearnPower;
	[SerializeField] private Button[] b_LearnFirewall;

	[SerializeField] private TextMeshProUGUI[] t_LearnCores;
	[SerializeField] private TextMeshProUGUI[] t_LearnPower;
	[SerializeField] private TextMeshProUGUI[] t_LearnFirewall;

	public void CloseMenu()
	{
		MainScene.Instance.PlayButton();
		this.gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		UpdateUI();
	}

	protected void UpdateUI()
	{
		for (int i = 0; i < b_LearnCores.Length; i++)
		{
			b_LearnCores[i].interactable = false;
		}
		if (MainScene.Instance.maxLevelCore < 5) b_LearnCores[MainScene.Instance.maxLevelCore].interactable = true;

		for (int i = 0; i < b_LearnPower.Length; i++)
		{
			b_LearnPower[i].interactable = false;
		}
		if (MainScene.Instance.maxLevelDatabase < 5) b_LearnPower[MainScene.Instance.maxLevelDatabase].interactable = true;

		for (int i = 0; i < b_LearnFirewall.Length; i++)
		{
			b_LearnFirewall[i].interactable = false;
		}
		if (MainScene.Instance.maxLevelFirewall < 5) b_LearnFirewall[MainScene.Instance.maxLevelFirewall].interactable = true;


		for (int i = 0; i < t_LearnCores.Length; i++)
		{
			if (i < MainScene.Instance.maxLevelCore) t_LearnCores[i].text = "unlocked";
			else t_LearnCores[i].text = "$" + MainScene.Instance.mData.serverData[i].priceUpgrade;
		}
		for (int i = 0; i < t_LearnFirewall.Length; i++)
		{
			if (i < MainScene.Instance.maxLevelFirewall) t_LearnFirewall[i].text = "unlocked";
			else t_LearnFirewall[i].text = "$" + MainScene.Instance.mData.firewallData[i].priceUpgrade;
		}
		for (int i = 0; i < t_LearnPower.Length; i++)
		{
			if (i < MainScene.Instance.maxLevelDatabase) t_LearnPower[i].text = "unlocked";
			else t_LearnPower[i].text = "$" + MainScene.Instance.mData.databaseData[i].priceUpgrade;
		}
	}

	public void UpdateCore(int level)
	{
		//Debug.Log("update core " + level);
		int priceUpgrade = MainScene.Instance.mData.serverData[level - 1].priceUpgrade;
		if (MainScene.Instance.m_CurrentCash < priceUpgrade)
		{
			MainScene.Instance.PlayError();
			return;
		}

		if (MainScene.Instance.maxLevelCore + 1 == level) 
		{
			//can upgrade
			MainScene.Instance.m_CurrentCash -= priceUpgrade;
			MainScene.Instance.PlayUpgrade();
			MainScene.Instance.maxLevelCore++;
			UpdateUI();
		}
	}

	public void UpdatePower(int level)
	{
		//Debug.Log("update power " + level);
		int priceUpgrade = MainScene.Instance.mData.databaseData[level - 1].priceUpgrade;
		if (MainScene.Instance.m_CurrentCash < priceUpgrade) {
			MainScene.Instance.PlayError();
			return;
		}

		if (MainScene.Instance.maxLevelDatabase + 1 == level)
		{
			//can upgrade
			MainScene.Instance.m_CurrentCash -= priceUpgrade;
			MainScene.Instance.PlayUpgrade();
			MainScene.Instance.maxLevelDatabase++;
			UpdateUI();
		}

	}

	public void UpdateFirewall(int level)
	{
		//Debug.Log("update firewall " + level);
		int priceUpgrade = MainScene.Instance.mData.firewallData[level - 1].priceUpgrade;
		if (MainScene.Instance.m_CurrentCash < priceUpgrade)
		{
			MainScene.Instance.PlayError();
			return;
		}

		if (MainScene.Instance.maxLevelFirewall + 1 == level)
		{
			//can upgrade
			MainScene.Instance.m_CurrentCash -= priceUpgrade;
			MainScene.Instance.PlayUpgrade();
			MainScene.Instance.maxLevelFirewall++;
			UpdateUI();
		}

	}

}
