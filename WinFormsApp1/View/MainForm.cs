using System;
using System.Drawing;
using System.Windows.Forms;
using WinFormsApp1.Controller;
using WinFormsApp1.View;

namespace WinFormsApp1.View
{
    public partial class MainForm : Form
    {
        private Timer gameTimer;
        private InputHandler inputHandler;
        private GameController gameController;
        private Renderer renderer;
        private UIRenderer uiRenderer;
        
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
            
            inputHandler = new InputHandler();
            gameController = new GameController(inputHandler);
            
            gameTimer = new Timer();
            gameTimer.Interval = 16;
            gameTimer.Tick += GameLoop;
            gameTimer.Start();
        }
        
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            inputHandler.KeyDown(e.KeyCode);
        }
        
        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            inputHandler.KeyUp(e.KeyCode);
        }
        
        private void GameLoop(object sender, EventArgs e)
        {
            gameController.Update();
            this.Invalidate();
        }
        
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            renderer = new Renderer(e.Graphics);
            uiRenderer = new UIRenderer(e.Graphics);
            
            renderer.Clear(Color.Black);
            
            if (gameController.GameState.IsGameOver())
            {
                uiRenderer.DrawGameOver();
                return;
            }
            
            if (gameController.GameState.IsVictory())
            {
                uiRenderer.DrawVictory();
                return;
            }
            
            renderer.DrawPlatforms(gameController.CurrentLevel.Platforms);
            renderer.DrawEnemies(gameController.CurrentLevel.Enemies);
            renderer.DrawLightSwitches(gameController.CurrentLevel.LightSwitches);
            renderer.DrawEnergyOrbs(gameController.CurrentLevel.EnergyOrbs);
            renderer.DrawPlayer(gameController.Player);
            
            uiRenderer.DrawEnergyBar(gameController.Player.Energy);
            uiRenderer.DrawLevelInfo(gameController.GameState.CurrentLevelIndex + 1);
        }
    }
}