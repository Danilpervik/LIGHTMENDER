using System;
using System.Drawing;
using System.Collections.Generic;
using WinFormsApp1.Model.Entities;

namespace WinFormsApp1.View
{
    public class Renderer
    {
        private readonly Camera _camera;

        private readonly Brush _playerBrush = new SolidBrush(Color.Yellow);
        private readonly Brush _platformBrush = new SolidBrush(Color.Gray);
        private readonly Brush _enemyBrush = new SolidBrush(Color.DarkRed);
        private readonly Brush _switchOffBrush = new SolidBrush(Color.Red);
        private readonly Brush _switchOnBrush = new SolidBrush(Color.Green);
        private readonly Brush _orbBrush = new SolidBrush(Color.Cyan);

        public Renderer(Camera camera)
        {
            _camera = camera;
        }

        public void Clear(Graphics g, Color color)
        {
            if (g == null) return;
            g.Clear(color);
        }

        public void DrawPlayer(Graphics g, Player player)
        {
            if (g == null || player == null) return;

            var bounds = player.GetBounds();
            var screenRect = _camera.Transform(new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height));

            g.FillEllipse(_playerBrush, screenRect.X, screenRect.Y, screenRect.Width, screenRect.Height);

            using (var eyeBrush = new SolidBrush(Color.White))
            using (var pupilBrush = new SolidBrush(Color.Black))
            {
                var eyeSize = screenRect.Width / 5;
                var leftEyeX = screenRect.X + screenRect.Width / 4;
                var rightEyeX = screenRect.X + screenRect.Width * 3 / 4 - eyeSize;
                var eyeY = screenRect.Y + screenRect.Height / 4;

                g.FillEllipse(eyeBrush, leftEyeX, eyeY, eyeSize, eyeSize);
                g.FillEllipse(eyeBrush, rightEyeX, eyeY, eyeSize, eyeSize);

                g.FillEllipse(pupilBrush, leftEyeX + eyeSize / 4, eyeY + eyeSize / 4, eyeSize / 2, eyeSize / 2);
                g.FillEllipse(pupilBrush, rightEyeX + eyeSize / 4, eyeY + eyeSize / 4, eyeSize / 2, eyeSize / 2);
            }
        }
        public void DrawDarkness(Graphics g, Player player, int screenWidth, int screenHeight)
        {
            if (g == null || player == null) return;

            var centerX = player.X + player.Width / 2;
            var centerY = player.Y + player.Height / 2;
            var screenCenter = _camera.Transform(centerX, centerY);
            var lightRadius = player.LightRadius;

            // Сохраняем состояние
            var state = g.Save();

            // Устанавливаем обрезку ВНЕ круга (всё, что за пределами круга)
            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                // Добавляем круг
                path.AddEllipse(
                    screenCenter.X - lightRadius,
                    screenCenter.Y - lightRadius,
                    lightRadius * 2,
                    lightRadius * 2
                );

                // Инвертируем обрезку: теперь видно всё, КРОМЕ круга
                g.SetClip(path, System.Drawing.Drawing2D.CombineMode.Exclude);
            }

            // Заливаем ВСЁ, что вне круга, чёрным цветом
            using (var blackBrush = new SolidBrush(Color.Black))
            {
                g.FillRectangle(blackBrush, 0, 0, screenWidth, screenHeight);
            }

            // Восстанавливаем обрезку
            g.Restore(state);
        }

        public void DrawPlatform(Graphics g, Platform platform)
        {
            if (g == null || platform == null) return;

            var bounds = platform.GetBounds();
            var screenRect = _camera.Transform(new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height));

            g.FillRectangle(_platformBrush, screenRect.X, screenRect.Y, screenRect.Width, screenRect.Height);

            if (platform.Type == Platform.PlatformType.Slippery)
            {
                using (var iceBrush = new SolidBrush(Color.LightBlue))
                {
                    g.FillRectangle(iceBrush, screenRect.X, screenRect.Y, screenRect.Width, 3);
                }
            }
        }

        public void DrawEnemy(Graphics g, Enemy enemy, Player player)
        {
            if (g == null || enemy == null) return;
            if (!enemy.IsActive) return;

            var bounds = enemy.GetBounds();
            var screenRect = _camera.Transform(new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height));

            Brush enemyBrush = IsEnemyInLight(enemy, player) ? new SolidBrush(Color.Orange) : _enemyBrush;

            g.FillRectangle(enemyBrush, screenRect.X, screenRect.Y, screenRect.Width, screenRect.Height);

            using (var eyeBrush = new SolidBrush(Color.White))
            using (var pupilBrush = new SolidBrush(Color.Red))
            {
                var eyeSize = screenRect.Width / 4;
                var leftEyeX = screenRect.X + screenRect.Width / 4;
                var rightEyeX = screenRect.X + screenRect.Width * 3 / 4 - eyeSize;
                var eyeY = screenRect.Y + screenRect.Height / 4;

                g.FillEllipse(eyeBrush, leftEyeX, eyeY, eyeSize, eyeSize);
                g.FillEllipse(eyeBrush, rightEyeX, eyeY, eyeSize, eyeSize);

                g.FillEllipse(pupilBrush, leftEyeX + eyeSize / 3, eyeY + eyeSize / 3, eyeSize / 3, eyeSize / 3);
                g.FillEllipse(pupilBrush, rightEyeX + eyeSize / 3, eyeY + eyeSize / 3, eyeSize / 3, eyeSize / 3);
            }
        }

        public void DrawSwitch(Graphics g, LightSwitch lightSwitch)
        {
            if (g == null || lightSwitch == null) return;

            var bounds = lightSwitch.GetBounds();
            var screenRect = _camera.Transform(new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height));

            var brush = lightSwitch.IsActivated ? _switchOnBrush : _switchOffBrush;
            g.FillRectangle(brush, screenRect.X, screenRect.Y, screenRect.Width, screenRect.Height);

            using (var buttonBrush = new SolidBrush(Color.DarkGray))
            {
                var buttonSize = screenRect.Width / 2;
                var buttonX = screenRect.X + screenRect.Width / 4;
                var buttonY = screenRect.Y + screenRect.Height / 4;
                g.FillEllipse(buttonBrush, buttonX, buttonY, buttonSize, buttonSize);
            }
        }

        public void DrawOrb(Graphics g, EnergyOrb orb)
        {
            if (g == null || orb == null) return;
            if (orb.IsCollected) return;

            var bounds = orb.GetBounds();
            var screenRect = _camera.Transform(new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height));

            using (var outerBrush = new SolidBrush(Color.FromArgb(100, 0, 255, 255)))
            using (var innerBrush = new SolidBrush(Color.Cyan))
            {
                g.FillEllipse(outerBrush, screenRect.X - 2, screenRect.Y - 2, screenRect.Width + 4, screenRect.Height + 4);
                g.FillEllipse(innerBrush, screenRect.X, screenRect.Y, screenRect.Width, screenRect.Height);
            }
        }

        public void DrawLightRadius(Graphics g, Player player)
        {
            if (g == null || player == null) return;

            var centerX = player.X + player.Width / 2;
            var centerY = player.Y + player.Height / 2;
            var screenCenter = _camera.Transform(centerX, centerY);

            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddEllipse(screenCenter.X - player.LightRadius,
                               screenCenter.Y - player.LightRadius,
                               player.LightRadius * 2,
                               player.LightRadius * 2);

                using (var brush = new System.Drawing.Drawing2D.PathGradientBrush(path))
                {
                    brush.CenterColor = Color.FromArgb(120, 255, 255, 150);
                    brush.SurroundColors = new Color[] { Color.FromArgb(0, 0, 0, 0) };
                    g.FillPath(brush, path);
                }
            }
        }

        private bool IsEnemyInLight(Enemy enemy, Player player)
        {
            if (enemy == null || player == null) return false;

            var enemyCenterX = enemy.X + enemy.Width / 2;
            var enemyCenterY = enemy.Y + enemy.Height / 2;

            var playerCenterX = player.X + player.Width / 2;
            var playerCenterY = player.Y + player.Height / 2;

            var dx = enemyCenterX - playerCenterX;
            var dy = enemyCenterY - playerCenterY;
            var distance = (float)Math.Sqrt(dx * dx + dy * dy);

            return distance <= player.LightRadius;
        }

        // Методы для отрисовки списков
        public void DrawPlatforms(Graphics g, List<Platform> platforms)
        {
            if (g == null || platforms == null) return;
            foreach (var platform in platforms)
                DrawPlatform(g, platform);
        }

        public void DrawEnemies(Graphics g, List<Enemy> enemies, Player player)
        {
            if (g == null || enemies == null) return;
            foreach (var enemy in enemies)
                DrawEnemy(g, enemy, player);
        }

        public void DrawLightSwitches(Graphics g, List<LightSwitch> switches)
        {
            if (g == null || switches == null) return;
            foreach (var switchItem in switches)
                DrawSwitch(g, switchItem);
        }

        public void DrawEnergyOrbs(Graphics g, List<EnergyOrb> orbs)
        {
            if (g == null || orbs == null) return;
            foreach (var orb in orbs)
                DrawOrb(g, orb);
        }
    }
}