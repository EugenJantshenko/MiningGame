using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class BullDozerAnimationController : MonoBehaviour
{
	#region Serialized Properties
	[SerializeField]
	private readonly float _startAnimationTime = 4f;
	#endregion

	#region Injected Properties
	[Inject]
	private PlayerProgress _playerProgress;
	[Inject]
	private SoundManager _soundManager;
	#endregion

	#region Private Members
	private Action _callBack;
	private Animator _animator;
	private float _animationSpeedCoeficient;
	private bool _loadEnded;
	#endregion

	#region Private Methods
	private void Start()
	{
		_animator = GetComponent<Animator>();
		CalculateAnimationSpeedCoeficient(_startAnimationTime);
	}

	private void CalculateAnimationSpeedCoeficient(float startAnimationTime)
	{
		if (_playerProgress.BullDozerLevel == 1)
		{
			_animationSpeedCoeficient = 2f / startAnimationTime;
		}
		else
		{
			_animationSpeedCoeficient = 2f / (startAnimationTime - (0.1f * _playerProgress.BullDozerLevel));
		}
	}

	private IEnumerator WaitForStoppingAnimation()
	{
		while (!_loadEnded)//условия выхода конец анимации
		{
			yield return null;
		}

		_animator.SetBool("startPlayAnimation", false);
		_loadEnded = false;
		_callBack.Invoke();
	}
	#endregion

	#region public Methods
	public void StartAnimation(Action callBack)
	{
		if (_playerProgress.BullDozerLevel != 1)
		{
			_playerProgress.BullDozerLoadingTime = _startAnimationTime - (0.1f * _playerProgress.BullDozerLevel);
		}
		else
		{
			_playerProgress.BullDozerLoadingTime = _startAnimationTime;
		}

		CalculateAnimationSpeedCoeficient(_startAnimationTime);
		_callBack = callBack;
		_soundManager.PlaySound(SoundEnum.DozerLoading);
		_animator.speed = _animationSpeedCoeficient;
		_animator.SetBool("startPlayAnimation", true);
		StartCoroutine(WaitForStoppingAnimation());
		_loadEnded = false;
	}

	public void AnimaionEnded(string message)
	{
		if (message == "LoadAnimationEnded")
		{
			_loadEnded = true;
			_soundManager.PlaySound(SoundEnum.LoadingComplete);
		}
	}
	#endregion
}