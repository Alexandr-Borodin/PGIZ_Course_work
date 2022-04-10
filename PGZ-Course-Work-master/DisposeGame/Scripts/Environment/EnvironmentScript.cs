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
        private float _destroyBorder;
        private float _speed;

        public EnvironmentScript(Vector3 direction, float speed, float destroyBorder)
        {
            _direction = direction;
            _speed = speed;
            _destroyBorder = destroyBorder;
        }

        public override void Update(float delta)
        {
            GameObject.MoveBy(_direction * _speed);

            if (GameObject.Position.Y >= _destroyBorder)
            {
                GameObject.Scene.RemoveGameObject(GameObject);
            }
        }
    }
}
