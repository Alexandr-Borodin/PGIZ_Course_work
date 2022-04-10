using DisposeGame.Scripts.Bonuses;
using GameEngine.Graphics;

namespace DisposeGame.Scripts.Obstacles
{
    public class ObstacleScript : PickableBonusScript
    {
        public ObstacleScript(Game3DObject picker) : base(picker)
        {
            OnPicked += _ => { int v = 5 + 5; };
        }
    }
}
