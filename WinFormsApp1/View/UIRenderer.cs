using System.Drawing;
using WinFormsApp1.Model.Entities;

namespace WinFormsApp1.View
{
    public class UIRenderer
    {
        private readonly Font font = new Font("Arial", 16);
        private readonly Font smallFont = new Font("Arial", 12);
        private readonly Brush textBrush = new SolidBrush(Color.White);
        private readonly Brush energyBgBrush = new SolidBrush(Color.DarkGray);
        private readonly Brush energyFillBrush = new SolidBrush(Color.Green);
        private readonly Brush energyLowBrush = new SolidBrush(Color.Red);

        public UIRenderer()
        {
        }

        public void DrawEnergyBar(Graphics graphics, Player player, int x, int y, int width, int height)
        {
            if (player == null) return;

            graphics.FillRectangle(energyBgBrush, x, y, width, height);

            var fillWidth = (int)(width * (player.Energy / 100f));
            var fillBrush = player.Energy < 30 ? energyLowBrush : energyFillBrush;
            graphics.FillRectangle(fillBrush, x, y, fillWidth, height);

            using (var pen = new Pen(Color.White, 2))
            {
                graphics.DrawRectangle(pen, x, y, width, height);
            }

            var energyText = $"{Mathf.RoundToInt(player.Energy)}%";
            var textSize = graphics.MeasureString(energyText, smallFont);
            var textX = x + width / 2 - textSize.Width / 2;
            var textY = y + height / 2 - textSize.Height / 2;
            graphics.DrawString(energyText, smallFont, textBrush, textX, textY);
        }

        public void DrawGameMessage(Graphics graphics, string message, float x, float y)
        {
            if (string.IsNullOrEmpty(message)) return;

            using (var bgBrush = new SolidBrush(Color.FromArgb(180, 0, 0, 0)))
            {
                var textSize = graphics.MeasureString(message, font);
                var bgX = x - textSize.Width / 2 - 10;
                var bgY = y - textSize.Height / 2 - 5;
                graphics.FillRectangle(bgBrush, bgX, bgY, textSize.Width + 20, textSize.Height + 10);
            }

            graphics.DrawString(message, font, textBrush, x, y, new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            });
        }

        public void DrawScore(Graphics graphics, int score, int x, int y)
        {
            var scoreText = $"Счёт: {score}";
            graphics.DrawString(scoreText, font, textBrush, x, y);
        }

        public void DrawGameOver(Graphics graphics, float screenWidth, float screenHeight)
        {
            var centerX = screenWidth / 2;
            var centerY = screenHeight / 2;

            using (var bgBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 0)))
            {
                graphics.FillRectangle(bgBrush, 0, 0, screenWidth, screenHeight);
            }

            DrawGameMessage(graphics, "ИГРА ОКОНЧЕНА", centerX, centerY - 40);
            DrawGameMessage(graphics, "Нажмите R для перезапуска", centerX, centerY + 20);
        }

        public void DrawVictory(Graphics graphics, float screenWidth, float screenHeight)
        {
            var centerX = screenWidth / 2;
            var centerY = screenHeight / 2;

            using (var bgBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 0)))
            {
                graphics.FillRectangle(bgBrush, 0, 0, screenWidth, screenHeight);
            }

            DrawGameMessage(graphics, "ПОБЕДА!", centerX, centerY - 40);
            DrawGameMessage(graphics, "Нажмите R для следующего уровня", centerX, centerY + 20);
        }

        public void DrawControls(Graphics graphics, float screenWidth)
        {
            var controlsText = "A/D или ←/→ - движение | Space - прыжок";
            var textSize = graphics.MeasureString(controlsText, smallFont);
            var x = screenWidth - textSize.Width - 10;
            var y = 10;

            using (var bgBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
            {
                graphics.FillRectangle(bgBrush, x - 5, y - 3, textSize.Width + 10, textSize.Height + 6);
            }

            graphics.DrawString(controlsText, smallFont, textBrush, x, y);
        }
    }

    public static class Mathf
    {
        public static int RoundToInt(float value)
        {
            return (int)System.Math.Round(value);
        }
    }
}