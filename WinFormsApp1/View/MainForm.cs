using System;
using System.Drawing;
using System.Windows.Forms;
using WinFormsApp1.Controller;
using WinFormsApp1.View;

namespace WinFormsApp1.View
{
    public partial class MainForm : Form
    {
        private GameLoopTimer gameTimer;
        private InputHandler inputHandler;
        private GameController gameController;
        private Camera _camera;
        private Renderer _renderer;
        private UIRenderer _uiRenderer;

        public MainForm()
        {
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.BackColor = Color.Black;
            this.ClientSize = new Size(800, 500);
            this.Text = "Lightmender - Чинитель Света";
            this.StartPosition = FormStartPosition.CenterScreen;

            this.KeyDown += MainForm_KeyDown;
            this.KeyUp += MainForm_KeyUp;
            this.Paint += MainForm_Paint;

            this.Activated += (s, e) => { this.Focus(); };
            this.Focus();
            this.Activate();

            inputHandler = new InputHandler();
            gameController = new GameController(inputHandler);
            gameController.GameState.StartGame();
            _camera = new Camera(this.ClientSize.Width, this.ClientSize.Height);

            _renderer = new Renderer(_camera);
            _uiRenderer = new UIRenderer();

            gameTimer = new GameLoopTimer();
            gameTimer.SetOnTick(GameLoop);
            gameTimer.Start();
        }

        private void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {
            inputHandler.KeyDown(e.KeyCode);

            if (e.KeyCode == Keys.R)
            {
                if (gameController.GameState.IsGameOver() || gameController.GameState.IsVictory())
                {
                    gameController.RestartLevel();
                }
            }
        }

        private void MainForm_KeyUp(object? sender, KeyEventArgs e)
        {
            inputHandler.KeyUp(e.KeyCode);
        }

        private void GameLoop()
        {
            gameController.Update();
            this.Invalidate();
        }

        private void MainForm_Paint(object? sender, PaintEventArgs e)
        {
            if (e.Graphics == null) return;

            Graphics g = e.Graphics;

            if (gameController.Player != null)
            {
                var playerCenterX = gameController.Player.X + gameController.Player.Width / 2;
                var playerCenterY = gameController.Player.Y + gameController.Player.Height / 2;
                _camera.Follow(playerCenterX, playerCenterY);
            }

            // 1. Чёрный фон
            _renderer.Clear(g, Color.Black);

            int screenWidth = this.ClientSize.Width;
            int screenHeight = this.ClientSize.Height;

            if (gameController.GameState.IsGameOver())
            {
                _uiRenderer.DrawGameOver(g, screenWidth, screenHeight);
                return;
            }

            if (gameController.GameState.IsVictory())
            {
                _uiRenderer.DrawVictory(g, screenWidth, screenHeight);
                return;
            }

            // 2. Рисуем ВСЕ объекты
            _renderer.DrawPlatforms(g, gameController.CurrentLevel.Platforms);
            _renderer.DrawEnemies(g, gameController.CurrentLevel.Enemies, gameController.Player);
            _renderer.DrawLightSwitches(g, gameController.CurrentLevel.LightSwitches);
            _renderer.DrawEnergyOrbs(g, gameController.CurrentLevel.EnergyOrbs);
            _renderer.DrawPlayer(g, gameController.Player);

            // 3. НАКЛАДЫВАЕМ ТЕМНОТУ (внутри круга света будет видно)
            _renderer.DrawDarkness(g, gameController.Player, screenWidth, screenHeight);

            // 4. UI поверх всего
            _uiRenderer.DrawEnergyBar(g, gameController.Player, 20, 20, 200, 25);
            _uiRenderer.DrawScore(g, gameController.GameState.CurrentLevelIndex + 1, 20, 55);
            _uiRenderer.DrawControls(g, screenWidth);
        }
    }
}