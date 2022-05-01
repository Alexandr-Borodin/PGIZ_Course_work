using GameEngine.Game;
using GameEngine.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisposeGame.Scripts.Character.Player
{
    public class HelpCubeMovementScript : Script
    {
        private InputController _inputController;
        private float _mouseSensitivity;

        public HelpCubeMovementScript(float mouseSensitivity = 0.25f)
        {
            _mouseSensitivity = mouseSensitivity;
            _inputController = InputController.GetInstance();

        }

        public override void Update(float delta)
        {
            if (_inputController.MouseUpdate)
            {
                GameObject.RotateZ(delta * _mouseSensitivity * _inputController.MouseRelativePositionX);
            }
        }
    }
}
