using GameEngine.Graphics;
using GameEngine.Scripts;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisposeGame.Scripts.Environment
{
    public delegate void ScriptHandler(Script sender, Game3DObject game3DObject);

    public class EnvironmentScript : Script
    {
        private Vector3 _direction;
        private float _destroyBorder;

        public event ScriptHandler Destroy;

        public float Speed { get; set; }

        public EnvironmentScript(Vector3 direction, float speed, float destroyBorder)
        {
            _direction = direction;
            Speed = speed;
            _destroyBorder = destroyBorder;
        }

        public override void Update(float delta)
        {
            GameObject.MoveBy(_direction * GameObject.Speed);

            if (GameObject.Position.Z <= _destroyBorder)
            {
                Destroy?.Invoke(this, GameObject);
            }
        }
    }
}
