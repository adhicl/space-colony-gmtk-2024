using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AddNewMenu : MonoBehaviour
{

	[SerializeField] private Button b_AddDatabase;
	[SerializeField] private Button b_AddFirewall;
	[SerializeField] private TextMeshProUGUI t_ChildLeft;

	private int leftChildren;
	private oServer cServer;

	public void ShowMenu()
	{
		cServer = MainScene.Instance.curServer;
		leftChildren = MainScene.Instance.curServer.mMaxChild - cServer.m_Children.Count;
		t_ChildLeft.text = "left:"+leftChildren.ToString();
	}

	public void AddNewDatabase()
	{
		if (leftChildren <= 0)
		{
			MainScene.Instance.PlayError();
			return;
		}
		MainScene.Instance.curServer.AddNewDatabase();
		this.gameObject.SetActive(false);
	}

	public void AddNewFirewall()
	{
		if (leftChildren <= 0)
		{
			MainScene.Instance.PlayError();
			return;
		}
		MainScene.Instance.curServer.AddNewFirewall();
		this.gameObject.SetActive(false);
	}

	public void CloseMenu()
	{
		MainScene.Instance.PlayButton();
		this.gameObject.SetActive(false);
	}

	
}
