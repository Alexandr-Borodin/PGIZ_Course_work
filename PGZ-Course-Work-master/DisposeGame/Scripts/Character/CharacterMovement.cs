using GameEngine.Animation;
using GameEngine.Collisions;
using GameEngine.Graphics;
using SharpDX;
using System.Collections.Generic;

namespace DisposeGame.Scripts.Character
{
    public class CharacterMovement
    {
        private float _speed;
        private Animation _animation;
        private bool _isAnimationPaused;


        public CharacterMovement(Animation animation, float speed)
        {
            _speed = speed;
            _animation = animation;
            _animation.Pause();
            _isAnimationPaused = true;
        }

        public void Move(Game3DObject character, Vector3 direction, float delta)
        {
            if (direction == Vector3.Zero)
            {
                if (!_isAnimationPaused)
                {
                    _isAnimationPaused = true;
                    _animation.Pause();
                }
                return;
            }
            if (_isAnimationPaused)
            {
                _animation.Restart();
                _isAnimationPaused = false;
            }

            direction.Normalize();
            Vector3 rotation = character.Rotation;
            Matrix rotationMatrix = Matrix.RotationYawPitchRoll(rotation.Z, rotation.Y, rotation.X);
            MoveWithCollisionCheck(character, (Vector3)Vector3.Transform(direction * _speed * delta, rotationMatrix));
            
        }

        private void MoveWithCollisionCheck(Game3DObject character, Vector3 direction)
        {
            character.MoveBy(direction);

            var partX = direction * Vector3.UnitX;
            var partY = direction * Vector3.UnitY;
            var partZ = direction * Vector3.UnitZ;
        }
    }
}
