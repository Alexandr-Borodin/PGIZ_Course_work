using AmazingUILibrary;
using AmazingUILibrary.Backgrounds;
using AmazingUILibrary.Containers;
using AmazingUILibrary.Drawing;
using AmazingUILibrary.Elements;
using GameEngine.Game;
using SharpDX;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;

namespace DisposeGame.Scenes
{
    public class WinMenuScene : Scene
    {
        protected override UIElement InitializeUI(Loader loader, DrawingContext context, int screenWidth, int screenHeight)
        {
            context.NewNinePartsBitmap("glowingBorder", loader.LoadBitmapFromFile(@"Textures\GlowingBorder.png"), 21, 29, 21, 29);
            context.NewBitmap("backgroundBitmap", loader.LoadBitmapFromFile(@"Textures\mainbg.jpg"));
            context.NewSolidBrush("whiteBrush", new RawColor4(1f, 1f, 1f, 1f));
            context.NewTextFormat("textFormat", textAlignment: TextAlignment.Center, paragraphAlignment: ParagraphAlignment.Center);

            var ui = new UISequentialContainer(Vector2.Zero, new Vector2(screenWidth, screenHeight))
            {
                MainAxis = UISequentialContainer.Alignment.Center,
                CrossAxis = UISequentialContainer.Alignment.Center,
                Background = new TextureBackground("backgroundBitmap")
            };
            var menu = new UISequentialContainer(Vector2.Zero, new Vector2(150, 180))
            {
                MainAxis = UISequentialContainer.Alignment.Center,
                CrossAxis = UISequentialContainer.Alignment.Center,
                Background = new NinePartsTextureBackground("glowingBorder")
            };
            ui.Add(menu);

            var restartText = new UIText("Restart", new Vector2(120, 52), "textFormat", "whiteBrush");
            var mainMenuText = new UIText("Main Menu", new Vector2(120, 52), "textFormat", "whiteBrush");

            var restartButton = new UIButton(restartText) 
            { 
                ReleasedBackground = new NinePartsTextureBackground("glowingBorder"), 
                PressedBackground = new NinePartsTextureBackground("glowingBorder")
            };
            var mainMenuButton = new UIButton(mainMenuText)
            {
                ReleasedBackground = new NinePartsTextureBackground("glowingBorder"),
                PressedBackground = new NinePartsTextureBackground("glowingBorder")
            };

            restartButton.OnClicked += () =>
            {
                Game.ChangeScene(new GameScene());
            };
            mainMenuButton.OnClicked += () =>
            {
                Game.ChangeScene(new MainMenuScene());
            };

            menu.Add(new UIText("Win!", new Vector2(120, 52), "textFormat", "whiteBrush"));
            menu.Add(restartButton);
            menu.Add(mainMenuButton);

            return ui;
        }
    }
}
