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
    public class MainMenuScene : Scene
    {
        protected override UIElement InitializeUI(Loader loader, DrawingContext context, int screenWidth, int screenHeight)
        {
            context.NewNinePartsBitmap("glowingBorder", loader.LoadBitmapFromFile(@"Textures\GlowingBorder.png"), 21, 29, 21, 29);
            context.NewBitmap("backgroundBitmap", loader.LoadBitmapFromFile(@"C:\Учёба\3-ий курс\2-ой семестр\Course work\Repository\PGIZ_Course_work\DisposeGame\Textures\mainbg.jpg"));
            context.NewSolidBrush("whiteBrush", new RawColor4(1f, 1f, 1f, 1f));
            context.NewTextFormat("textFormat", textAlignment: TextAlignment.Center, paragraphAlignment: ParagraphAlignment.Center);

            var ui = new UISequentialContainer(Vector2.Zero, new Vector2(screenWidth, screenHeight))
            {
                MainAxis = UISequentialContainer.Alignment.Center,
                CrossAxis = UISequentialContainer.Alignment.Center,
                Background = new TextureBackground("backgroundBitmap")
            };
            var menu = new UISequentialContainer(Vector2.Zero, new Vector2(250, 500))
            {
                MainAxis = UISequentialContainer.Alignment.End,
                CrossAxis = UISequentialContainer.Alignment.Center,
            };
            ui.Add(menu);

            var startText = new UIText("Start", new Vector2(200, 52), "textFormat", "whiteBrush");
            var quitText = new UIText("Quit", new Vector2(200, 52), "textFormat", "whiteBrush");

            var startButton = new UIButton(startText) 
            { 
                ReleasedBackground = new NinePartsTextureBackground("glowingBorder"), 
                PressedBackground = new NinePartsTextureBackground("glowingBorder")
            };
            var quitButton = new UIButton(quitText)
            {
                ReleasedBackground = new NinePartsTextureBackground("glowingBorder"),
                PressedBackground = new NinePartsTextureBackground("glowingBorder")
            };

            startButton.OnClicked += () =>
            {
                Game.ChangeScene(new GameScene());
            };
            quitButton.OnClicked += () =>
            {
                Game.CloseGame();
            };

            menu.Add(startButton);
            menu.Add(quitButton);

            return ui;
        }
    }
}
