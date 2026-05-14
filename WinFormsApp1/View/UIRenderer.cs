using System;
using System.Drawing;
using WinFormsApp1.Model.Entities;

namespace WinFormsApp1.View
{
    public class UIRenderer
    {
        private readonly Font _font = new Font("Arial", 16);
        private readonly Font _smallFont = new Font("Arial", 12);
        private readonly Brush _textBrush = new SolidBrush(Color.White);
        private readonly Brush _energyBgBrush = new SolidBrush(Color.DarkGray);
        private readonly Brush _energyFillBrush = new SolidBrush(Color.Green);
        private readonly Brush _energyLowBrush = new SolidBrush(Color.Red);

        public UIRenderer()
        {
        }

        public void DrawEnergyBar(Graphics g, Player player, int x, int y, int width, int height)
        {
            if (g == null || player == null) return;

            g.FillRectangle(_energyBgBrush, x, y, width, height);

            var fillWidth = (int)(width * (player.Energy / 100f));
            var fillBrush = player.Energy < 30 ? _energyLowBrush : _energyFillBrush;
            g.FillRectangle(fillBrush, x, y, fillWidth, height);

            using (var pen = new Pen(Color.White, 2))
            {
                g.DrawRectangle(pen, x, y, width, height);
            }

            var energyText = $"{Math.Round(player.Energy)}%";
            var textSize = g.MeasureString(energyText, _smallFont);
            var textX = x + width / 2 - textSize.Width / 2;
            var textY = y + height / 2 - textSize.Height / 2;
            g.DrawString(energyText, _smallFont, _textBrush, textX, textY);
        }

        public void DrawGameMessage(Graphics g, string message, float x, float y)
        {
            if (g == null || string.IsNullOrEmpty(message)) return;

            using (var bgBrush = new SolidBrush(Color.FromArgb(180, 0, 0, 0)))
            {
                var textSize = g.MeasureString(message, _font);
                var bgX = x - textSize.Width / 2 - 10;
                var bgY = y - textSize.Height / 2 - 5;
                g.FillRectangle(bgBrush, bgX, bgY, textSize.Width + 20, textSize.Height + 10);
            }

            g.DrawString(message, _font, _textBrush, x, y, new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            });
        }

        public void DrawScore(Graphics g, int score, int x, int y)
        {
            if (g == null) return;
            var scoreText = $"Счёт: {score}";
            g.DrawString(scoreText, _font, _textBrush, x, y);
        }

        public void DrawGameOver(Graphics g, float screenWidth, float screenHeight)
        {
            if (g == null) return;
            var centerX = screenWidth / 2;
            var centerY = screenHeight / 2;

            using (var bgBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 0)))
            {
                g.FillRectangle(bgBrush, 0, 0, screenWidth, screenHeight);
            }

            DrawGameMessage(g, "ИГРА ОКОНЧЕНА", centerX, centerY - 40);
            DrawGameMessage(g, "Нажмите R для перезапуска", centerX, centerY + 20);
        }

        public void DrawVictory(Graphics g, float screenWidth, float screenHeight)
        {
            if (g == null) return;
            var centerX = screenWidth / 2;
            var centerY = screenHeight / 2;

            using (var bgBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 0)))
            {
                g.FillRectangle(bgBrush, 0, 0, screenWidth, screenHeight);
            }

            DrawGameMessage(g, "ПОБЕДА!", centerX, centerY - 40);
            DrawGameMessage(g, "Нажмите R для следующего уровня", centerX, centerY + 20);
        }

        public void DrawControls(Graphics g, float screenWidth)
        {
            if (g == null) return;
            var controlsText = "A/D или ←/→ - движение | Space - прыжок";
            var textSize = g.MeasureString(controlsText, _smallFont);
            var x = screenWidth - textSize.Width - 10;
            var y = 10;

            using (var bgBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
            {
                g.FillRectangle(bgBrush, x - 5, y - 3, textSize.Width + 10, textSize.Height + 6);
            }

            g.DrawString(controlsText, _smallFont, _textBrush, x, y);
        }
    }
}