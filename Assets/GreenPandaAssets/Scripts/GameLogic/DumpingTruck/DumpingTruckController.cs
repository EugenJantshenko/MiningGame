using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumpingTruckController : MonoBehaviour
{
	#region Serialized Properties
	[SerializeField]
	private GameObject _сargo;
	[SerializeField]
	private readonly float _startMovingTime = 5f;
	#endregion

	#region Private Memers
	private PlayerProgress _playerProgress;
	private List<Transform> _pathPoints;
	private bool _isCanMove = true;
	private readonly bool _stopMoveCoroutine = false;
	private bool _isLoadingCargo = false;
	private int _previousPoint;
	private float _truckSpeed = 0.001f;
	private float _fullDistance;
	#endregion

	#region Constants
	private const float MIN_DITANCE = 0.5f;
	#endregion

	#region Private Methods
	private void CalculateTruckSpeed()
	{
		_truckSpeed = _fullDistance / _startMovingTime;
	}

	private void CalculateFullDistance()
	{
		for (int i = 0; i < _pathPoints.Count - 1; i++)
		{
			_fullDistance += Vector3.Distance(_pathPoints[i + 1].position, _pathPoints[i].position);
		}
	}

	private void IsCargoActive(bool isActive)
	{
		if (_сargo != null)
		{
			_сargo.SetActive(isActive);
		}
		else
		{
			Debug.LogWarningFormat("[{0}][IsCargoActive]_сargo is NULL!", GetType().Name);
		}
	}

	private IEnumerator MoveLogic()
	{
		while (!_stopMoveCoroutine)
		{
			if (!_isLoadingCargo)
			{
				if (_isCanMove)
				{
					MoveTo();
				}
				else
				{
					if (_previousPoint == _pathPoints.Count - 2)
					{
						_playerProgress.HandOverCargo();
						SetStartPosition();
						LoockAtNextPoint();
					}
					else
					{
						_previousPoint++;
						LoockAtNextPoint();
					}
					_isCanMove = true;
				}
			}
			yield return new WaitForFixedUpdate();
		}
	}

	private void MoveTo()
	{
		transform.localPosition += NormalVector3() * Time.deltaTime * _truckSpeed;
		float distance = Vector3.Distance(GetNexpPointPosition(), transform.localPosition);

		if (distance < MIN_DITANCE)
		{
			_isCanMove = false;
		}
	}

	private Vector3 NormalVector3()
	{
		return (GetNexpPointPosition() - transform.localPosition).normalized;
	}

	private void LoockAtNextPoint()
	{
		if (_previousPoint + 1 < _pathPoints.Count)
		{
			transform.LookAt(new Vector3(GetNexpPointPosition().x, transform.position.y, GetNexpPointPosition().z));
		}
	}

	private Vector3 GetNexpPointPosition()
	{
		return new Vector3(_pathPoints[_previousPoint + 1].localPosition.x,
																	 transform.localPosition.y,
																	 _pathPoints[_previousPoint + 1].localPosition.z);
	}

	private void SetStartPosition()
	{
		transform.localPosition = _pathPoints[0].position;
		_previousPoint = 0;
		IsCargoActive(false);

		if (_playerProgress.DumpingTruckLevel != 1)
		{
			_playerProgress.TruckCycleTime = _startMovingTime - (0.15f * _playerProgress.DumpingTruckLevel);
		}
		else
		{
			_playerProgress.TruckCycleTime = _startMovingTime;
		}
		_playerProgress.CalculateCoinsPerMinute();
	}
	#endregion

	#region Public Methods
	public DumpingTruckController SetParams(PlayerProgress playerProgress)
	{
		_playerProgress = playerProgress;
		return this;
	}

	public void Initialized(List<Transform> pathPoints)
	{
		_pathPoints = pathPoints;
		if (_pathPoints == null || _pathPoints.Count == 0)
		{
			Debug.LogWarningFormat("[{0}][Initialized]_pathPoints is NULL ore empty", GetType().Name);
			return;
		}
		_fullDistance = 0;
		CalculateFullDistance();
		CalculateTruckSpeed();
		SetStartPosition();
		transform.rotation = new Quaternion(0, 0, 0, 0);
		LoockAtNextPoint();
		StartCoroutine(MoveLogic());
	}

	public void IsLoadingCargo(bool value)
	{
		_isLoadingCargo = value;
		if (!_isLoadingCargo)
		{
			IsCargoActive(true);

		}
	}
	#endregion
}