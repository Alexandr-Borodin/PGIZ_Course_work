using GameEngine.Collisions;
using GameEngine.Graphics;
using GameEngine.Scripts;
using SharpDX;
using System;

namespace DisposeGame.Scripts.Environment
{
    public class EnvironmentScript : Script
    {
        private Vector3 _direction;
        private float _speed;

        public EnvironmentScript(Vector3 direction, float speed)
        {
            _direction = direction;
            _speed = speed;
        }

        public override void Update(float delta)
        {
            GameObject.MoveBy(_direction * _speed);
        }
    }
}
