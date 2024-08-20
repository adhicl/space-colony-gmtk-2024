using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TItleScene : MonoBehaviour
{
	[SerializeField] AudioClip sfxButton;

	public void StartPlay()
	{
		this.GetComponent<AudioSource>().PlayOneShot(sfxButton);
		SceneManager.LoadScene("MainScene");
	}
}
