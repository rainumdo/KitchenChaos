using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoveCounter : BaseCounter, IHasProgress
{
	public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
	public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
	public class OnStateChangedEventArgs : EventArgs
	{
		public State state;
	}

	public enum State
	{
		Idle,
		Frying,
		Fried,
		Burned
	}

	[SerializeField] private FryingRicipeSO[] fryingRicipeSOArray;
	[SerializeField] private BurningRicipeSO[] BurningRicipeSOArray;

	private State state;
	private float fryingTimer;
	private FryingRicipeSO fryingRicipeSO;
	private float burningTimer;
	private BurningRicipeSO burningRicipeSO;

	private void Start()
	{
		state = State.Idle;
	}

	private void Update()
	{
		if (HasKitchenObject())
		{
			switch (state)
			{
				case State.Idle:
					break;
				case State.Frying:
					fryingTimer += Time.deltaTime;

					OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
					{
						progressNormalized = fryingTimer / fryingRicipeSO.fryingTimerMax
					});

					if (fryingTimer > fryingRicipeSO.fryingTimerMax)
					{
						// Fried
						GetKitchenObject().DestroySelf();

						KitchenObject.SpawnKitchenObject(fryingRicipeSO.output, this);

						state = State.Fried;
						burningTimer = 0f;
						burningRicipeSO = GetBurningRecipeSOwithInput(GetKitchenObject().GetKitchenObjectSO());

						OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
						{
							state = state
						});
					}
					break;
				case State.Fried:
					burningTimer += Time.deltaTime;

					OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
					{
						progressNormalized = burningTimer / burningRicipeSO.burningTimerMax
					});

					if (burningTimer > burningRicipeSO.burningTimerMax)
					{
						// Fried
						GetKitchenObject().DestroySelf();

						KitchenObject.SpawnKitchenObject(burningRicipeSO.output, this);

						state = State.Burned;

						OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
						{
							state = state
						});

						OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
						{
							progressNormalized = 0f
						});
					}
					break;
				case State.Burned:
					break;
			}
		}
	}

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

					fryingRicipeSO = GetFryingRecipeSOwithInput(GetKitchenObject().GetKitchenObjectSO());

					state = State.Frying;
					fryingTimer = 0f;

					OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
					{
						state = state
					});

					OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
					{
						progressNormalized = fryingTimer / fryingRicipeSO.fryingTimerMax
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

						state = State.Idle;

						OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
						{
							state = state
						});

						OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
						{
							progressNormalized = 0f
						});
					}
				}
			}
			else
			{
				// player is not carrying anything
				this.GetKitchenObject().SetKitchenObjectParent(player);

				state = State.Idle;

				OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
				{
					state = state
				});

				OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
				{
					progressNormalized = 0f
				});
			}
		}
	}

	public bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
	{
		FryingRicipeSO cuttingRicipeSO = GetFryingRecipeSOwithInput(inputKitchenObjectSO);
		return cuttingRicipeSO != null;
	}

	private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
	{
		FryingRicipeSO fryingRicipeSO = GetFryingRecipeSOwithInput(inputKitchenObjectSO);
		if (fryingRicipeSO != null)
		{
			return fryingRicipeSO.output;
		}
		else
		{
			return null;
		}
	}

	private FryingRicipeSO GetFryingRecipeSOwithInput(KitchenObjectSO inputKitchenObjectSO)
	{
		foreach (FryingRicipeSO fryingRecipeSO in fryingRicipeSOArray)
		{
			if (fryingRecipeSO.input == inputKitchenObjectSO)
			{
				return fryingRecipeSO;
			}
		}
		return null;
	}

	private BurningRicipeSO GetBurningRecipeSOwithInput(KitchenObjectSO inputKitchenObjectSO)
	{
		foreach (BurningRicipeSO burningRecipeSO in BurningRicipeSOArray)
		{
			if (burningRecipeSO.input == inputKitchenObjectSO)
			{
				return burningRecipeSO;
			}
		}
		return null;
	}

	public bool IsFried()
	{
		return state == State.Fried;
	}
}
