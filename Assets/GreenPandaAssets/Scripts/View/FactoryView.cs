using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FactoryView : MonoBehaviour
{
	#region Serialized Properties
	[SerializeField]
	private List<GameObject> skinsList;
	#endregion

	#region Injected Values
	[Inject]
	private PlayerProgress _playerProgress;
	[Inject]
	private SignalBus _signalBus;
	#endregion

	#region Private Members
	private Animator _anim;
	private int _currentSkinLevel = 1;
	private float _animDuration = 1f;
	#endregion

	#region Private Methods
	private void Start()
	{
		_signalBus.Subscribe<FactoryUpgradePurchasedSignal>(Upgrade);
		_anim = GetComponent<Animator>();
		UpdateSkin(_currentSkinLevel);
	}

	private void Upgrade(FactoryUpgradePurchasedSignal data)
	{
		int skinLevel = -1;
		skinLevel = _playerProgress.FactoryyLevel / 5 + 1;
		SetSkinLevel(skinLevel);
	}

	private IEnumerator WaitForSkinUpdate()
	{
		yield return new WaitForSeconds(_animDuration / 2);
		_anim.SetBool("isUpgrading", false);
		UpdateSkin(_currentSkinLevel);
	}

	private void UpdateSkin(int skinLevel)
	{
		if (skinLevel < skinsList.Count)
		{
			skinsList[skinLevel - 1].SetActive(true);
		}
	}
	#endregion

	#region Public Methods
	public void SetSkinLevel(int skinLevel)
	{
		_anim.SetBool("isUpgrading", true);
		_currentSkinLevel = skinLevel;
		StartCoroutine(WaitForSkinUpdate());
	}
	#endregion
}