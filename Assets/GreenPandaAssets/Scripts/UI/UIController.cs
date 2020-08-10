using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class UIController : MonoBehaviour
{
	#region Serialized Fields
	[SerializeField]
	private FadeController _fadeController;
	[SerializeField]
	Canvas canvas;
	[SerializeField]
	private Sprite _dozerImage, _fabricImage, _truckImage;
	#endregion

	#region Injected Fields
	[Inject]
	private PlayerProgress _playerProgress;
	[Inject]
	private SignalBus _signalBus;
	#endregion

	#region Private Members
	private GameObject _notEnoughtMoneyPopup, _maxLevelReachedPopup, _featureUpgradePopup;
	private List<GameObject> treesList;
	private List<EnvProp> envList;

	private Button _upgradeFactoryButton, _upgradeDozerButton, _upgradeTruckButton;
	private Button _showUpgradePanel;
	private Button _restartButton;
	private TextMeshProUGUI _coinText, _coinPerMinuteText;
	private TextMeshProUGUI _upgradeFactoryText, _upgradeDozerText, _upgradeTruckText;
	private TextMeshProUGUI _factoryLevelText, _dozerLevelText, _truckLevelText;
	private TextMeshProUGUI _factoryNextLevelText, _dozerNextLevelText, _truckNextLevelText;

	private float _currentScale = InitScale;
	private float _deltaTime = AnimationTimeSeconds / FramesCount;
	private float _dx = (TargetScale - InitScale) / FramesCount;
	private bool _upScale = true;
	#endregion

	#region Conastants
	private const float TargetScale = 1.3f;
	private const float InitScale = 1f;
	private const int FramesCount = 100;
	private const float AnimationTimeSeconds = 1;
	#endregion

	#region Pivate Voids
	private void Awake()
	{
		_upgradeDozerButton = canvas.gameObject.transform.Find("UIContorller/UpgradePanel/DozerUpgradePanel/Button").GetComponent<Button>();
		_upgradeDozerButton.onClick.AddListener(UpgradeDozer);
		_upgradeTruckButton = canvas.gameObject.transform.Find("UIContorller/UpgradePanel/TruckUpgradePanel/Button").GetComponent<Button>();
		_upgradeTruckButton.onClick.AddListener(UpgradeTruck);
		_upgradeFactoryButton = canvas.gameObject.transform.Find("UIContorller/UpgradePanel/FactoryUpgradePanel/Button").GetComponent<Button>();
		_upgradeFactoryButton.onClick.AddListener(UpgradeFactory);
		_coinText = canvas.gameObject.transform.Find("UIContorller/Top/CoinValue_Text").GetComponent<TextMeshProUGUI>();
		_coinText.text = _playerProgress.Coins.ToString();
		_signalBus.Subscribe<AddCoinsSignal>(AddCoin);
		_restartButton = canvas.gameObject.transform.Find("UIContorller/Top/Button_Restart").GetComponent<Button>();
		_restartButton.onClick.AddListener(RestartGame);

		_upgradeFactoryText = canvas.gameObject.transform.Find("UIContorller/UpgradePanel/FactoryUpgradePanel/Button/Value_Text").GetComponent<TextMeshProUGUI>();
		_upgradeDozerText = canvas.gameObject.transform.Find("UIContorller/UpgradePanel/DozerUpgradePanel/Button/Value_Text").GetComponent<TextMeshProUGUI>();
		_upgradeTruckText = canvas.gameObject.transform.Find("UIContorller/UpgradePanel/TruckUpgradePanel/Button/Value_Text").GetComponent<TextMeshProUGUI>();
		_factoryLevelText = canvas.gameObject.transform.Find("UIContorller/UpgradePanel/FactoryUpgradePanel/Upgrade_Image/Image/Text_1").GetComponent<TextMeshProUGUI>();
		_dozerLevelText = canvas.gameObject.transform.Find("UIContorller/UpgradePanel/DozerUpgradePanel/Upgrade_Image/Image/Text_1").GetComponent<TextMeshProUGUI>();
		_truckLevelText = canvas.gameObject.transform.Find("UIContorller/UpgradePanel/TruckUpgradePanel/Upgrade_Image/Image/Text_1").GetComponent<TextMeshProUGUI>();
		_factoryNextLevelText = canvas.gameObject.transform.Find("UIContorller/UpgradePanel/FactoryUpgradePanel/Upgrade_Image/Image/Text_2").GetComponent<TextMeshProUGUI>();
		_dozerNextLevelText = canvas.gameObject.transform.Find("UIContorller/UpgradePanel/DozerUpgradePanel/Upgrade_Image/Image/Text_2").GetComponent<TextMeshProUGUI>();
		_truckNextLevelText = canvas.gameObject.transform.Find("UIContorller/UpgradePanel/TruckUpgradePanel/Upgrade_Image/Image/Text_2").GetComponent<TextMeshProUGUI>();

		_notEnoughtMoneyPopup = canvas.gameObject.transform.Find("UIContorller/PopupsPanel/NotEnoughtMoneyPopup").gameObject;
		_maxLevelReachedPopup = canvas.gameObject.transform.Find("UIContorller/PopupsPanel/MaxLevelPopup").gameObject;
		_featureUpgradePopup = canvas.gameObject.transform.Find("UIContorller/PopupsPanel/FeatureUpgradePopup").gameObject;
		_coinPerMinuteText = canvas.gameObject.transform.Find("UIContorller/Top/Image_CoinPerMinute/CointPerMinute_Value").GetComponent<TextMeshProUGUI>();
		envList = FindObjectsOfType<EnvProp>().ToList();
		treesList = new List<GameObject>();
		GetAllTrees();
	}

	private void Update()
	{
		UpdateCoinsValue();
		UpdateUpgradePanelValues();
	}

	private void GetAllTrees()
	{
		foreach (var item in envList)
		{
			if (item.name.Contains("Tree") || item.name.Contains("Grass") || item.name.Contains("Bush") || item.name.Contains("Branch"))
			{
				treesList.Add(item.gameObject);
			}
		}
	}

	private void AnimateAllTrees()
	{
		StartCoroutine(ScaleTreesCoroutine());
	}

	private IEnumerator ScaleTreesCoroutine()
	{
		while (_upScale)
		{
			_currentScale += _dx;
			if (_currentScale > TargetScale)
			{
				_upScale = false;
				_currentScale = TargetScale;
			}
			foreach (var item in treesList)
			{
				item.transform.localScale = Vector3.one * _currentScale;
			}

			yield return new WaitForSeconds(_deltaTime);
		}

		while (!_upScale)
		{
			_currentScale -= _dx;
			if (_currentScale < InitScale)
			{
				_upScale = true;
				_currentScale = InitScale;
			}
			foreach (var item in treesList)
			{
				item.transform.localScale = Vector3.one * _currentScale;
			}
			yield return new WaitForSeconds(_deltaTime);
		}
	}

	private void RestartGame()
	{
		PlayerPrefs.DeleteAll();
		SceneManager.LoadScene("MainScene");
		_coinPerMinuteText.text = "";
	}

	private void AddCoin(AddCoinsSignal data)
	{
		UpdateCoinsValue();
	}

	private void ShowNoMoneyPopup()
	{
		_notEnoughtMoneyPopup.SetActive(true);
		_fadeController.FadeIn(_notEnoughtMoneyPopup);
	}

	private void ShowFeatureUpgradePopup(string name, int level)
	{
		switch (name)
		{
			case "Factory":
				{
					_featureUpgradePopup.transform.Find("Image").GetComponent<Image>().sprite = _fabricImage;
					break;
				}
			case "Dozer":
				{
					_featureUpgradePopup.transform.Find("Image").GetComponent<Image>().sprite = _dozerImage;
					break;
				}
			case "Truck":
				{
					_featureUpgradePopup.transform.Find("Image").GetComponent<Image>().sprite = _truckImage;
					break;
				}
		}
		_featureUpgradePopup.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "this feature was upgraded to level: " + level;
		_featureUpgradePopup.gameObject.SetActive(true);
		_fadeController.FadeIn(_featureUpgradePopup);
	}

	private void ShowMaxLevelPopup(string name, int level)
	{
		switch (name)
		{
			case "Factory":
				{
					_maxLevelReachedPopup.transform.Find("Image").GetComponent<Image>().sprite = _fabricImage;
					break;
				}
			case "Dozer":
				{
					_maxLevelReachedPopup.transform.Find("Image").GetComponent<Image>().sprite = _dozerImage;
					break;
				}
			case "Truck":
				{
					_maxLevelReachedPopup.transform.Find("Image").GetComponent<Image>().sprite = _truckImage;
					break;
				}
		}
		_maxLevelReachedPopup.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "this feature was upgraded to maximum level: " + level;
		_maxLevelReachedPopup.gameObject.SetActive(true);
		_fadeController.FadeIn(_maxLevelReachedPopup);
	}

	private void UpdateUpgradePanelValues()
	{
		if (_playerProgress != null)
		{
			_factoryLevelText.text = _playerProgress.FactoryyLevel.ToString();
			_dozerLevelText.text = _playerProgress.BullDozerLevel.ToString();
			_truckLevelText.text = _playerProgress.DumpingTruckLevel.ToString();
			_upgradeFactoryText.text = _playerProgress.FactoryUpgradeStartValue.ToString();
			_upgradeDozerText.text = _playerProgress.BullDozerUpgradeStartValue.ToString();
			_upgradeTruckText.text = _playerProgress.TruckUpgradeStartValue.ToString();
			if (_playerProgress.FactoryyLevel < 15)
			{
				_factoryNextLevelText.text = (_playerProgress.FactoryyLevel + 1).ToString();
			}
			else
			{
				_factoryNextLevelText.text = 15.ToString();
			}
			if (_playerProgress.BullDozerLevel < 15)
			{
				_dozerNextLevelText.text = (_playerProgress.BullDozerLevel + 1).ToString();
			}
			else
			{
				_dozerNextLevelText.text = 15.ToString();
			}
			if (_playerProgress.DumpingTruckLevel < 15)
			{
				_truckNextLevelText.text = (_playerProgress.DumpingTruckLevel + 1).ToString();
			}
			else
			{
				_truckNextLevelText.text = 15.ToString();
			}
			_coinPerMinuteText.text = (_playerProgress.CalculateCoinsPerMinute() + "/min").ToString();
		}
	}

	private void UpgradeFactory()
	{
		if (_playerProgress.FactoryyLevel < 15)
		{
			if (_playerProgress.Coins >= _playerProgress.FactoryUpgradeStartValue)
			{
				_playerProgress.UpgradeFactory();
				_playerProgress.Coins -= _playerProgress.FactoryUpgradeStartValue;
				_playerProgress.FactoryUpgradeStartValue = _playerProgress.FactoryUpgradeStartValue * 2;
				UpdateCoinsValue();
				UpdateUpgradePanelValues();
				AnimateAllTrees();
				if (_playerProgress.FactoryyLevel % 5 == 0 && _playerProgress.FactoryyLevel < 15)
				{
					ShowFeatureUpgradePopup("Factory", _playerProgress.FactoryyLevel);
				}
			}
			else
			{
				ShowNoMoneyPopup();
			}
		}
		else
		{
			ShowMaxLevelPopup("Factory", _playerProgress.FactoryyLevel);
			_upgradeFactoryButton.interactable = false;
		}
	}

	private void UpgradeDozer()
	{
		if (_playerProgress.BullDozerLevel < 15)
		{
			if (_playerProgress.Coins >= _playerProgress.BullDozerUpgradeStartValue)
			{
				_playerProgress.UpgradeBullDozer();
				_playerProgress.Coins -= _playerProgress.BullDozerUpgradeStartValue;
				_playerProgress.BullDozerUpgradeStartValue = _playerProgress.BullDozerUpgradeStartValue * 2;
				UpdateCoinsValue();
				UpdateUpgradePanelValues();
				AnimateAllTrees();
				if (_playerProgress.BullDozerLevel % 5 == 0 && _playerProgress.BullDozerLevel < 15)
				{
					ShowFeatureUpgradePopup("Dozer", _playerProgress.BullDozerLevel);
				}
			}
			else
			{
				ShowNoMoneyPopup();
			}
		}
		else
		{
			_upgradeDozerButton.interactable = false;
			ShowMaxLevelPopup("Dozer", _playerProgress.BullDozerLevel);
		}
	}

	private void UpgradeTruck()
	{
		if (_playerProgress.DumpingTruckLevel < 15)
		{
			if (_playerProgress.Coins >= _playerProgress.TruckUpgradeStartValue)
			{
				_playerProgress.UpgradeTruck();
				_playerProgress.Coins -= _playerProgress.TruckUpgradeStartValue;
				_playerProgress.TruckUpgradeStartValue = _playerProgress.TruckUpgradeStartValue * 2;
				UpdateCoinsValue();
				UpdateUpgradePanelValues();
				AnimateAllTrees();
				if (_playerProgress.DumpingTruckLevel % 5 == 0 && _playerProgress.DumpingTruckLevel < 15)
				{
					ShowFeatureUpgradePopup("Truck", _playerProgress.DumpingTruckLevel);
				}
			}
			else
			{
				ShowNoMoneyPopup();
			}
		}
		else
		{
			ShowMaxLevelPopup("Truck", _playerProgress.DumpingTruckLevel);
			_upgradeTruckButton.interactable = false;
		}
	}

	private void UpdateCoinsValue()
	{
		_coinText.text = _playerProgress.Coins.ToString();
	}
	#endregion
}