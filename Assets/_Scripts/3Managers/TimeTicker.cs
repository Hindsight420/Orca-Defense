using System;
using UnityEngine;

namespace Assets._Scripts._3Managers
{
    public class TimeTicker : MonoBehaviour
    {
        [SerializeField]
        private float TICK_INTERVAL_SECOND = 0.2f;
        private float _tickTimer;

        public static int CurrentTick { get; private set; }
        public const int START_OF_THE_GAME = 0;
        public static event EventHandler<int> OnTick;

        private void Awake()
        {
            CurrentTick = 0;
        }

        private void Update()
        {
            _tickTimer += Time.deltaTime;
            if (_tickTimer >= TICK_INTERVAL_SECOND)
            {
                CurrentTick++;
                _tickTimer -= TICK_INTERVAL_SECOND;

                OnTick?.Invoke(this, CurrentTick);
            }
        }

        public static int GetInnerTick(int startTick) => CurrentTick - startTick;
    }
}
