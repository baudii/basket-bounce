using UnityEngine;
using UnityEngine.Events;

namespace BasketBounce.Systems
{
    public class EventInitializer : MonoBehaviour
    {
        [SerializeField] UnityEvent onAwake;
        [SerializeField] UnityEvent onStart;
        [SerializeField] UnityEvent onEnable;

        void Awake()
        {
            onAwake?.Invoke();
        }

        void Start()
        {
            onStart?.Invoke();
        }

        void OnEnable()
        {
            onEnable?.Invoke();
        }
    }
}
