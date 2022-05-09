using DisposeGame.Components;
using DisposeGame.Scripts.Environment;
using GameEngine.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace DisposeGame.Scripts.Bonuses
{
    public class TemporaryBonus : PickableBonusScript
    {
        private List<Stopwatch> _bonusesTimers = new List<Stopwatch>();
        private int _bonusTime;

        public event ScriptHandler BonusEnd;

        public TemporaryBonus(Game3DObject picker, int bonusTime = 10) : base(picker)
        {
            _bonusTime = bonusTime;
            OnPicked += (sender, gameObject) =>
            {
                var stopwatch = new Stopwatch();
                _bonusesTimers.Add(stopwatch);
                stopwatch.Start();
            };
        }

        public void EndBonus()
        {
            BonusEnd?.Invoke(this, GameObject);
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            foreach (var timer in _bonusesTimers)
            {
                if (timer.Elapsed.TotalSeconds >= _bonusTime)
                {
                    timer.Stop();
                    BonusEnd?.Invoke(this, GameObject);
                    _bonusesTimers.Remove(timer);
                    break;
                }
            }
        }
    }
}
