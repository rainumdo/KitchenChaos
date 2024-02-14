using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
	public static Player Instance { get; private set; }

	public event EventHandler OnPickedSomething;
	public event EventHandler<OnSelectCounterChangedEventArgs> OnSelectCounterChanged;
	public class OnSelectCounterChangedEventArgs : EventArgs
	{
		public BaseCounter selectedCounter;
	}

	[SerializeField] private float moveSpeed = 7f;
	[SerializeField] private GameInput gameInput;
	[SerializeField] private LayerMask counterLayerMask;

	private bool isWalking;
	private Vector3 lastInteractDir;
	private BaseCounter selectedCounter;
	private KitchenObject kitchenObject;
	public Transform kitchenObjectHoldPoint;

	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("There is more than one Player instance");
		}
		Instance = this;
	}

	// Start is called before the first frame update
	void Start()
	{
		gameInput.OnInteractAction += GameInput_OnInteractAction;
		gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
	}

	private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e)
	{
		if (!GameManager.Instance.IsGamePlaying())
		{
			return;
		}

		if (selectedCounter != null)
		{
			selectedCounter.InteractAlternate(this);
		}
	}

	private void GameInput_OnInteractAction(object sender, System.EventArgs e)
	{
		if (!GameManager.Instance.IsGamePlaying())
		{
			return;
		}

		if (selectedCounter != null)
		{
			selectedCounter.Interact(this);
		}
	}

	// Update is called once per frame
	private void Update()
	{
		HandleMovement();
		HandleInteraction();
	}

	public bool IsWalking()
	{
		return isWalking;
	}

	private void HandleInteraction()
	{
		Vector2 inputvector = gameInput.GetMovementVectorNormalized();

		Vector3 moveDir = new Vector3(inputvector.x, 0f, inputvector.y);
		if (moveDir != Vector3.zero)
		{
			lastInteractDir = moveDir;
		}

		float interactDistance = 2f;
		if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, counterLayerMask))
		{
			if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
			{
				// Has ClearCounter
				if (baseCounter != selectedCounter)
				{
					SetSelectdCounter(baseCounter);
				}
			}
			else
			{
				SetSelectdCounter(null);
			}
		}
		else
		{
			SetSelectdCounter(null);
		}
	}

	private void HandleMovement()
	{
		Vector2 inputvector = gameInput.GetMovementVectorNormalized();

		Vector3 moveDir = new Vector3(inputvector.x, 0f, inputvector.y);

		float moveDistance = Time.deltaTime * moveSpeed;
		float playerRaduis = .7f;
		float playerHeight = 2f;
		bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRaduis, moveDir, moveDistance);

		if (!canMove)
		{
			// Cannot move toward moveDir 
			// Attempt only x movement
			Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
			canMove = (moveDir.x < -.5f || moveDir.x > .5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRaduis, moveDirX, moveDistance);
			if (canMove)
			{
				// Can move only on the X
				moveDir = moveDirX;
			}
			else
			{
				// Cannot move only on the X
				// Attempt only Z movement
				Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
				canMove = (moveDir.z < -.5f || moveDir.z > .5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRaduis, moveDirZ, moveDistance);
				if (canMove)
				{
					// Can move only on the Z
					moveDir = moveDirZ;
				}
				else
				{
					// Cannot move in any direction 
				}
			}

		}

		if (canMove)
		{
			transform.position += moveDir * moveDistance;
		}


		isWalking = moveDir != Vector3.zero;
		if (moveDir != Vector3.zero)
		{
			float rotateSpeed = 10f;
			transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
		}
	}

	private void SetSelectdCounter(BaseCounter selectedCounter)
	{
		this.selectedCounter = selectedCounter;

		OnSelectCounterChanged?.Invoke(this, new OnSelectCounterChangedEventArgs
		{
			selectedCounter = selectedCounter
		});
	}

	public Transform GetKitchenObjectFollowTransform()
	{
		return kitchenObjectHoldPoint;
	}

	public void SetKitchenObject(KitchenObject kitchenObject)
	{
		this.kitchenObject = kitchenObject;

		if (kitchenObject != null)
		{
			OnPickedSomething?.Invoke(this, EventArgs.Empty);
		}
	}

	public KitchenObject GetKitchenObject()
	{
		return kitchenObject;
	}

	public void ClearKitchenObject()
	{
		kitchenObject = null;
	}

	public bool HasKitchenObject()
	{
		return kitchenObject != null;
	}

}
