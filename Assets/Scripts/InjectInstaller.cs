using UnityEngine;
using Zenject;

public class InjectInstaller : MonoInstaller
{

	[SerializeField]
	private MainScene m_Main;

	public override void InstallBindings()
	{
		Container.BindInstance(m_Main).AsSingle();
	}
}