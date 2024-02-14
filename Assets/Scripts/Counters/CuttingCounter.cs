using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
	public static event EventHandler OnAnyCut;

	new public static void ResetStaticData()
	{
		OnAnyCut = null;
	}

	public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
	public event EventHandler OnCut;

	[SerializeField] private CuttingRicipeSO[] cuttingRicipeSOArray;

	private int cuttingProgress;

	public override void Interact(Player player)
	{
		if (!HasKitchenObject())
		{
			// There is no KitchenObject
			if (player.HasKitchenObject())
			{
				// player is carrying something
				if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
				{
					player.GetKitchenObject().SetKitchenObjectParent(this);
					cuttingProgress = 0;
					CuttingRicipeSO cuttingRicipeSO = GetCuttingRecipeSOwithInput(GetKitchenObject().GetKitchenObjectSO());
					OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
					{
						progressNormalized = (float)cuttingProgress / cuttingRicipeSO.cuttingProgressMax
					});
				}
			}
			else
			{
				// player is not carrying anything
			}
		}
		else
		{
			// There is a KitchenObject
			if (player.HasKitchenObject())
			{
				// player is carrying something
				if (player.GetKitchenObject().TryGetPlate(out PlateKitChenObject plateKitChenObject))
				{
					// Player is holding a Plate
					if (plateKitChenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
					{
						GetKitchenObject().DestroySelf();
					}
				}
			}
			else
			{
				// player is not carrying anything
				this.GetKitchenObject().SetKitchenObjectParent(player);
			}
		}
	}

	public override void InteractAlternate(Player player)
	{
		if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
		{
			// There is a KitchenObject here AND it can be cut
			cuttingProgress++;

			OnCut?.Invoke(this, EventArgs.Empty);
			//Debug.Log(OnAnyCut.GetInvocationList().Length);
			OnAnyCut?.Invoke(this, EventArgs.Empty);

			CuttingRicipeSO cuttingRicipeSO = GetCuttingRecipeSOwithInput(GetKitchenObject().GetKitchenObjectSO());

			OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
			{
				progressNormalized = (float)cuttingProgress / cuttingRicipeSO.cuttingProgressMax
			});

			if (cuttingProgress >= cuttingRicipeSO.cuttingProgressMax)
			{
				KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
				GetKitchenObject().DestroySelf();
				KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
			}
		}
	}

	public bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
	{
		CuttingRicipeSO cuttingRicipeSO = GetCuttingRecipeSOwithInput(inputKitchenObjectSO);
		return cuttingRicipeSO != null;
	}

	private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
	{
		CuttingRicipeSO cuttingRicipeSO = GetCuttingRecipeSOwithInput(inputKitchenObjectSO);
		if (cuttingRicipeSO != null)
		{
			return cuttingRicipeSO.output;
		}
		else
		{
			return null;
		}
	}

	private CuttingRicipeSO GetCuttingRecipeSOwithInput(KitchenObjectSO inputKitchenObjectSO)
	{
		foreach (CuttingRicipeSO cuttingRecipeSO in cuttingRicipeSOArray)
		{
			if (cuttingRecipeSO.input == inputKitchenObjectSO)
			{
				return cuttingRecipeSO;
			}
		}
		return null;
	}
}
