using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ServerData", order = 0)]
public class SO_ServerData : ScriptableObject
{
	public ServerData[] serverData;
	public DatabaseData[] databaseData;
	public FirewallData[] firewallData;
	public PirateData[] pirateData;
}
