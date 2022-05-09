using DisposeGame.Components;
using GameEngine.Graphics;

namespace DisposeGame.Scripts.Bonuses
{
    public class HealthBonusScript : PickableBonusScript
    {
        public HealthBonusScript(Game3DObject picker, int heal = 10) : base(picker)
        {
            OnPicked += (sender, gameObject) => picker.GetComponent<HealthComponent>().Heal(heal);
        }
    }
}
