using GameEngine.Components;

namespace DisposeGame.Components
{
    public class ObstacleInfoComponent : ObjectComponent
    {
        public bool IsFree { get; set; }

        public float OriginSpeed { get; set; }
    }
}
