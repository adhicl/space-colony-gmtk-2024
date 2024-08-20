using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

public class Const
{
	public enum UIType
	{
		None,Learn,Upgrade,Add
	}
}

[Serializable]
public struct ServerData
{
	public float maxHealth;
	public int maxChildren;
	public int costPerTick;
	public int priceUpgrade;
	public float widthInfluence;
	public float heightInfluence;
	public float sizeCanvas;
}

[Serializable]
public struct DatabaseData
{
	public int costPerTick;
	public int priceUpgrade;
}

[Serializable]
public struct FirewallData
{
	public int costPerTick;
	public int priceUpgrade;
	public float damagePoint;
}

[Serializable]
public struct PirateData
{
	public int health;
	public float damagePoint;
}
