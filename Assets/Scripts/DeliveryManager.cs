using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeliveryManager : MonoBehaviour
{
	public event EventHandler OnRecipeSpawnd;
	public event EventHandler OnRecipeCompleted;
	public event EventHandler OnRecipeSuccess;
	public event EventHandler OnRecipeFailed;

	public static DeliveryManager Instance { get; private set; }

	[SerializeField] RecipeListSO recipeListSO;

	private List<RecipeSO> waitingRecipeSOList;
	private float spawnRecipeTimer;
	private float spawnRecipeTimerMax = 4f;
	private int waitingRecipesMax = 4;
	private int successfulRecipeAmount;

	private void Awake()
	{
		Instance = this;
		waitingRecipeSOList = new List<RecipeSO>();
	}

	private void Update()
	{
		spawnRecipeTimer -= Time.deltaTime;
		if (spawnRecipeTimer <= 0f)
		{
			spawnRecipeTimer = spawnRecipeTimerMax;

			if (GameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipesMax)
			{
				RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];

				waitingRecipeSOList.Add(waitingRecipeSO);

				OnRecipeSpawnd?.Invoke(this, EventArgs.Empty);
			}
		}
	}

	public void DeliverRecipe(PlateKitChenObject plateKitChenObject)
	{
		for (int i = 0; i < waitingRecipeSOList.Count; i++)
		{
			RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
			if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitChenObject.GetKitchenObjectSOList().Count)
			{
				// Has the same number of ingredients
				bool plateContentMatchesRecipe = true;
				foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
				{
					// Cycling through all ingredients
					bool ingredientFound = false;
					foreach (KitchenObjectSO plateKitChenObjectSO in plateKitChenObject.GetKitchenObjectSOList())
					{
						if (recipeKitchenObjectSO == plateKitChenObjectSO)
						{
							// Ingredient matches
							ingredientFound = true;
							break;
						}
					}
					if (!ingredientFound)
					{
						// This Recipe Ingredient was not found on the Plate
						plateContentMatchesRecipe = false;
					}
				}

				if (plateContentMatchesRecipe)
				{
					// Player delivered the correct recipe!
					successfulRecipeAmount++;

					waitingRecipeSOList.RemoveAt(i);

					OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
					OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
					return;
				}
			}
		}
		// No matches found
		// Player do not deliver a correct recipe
		OnRecipeFailed?.Invoke(this, EventArgs.Empty);
	}

	public List<RecipeSO> GetWaitingRecipeSOList()
	{
		return waitingRecipeSOList;
	}

	public int GetSuccessfulRecipeAmount()
	{
		return successfulRecipeAmount;
	}

}
