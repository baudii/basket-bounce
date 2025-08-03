using BasketBounce.DOTweenComponents;
using BasketBounce.Systems;
using KK.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BasketBounce.Gameplay
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class FallingInteractable : MonoBehaviour, IResetableItem, ILevelValidator
	{
		[SerializeField] Rigidbody2D rb;
		[SerializeField] SimpleMoveTween simpleMoveTween;
		[SerializeField] bool isDisabled;
		[SerializeField] int blocksUnder;
		Vector3 initialPosition;


		bool isFalling;
		bool hasFallen;
		bool isDisabledInitial;
		bool isArrowEnabledInitial;
		
		float startTime;
		float? fallenTime;
		float minFallTime = 0.5f;

		int blocksUnderBroken;

		private void Awake()
		{
			this.Log($"Awake in {transform.name}");
			initialPosition = transform.position;
			rb = GetComponent<Rigidbody2D>();
			isDisabledInitial = isDisabled;
			isArrowEnabledInitial = simpleMoveTween.gameObject.activeSelf;
		}

		private void FixedUpdate()
		{
            if (hasFallen || isDisabled)
				return;

			if (Time.time - startTime < 0.5f)
				return;

			if (rb.velocity.magnitude > 3)
			{
				fallenTime = null;

				if (isFalling) 
					return;

				simpleMoveTween.gameObject.SetActive(false);
				isFalling = true;
			}
			else if (isFalling)
			{
				if (fallenTime == null)
					fallenTime = Time.time;

				if (Time.time - fallenTime.Value >= minFallTime)
				{
					rb.isKinematic = true;
					rb.velocity = Vector3.zero;
					hasFallen = true;
				}
			}
		}

		public void DisableArrow() => simpleMoveTween.gameObject.SetActive(false);

		public void Enable()
		{
			blocksUnderBroken++;
			if (blocksUnderBroken >= blocksUnder)
			{
				isDisabled = false;
			}
		}

		public void ResetState()
		{
			this.Log(transform.name);
			transform.position = initialPosition;
			rb.isKinematic = false;
			isFalling = false;
			hasFallen = false;
			isDisabled = isDisabledInitial;
			startTime = Time.time;
			simpleMoveTween.gameObject.SetActive(true);
			blocksUnderBroken = 0;
		}


#if UNITY_EDITOR
		[Header("Editor only")]
		[SerializeField] bool validate = true;
		private void OnValidate()
		{
			if (validate)
			{
				Validate();
            }
		}

		private void HandleAsyncOp(AsyncOperationHandle<GameObject> prefab)
		{
			this.Log("Object loaded successfully");
			GameObject go = Instantiate(prefab.Result, transform);

            simpleMoveTween = go.GetComponent<SimpleMoveTween>();
			if (simpleMoveTween == null)
			{
				this.LogError("Something went wrong when creating SimpleMoveTween gameobject");
			}
		}

        public void Validate()
        {
			this.Log($"Validating {transform.name}");
            rb = GetComponent<Rigidbody2D>();
            rb.freezeRotation = true;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 6.0f;
            rb.mass = 5;
            if (simpleMoveTween == null)
            {
                var asyncOp = Addressables.LoadAssetAsync<GameObject>("AnimatedArrowObj");
                asyncOp.Completed += HandleAsyncOp;
            }
            validate = false;
        }
#endif

    }
}