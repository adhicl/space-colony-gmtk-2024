using System.Collections;
using UnityEngine;

public class EntityNode : MonoBehaviour
{
	[SerializeField] protected int mLevel;
	[SerializeField] protected float mHealth;
	[SerializeField] protected float mMaxHealth;
	[SerializeField] protected float mCostPerTick;

	public float PerTick()
	{
		if (mLevel == 0) return 0;
		return mCostPerTick;
	}

	public virtual void Upgrade()
	{
	}
}