using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerProgress : MonoBehaviour, IInitializable
{
	#region Injected Fields
	[Inject]
	private SignalBus _signalBuss;
	#endregion

	#region Serialized Fields
	[SerializeField]
	private List<ItemConfig> _configList;
	#endregion

	#region Private Members
	private int _rocksDelivered;
	private float _coinsPerMinute;
	#endregion

	#region Public Members
	public int FactoryyLevel;
	public int FactoryUpgradeStartValue;
	public int BullDozerLevel;
	public int BullDozerUpgradeStartValue;
	public int DumpingTruckLevel;
	public int TruckUpgradeStartValue;
	public int Coins;
	public float BullDozerLoadingTime;
	public float TruckCycleTime;
	#endregion

	#region Private Methods
	private void SaveData()
	{
		PlayerPrefs.SetInt("RockDelivered", _rocksDelivered);
		PlayerPrefs.SetInt("FactoryUpgradeStartValue", FactoryUpgradeStartValue);
		PlayerPrefs.SetInt("BullDozerUpgradeStartValue", BullDozerUpgradeStartValue);
		PlayerPrefs.SetInt("TruckUpgradeStartValue", TruckUpgradeStartValue);
		PlayerPrefs.SetInt("FactoryyLevel", FactoryyLevel);
		PlayerPrefs.SetInt("BullDozerLevel", BullDozerLevel);
		PlayerPrefs.SetInt("DumpingTruckLevel", DumpingTruckLevel);
		PlayerPrefs.SetInt("Coins", Coins);
	}

	private void OnApplicationFocus(bool focus)
	{
		if (!focus)
		{
			SaveData();
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			SaveData();
		}
	}

	private void OnApplicationQuit()
	{
		SaveData();
	}

	private void LoadData()
	{
		if (PlayerPrefs.HasKey("RockDelivered")) { _rocksDelivered = PlayerPrefs.GetInt("RockDelivered"); }
		if (PlayerPrefs.HasKey("FactoryUpgradeStartValue")) { FactoryUpgradeStartValue = PlayerPrefs.GetInt("FactoryUpgradeStartValue"); }
		if (PlayerPrefs.HasKey("BullDozerUpgradeStartValue")) { BullDozerUpgradeStartValue = PlayerPrefs.GetInt("BullDozerUpgradeStartValue"); }
		if (PlayerPrefs.HasKey("TruckUpgradeStartValue")) { TruckUpgradeStartValue = PlayerPrefs.GetInt("TruckUpgradeStartValue"); }
		if (PlayerPrefs.HasKey("FactoryyLevel")) { FactoryyLevel = PlayerPrefs.GetInt("FactoryyLevel"); }
		if (PlayerPrefs.HasKey("BullDozerLevel")) { BullDozerLevel = PlayerPrefs.GetInt("BullDozerLevel"); }
		if (PlayerPrefs.HasKey("DumpingTruckLevel")) { DumpingTruckLevel = PlayerPrefs.GetInt("DumpingTruckLevel"); }
		if (PlayerPrefs.HasKey("Coins")) { Coins = PlayerPrefs.GetInt("Coins"); }
	}
	#endregion

	#region Public Methods
	public void Initialize()
	{
		_rocksDelivered = 0;
		FactoryUpgradeStartValue = _configList[1].StartUpdatePrice;
		BullDozerUpgradeStartValue = _configList[0].StartUpdatePrice;
		TruckUpgradeStartValue = _configList[2].StartUpdatePrice;
		FactoryyLevel = _configList[1].Level;
		BullDozerLevel = _configList[0].Level;
		DumpingTruckLevel = _configList[2].Level;
		Coins = _configList[3].СurrentCoin;
		LoadData();
	}

	public void HandOverCargo()
	{
		if (BullDozerLevel == 5)
		{
			_rocksDelivered += 10;
		}
		else
		{
			_rocksDelivered += 5;
		}
		Coins += FactoryyLevel * 5;
		_signalBuss.Fire(new AddCoinsSignal() { value = FactoryyLevel });
		_rocksDelivered = 0;
	}

	public int CalculateCoinsPerMinute()
	{
		double cycles = 60 / (BullDozerLoadingTime + TruckCycleTime);
		double coinPerCycle = FactoryyLevel * 5;
		return (int)(cycles * coinPerCycle);
	}

	public void UpgradeFactory()
	{
		FactoryyLevel++;
		_signalBuss.Fire(new FactoryUpgradePurchasedSignal());
	}

	public void UpgradeBullDozer()
	{
		BullDozerLevel++;
		//_signalBuss.Fire(new DozerUpgradePurchasedSignal()); 
		// TODO: Сделать View для бульдозера когда будут скины
	}

	public void UpgradeTruck()
	{
		DumpingTruckLevel++;
		//_signalBuss.Fire(new TruckUpgradePurchasedSignal());
		// TODO: Сделать View для грузовика когда будут скины
	}
	#endregion
}