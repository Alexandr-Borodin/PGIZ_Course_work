using DisposeGame.Components;
using DisposeGame.Scenes;
using GameEngine.Animation;
using GameEngine.Game;
using GameEngine.Graphics;
using GameEngine.Scripts;
using SharpDX;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DisposeGame.Scripts.Character
{
    public class PlayerMovementScript : KeyboardListenerScript
    {
        private const float _rotationBorder = 0.4f;

        public event Action OnJump;

        private CharacterMovement _movement;
        private InputController _inputController;
        private Vector3 _moveDirection;
        private float _mouseSensitivity;

        private float _leftRotationOffset;
        private float _rightRotationOffset;

        private bool _isSpeedUp;
        private bool _isSpeedDown;

        public PlayerMovementScript(Animation animation, PhysicsComponent physics, List<Game3DObject> walls, float speed = 30f, float jump = 30, float mouseSensitivity = 0.25f)
        {
            var pauseMenuScene = new PauseMenuScene();

            _mouseSensitivity = mouseSensitivity;

            Actions.Add(Key.W, delta =>
            {
                //_moveDirection += Vector3.UnitZ;

                if (!_isSpeedUp)
                {
                    GameObject.Scene.GameObjects.ForEach(gameObject => gameObject.Speed *= 1.5f);
                    _isSpeedUp = true;
                }
            });
            Actions.Add(Key.S, delta =>
            {
                //_moveDirection -= Vector3.UnitZ;

                if (!_isSpeedDown)
                {
                    GameObject.Scene.GameObjects.ForEach(gameObject => gameObject.Speed /= 1.5f);
                    _isSpeedDown = true;
                }
            });
            Actions.Add(Key.A, delta =>
            {
                _moveDirection -= Vector3.UnitX;

                if (_leftRotationOffset > -_rotationBorder)
                {
                    RotateCar(-0.03f);
                    _leftRotationOffset += -0.03f;
                }
            });
            Actions.Add(Key.D, delta =>
            {
                _moveDirection += Vector3.UnitX;

                if (_rightRotationOffset < _rotationBorder)
                {
                    RotateCar(0.03f);
                    _rightRotationOffset += 0.03f;
                }
            });
            Actions.Add(Key.Escape, delta => GameObject.Scene.Game.ChangeScene(pauseMenuScene));
            Actions.Add(Key.Space, delta =>
            {
                if (GameObject.Position.Y < 0.01)
                {
                    physics.AddImpulse(Vector3.UnitY * jump);
                    OnJump?.Invoke();
                }
            });

            _inputController = InputController.GetInstance();

            _movement = new CharacterMovement(animation, speed/*, walls*/);

        }

        public override void Update(float delta)
        {
            base.Update(delta);

            if (!_inputController.IsPressed(Key.D) && _rightRotationOffset > 0)
            {
                RotateCar(-0.03f);
                _rightRotationOffset += -0.03f;
            }

            if (!_inputController.IsPressed(Key.A) && _leftRotationOffset < 0)
            {
                RotateCar(0.03f);
                _leftRotationOffset += 0.03f;
            }

            if (!_inputController.IsPressed(Key.W) && _isSpeedUp)
            {
                _isSpeedUp = false;
                GameObject.Scene.GameObjects.ForEach(gameObject => gameObject.Speed /= 1.5f);
            }

            if (!_inputController.IsPressed(Key.S) && _isSpeedDown)
            {
                _isSpeedDown = false;
                GameObject.Scene.GameObjects.ForEach(gameObject => gameObject.Speed *= 1.5f);
            }
        }

        private void RotateCar(float rotationIncrement)
        {
            GameObject.Children.ForEach(child => child.RotateZ(rotationIncrement));
            GameObject.Children.Last().RotateZ(rotationIncrement * -1);
        }

        protected override void BeforeKeyProcess(float delta)
        {
            _moveDirection = Vector3.Zero;
        }

        protected override void AfterKeyProcess(float delta)
        {
            _movement.Move(GameObject, _moveDirection, delta);
        }
    }
}
