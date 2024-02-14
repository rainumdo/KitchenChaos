using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconUI : MonoBehaviour
{
	[SerializeField] private PlateKitChenObject plateKitChenObject;
	[SerializeField] private Transform iconTemplate;

	private void Awake()
	{
		iconTemplate.gameObject.SetActive(false);
	}

	private void Start()
	{
		plateKitChenObject.OnIngredientAdded += PlateKitChenObject_OnIngredientAdded;
	}

	private void PlateKitChenObject_OnIngredientAdded(object sender, PlateKitChenObject.OnIngredientAddedEventArgs e)
	{
		UpdateVisual();
	}

	private void UpdateVisual()
	{
		foreach (Transform child in transform)
		{
			if (child == iconTemplate)
			{
				continue;
			}
			Destroy(child.gameObject);
		}

		foreach (KitchenObjectSO kitchenObjectSO in plateKitChenObject.GetKitchenObjectSOList())
		{
			Transform iconTransform = Instantiate(iconTemplate, transform);
			iconTransform.gameObject.SetActive(true);
			iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
		}

	}

}
