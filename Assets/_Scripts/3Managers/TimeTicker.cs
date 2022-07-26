using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts._3Managers
{
    public class TimeTicker : MonoBehaviour
    {
        public static event EventHandler<int> OnTick;
        public static readonly int START_OF_THE_GAME = 0;

        private const float TICK_TIMER_MAX = 0.2f;

        private static int tick;
        private float tickTimer;

        public static int CurrentTick { get => tick; }

        private void Awake()
        {
            tick = 0;
        }

        private void Update()
        {
            tickTimer += Time.deltaTime;
            if (tickTimer >= TICK_TIMER_MAX)
            {
                tick++;
                tickTimer -= TICK_TIMER_MAX;

                OnTick?.Invoke(this, tick);
            }
        }

        public static int GetInnerTick(int startTick) => tick - startTick;
    }
}
