using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI keyUpText;
	[SerializeField] private TextMeshProUGUI keyDownText;
	[SerializeField] private TextMeshProUGUI keyLeftText;
	[SerializeField] private TextMeshProUGUI keyRightText;
	[SerializeField] private TextMeshProUGUI keyInteractText;
	[SerializeField] private TextMeshProUGUI keyAltText;
	[SerializeField] private TextMeshProUGUI keyPauseText;

	private void Start()
	{
		GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
		GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

		UpdateVisual();

		Show();
	}

	private void GameManager_OnStateChanged(object sender, System.EventArgs e)
	{
		if (GameManager.Instance.IsCountdownToStartActive())
		{
			Hide();
		}
	}

	private void GameInput_OnBindingRebind(object sender, System.EventArgs e)
	{
		UpdateVisual();
	}

	private void UpdateVisual()
	{
		keyUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
		keyDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
		keyLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
		keyRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
		keyInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
		keyAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlt);
		keyPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
	}

	private void Show()
	{
		gameObject.SetActive(true);
	}

	private void Hide()
	{
		gameObject.SetActive(false);
	}
}
