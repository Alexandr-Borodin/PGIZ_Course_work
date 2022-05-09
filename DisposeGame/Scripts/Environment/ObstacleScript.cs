using DisposeGame.Components;
using DisposeGame.Scripts.Bonuses;
using GameEngine.Collisions;
using GameEngine.Graphics;
using GameEngine.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisposeGame.Scripts.Environment
{
    public class ObstacleScript : Script
    {
        private Game3DObject _picker;

        public event ScriptHandler OnPicked;

        public ObstacleScript(Game3DObject picker, int dammage)
        {
            _picker = picker;
            OnPicked += (sender, gameObject) => picker.GetComponent<HealthComponent>().DealDamage(dammage);
        }

        public override void Update(float delta)
        {
            if (ObjectCollision.Intersects(_picker.Collision, GameObject.Collision))
            {
                OnPicked?.Invoke(this, GameObject);
            }
        }
    }
}
