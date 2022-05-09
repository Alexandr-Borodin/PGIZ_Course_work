using GameEngine.Graphics;
using GameEngine.Scripts;
using SharpDX.DirectInput;

namespace DisposeGame.Scripts
{
    public class CameraFovController : KeyboardListenerScript
    {
        public CameraFovController(GameEngine.Graphics.Camera camera)
        {
            Actions.Add(Key.Z, delta => camera.FOVY += 0.05f);
            Actions.Add(Key.X, delta => camera.FOVY -= 0.05f);
        }
    }
}
