using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
	#region Serialized Fields
	[SerializeField]
	private GameObject _playerProgressPrefab;
	[SerializeField]
	private GameObject _soundManagerPrefab;
	#endregion

	#region Public Methods
	public override void InstallBindings()
	{
		Container.BindInterfacesAndSelfTo<PlayerProgress>().FromComponentsInNewPrefab(_playerProgressPrefab).WithGameObjectName("PlayerProgress").AsSingle();
		Container.BindInterfacesAndSelfTo<SoundManager>().FromComponentsInNewPrefab(_soundManagerPrefab).WithGameObjectName("SoundManager").AsSingle();
	}
	#endregion
}