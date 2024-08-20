using System.Collections;
using UnityEngine;

public class ShieldServer : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "BulletEnemy")
		{
			EnemyBullet blt = collision.gameObject.GetComponent<EnemyBullet>();
			MainScene.Instance.DamageShield(blt.dmg);
			blt.DestroyMe();
		}	
	}
}