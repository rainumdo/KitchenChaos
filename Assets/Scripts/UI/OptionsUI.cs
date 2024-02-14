using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class OptionsUI : MonoBehaviour
{
	public static OptionsUI Instance { get; private set; }

	[SerializeField] private Button soundEffectButton;
	[SerializeField] private Button musicButton;
	[SerializeField] private Button closeButton;
	[SerializeField] private TextMeshProUGUI soundEffectText;
	[SerializeField] private TextMeshProUGUI musicText;

	[SerializeField] private TextMeshProUGUI upText;
	[SerializeField] private TextMeshProUGUI downText;
	[SerializeField] private TextMeshProUGUI leftText;
	[SerializeField] private TextMeshProUGUI rightText;
	[SerializeField] private TextMeshProUGUI interactText;
	[SerializeField] private TextMeshProUGUI interactAltText;
	[SerializeField] private TextMeshProUGUI pauseText;

	[SerializeField] private Button upButton;
	[SerializeField] private Button downButton;
	[SerializeField] private Button leftButton;
	[SerializeField] private Button rightButton;
	[SerializeField] private Button interactButton;
	[SerializeField] private Button interactAltButton;
	[SerializeField] private Button pauseButton;

	[SerializeField] private Transform pressToRebindKeyTransform;

	private Action onCloseButton;

	private void Awake()
	{
		Instance = this;

		soundEffectButton.onClick.AddListener(() =>
		{
			SoundManager.Instance.ChangeVolume();
			UpdateVisual();
		});
		musicButton.onClick.AddListener(() =>
		{
			MusicManager.Instance.ChangeVolume();
			UpdateVisual();
		});
		closeButton.onClick.AddListener(() =>
		{
			Hide();
			onCloseButton();
		});

		upButton.onClick.AddListener(() => Rebinding(GameInput.Binding.Move_Up));
		downButton.onClick.AddListener(() => Rebinding(GameInput.Binding.Move_Down));
		leftButton.onClick.AddListener(() => Rebinding(GameInput.Binding.Move_Left));
		rightButton.onClick.AddListener(() => Rebinding(GameInput.Binding.Move_Right));
		interactButton.onClick.AddListener(() => Rebinding(GameInput.Binding.Interact));
		interactAltButton.onClick.AddListener(() => Rebinding(GameInput.Binding.InteractAlt));
		pauseButton.onClick.AddListener(() => Rebinding(GameInput.Binding.Pause));
	}

	private void Start()
	{
		GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpasued;

		UpdateVisual();

		HidePressKeyToRebindKey();
		Hide();
	}

	private void GameManager_OnGameUnpasued(object sender, System.EventArgs e)
	{
		Hide();
	}

	private void UpdateVisual()
	{
		soundEffectText.text = "Sound : " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
		musicText.text = "Music : " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

		upText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
		downText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
		leftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
		rightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
		interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
		interactAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlt);
		pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
	}

	public void Show(Action onCloseButton)
	{
		this.onCloseButton = onCloseButton;

		gameObject.SetActive(true);

		soundEffectButton.Select();
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

	private void ShowPressKeyToRebindKey()
	{
		pressToRebindKeyTransform.gameObject.SetActive(true);
	}

	private void HidePressKeyToRebindKey()
	{
		pressToRebindKeyTransform.gameObject.SetActive(false);
	}

	private void Rebinding(GameInput.Binding binding)
	{
		ShowPressKeyToRebindKey();
		GameInput.Instance.Rebinding(binding, () =>
		{
			HidePressKeyToRebindKey();
			UpdateVisual();
		});
	}
}
