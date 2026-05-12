using System.Drawing;
using WinFormsApp1.Model.Entities;

namespace WinFormsApp1.View
{
    public class UIRenderer
    {
        private readonly Graphics _graphics;
        private readonly Font _font = new Font("Arial", 16);
        private readonly Font _smallFont = new Font("Arial", 12);
        private readonly Brush _textBrush = new SolidBrush(Color.White);
        private readonly Brush _energyBgBrush = new SolidBrush(Color.DarkGray);
        private readonly Brush _energyFillBrush = new SolidBrush(Color.Green);
        private readonly Brush _energyLowBrush = new SolidBrush(Color.Red);

        public UIRenderer(Graphics graphics)
        {
            _graphics = graphics;
        }

        public void DrawEnergyBar(Player player, int x, int y, int width, int height)
        {
            if (player == null) return;

            // Фон полоски энергии
            _graphics.FillRectangle(_energyBgBrush, x, y, width, height);

            // Заполнение в зависимости от энергии
            var fillWidth = (int)(width * (player.Energy / 100f));
            var fillBrush = player.Energy < 30 ? _energyLowBrush : _energyFillBrush;
            _graphics.FillRectangle(fillBrush, x, y, fillWidth, height);

            // Рамка
            using (var pen = new Pen(Color.White, 2))
            {
                _graphics.DrawRectangle(pen, x, y, width, height);
            }

            // Текст с процентом энергии
            var energyText = $"{Mathf.RoundToInt(player.Energy)}%";
            var textSize = _graphics.MeasureString(energyText, _smallFont);
            var textX = x + width / 2 - textSize.Width / 2;
            var textY = y + height / 2 - textSize.Height / 2;
            _graphics.DrawString(energyText, _smallFont, _textBrush, textX, textY);
        }

        public void DrawGameMessage(string message, float x, float y)
        {
            if (string.IsNullOrEmpty(message)) return;

            // Фон для сообщения
            using (var bgBrush = new SolidBrush(Color.FromArgb(180, 0, 0, 0)))
            {
                var textSize = _graphics.MeasureString(message, _font);
                var bgX = x - textSize.Width / 2 - 10;
                var bgY = y - textSize.Height / 2 - 5;
                _graphics.FillRectangle(bgBrush, bgX, bgY, textSize.Width + 20, textSize.Height + 10);
            }

            _graphics.DrawString(message, _font, _textBrush, x, y, new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            });
        }

        public void DrawScore(int score, int x, int y)
        {
            var scoreText = $"Счёт: {score}";
            _graphics.DrawString(scoreText, _font, _textBrush, x, y);
        }

        public void DrawGameOver(float screenWidth, float screenHeight)
        {
            var centerX = screenWidth / 2;
            var centerY = screenHeight / 2;

            using (var bgBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 0)))
            {
                _graphics.FillRectangle(bgBrush, 0, 0, screenWidth, screenHeight);
            }

            DrawGameMessage("ИГРА ОКОНЧЕНА", centerX, centerY - 40);
            DrawGameMessage("Нажмите R для перезапуска", centerX, centerY + 20);
        }

        public void DrawVictory(float screenWidth, float screenHeight)
        {
            var centerX = screenWidth / 2;
            var centerY = screenHeight / 2;

            using (var bgBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 0)))
            {
                _graphics.FillRectangle(bgBrush, 0, 0, screenWidth, screenHeight);
            }

            DrawGameMessage("ПОБЕДА!", centerX, centerY - 40);
            DrawGameMessage("Нажмите R для следующего уровня", centerX, centerY + 20);
        }

        public void DrawControls(float screenWidth)
        {
            var controlsText = "A/D или ←/→ - движение | Space - прыжок";
            var textSize = _graphics.MeasureString(controlsText, _smallFont);
            var x = screenWidth - textSize.Width - 10;
            var y = 10;

            using (var bgBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
            {
                _graphics.FillRectangle(bgBrush, x - 5, y - 3, textSize.Width + 10, textSize.Height + 6);
            }

            _graphics.DrawString(controlsText, _smallFont, _textBrush, x, y);
        }
    }

    // Вспомогательный класс для округления
    public static class Mathf
    {
        public static int RoundToInt(float value)
        {
            return (int)System.Math.Round(value);
        }
    }
}