using System;
using System.Drawing;
using WinFormsApp1.Model.Entities;

namespace WinFormsApp1.View
{
    public class Renderer
    {
        private readonly Camera _camera;
        private readonly Graphics _graphics;

        private readonly Brush _playerBrush = new SolidBrush(Color.Yellow);
        private readonly Brush _platformBrush = new SolidBrush(Color.Gray);
        private readonly Brush _enemyBrush = new SolidBrush(Color.DarkRed);
        private readonly Brush _switchOffBrush = new SolidBrush(Color.Red);
        private readonly Brush _switchOnBrush = new SolidBrush(Color.Green);
        private readonly Brush _orbBrush = new SolidBrush(Color.Cyan);

        public Renderer(Graphics graphics, Camera camera)
        {
            _graphics = graphics;
            _camera = camera;
        }

        public void DrawPlayer(Player player)
        {
            if (player == null) return;
            if (!player.IsAlive) return;

            var bounds = player.GetBounds();
            var screenRect = _camera.Transform(new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height));

            _graphics.FillEllipse(_playerBrush, screenRect.X, screenRect.Y, screenRect.Width, screenRect.Height);

            using (var eyeBrush = new SolidBrush(Color.White))
            using (var pupilBrush = new SolidBrush(Color.Black))
            {
                var eyeSize = screenRect.Width / 5;
                var leftEyeX = screenRect.X + screenRect.Width / 4;
                var rightEyeX = screenRect.X + screenRect.Width * 3 / 4 - eyeSize;
                var eyeY = screenRect.Y + screenRect.Height / 4;

                _graphics.FillEllipse(eyeBrush, leftEyeX, eyeY, eyeSize, eyeSize);
                _graphics.FillEllipse(eyeBrush, rightEyeX, eyeY, eyeSize, eyeSize);

                _graphics.FillEllipse(pupilBrush, leftEyeX + eyeSize / 4, eyeY + eyeSize / 4, eyeSize / 2, eyeSize / 2);
                _graphics.FillEllipse(pupilBrush, rightEyeX + eyeSize / 4, eyeY + eyeSize / 4, eyeSize / 2, eyeSize / 2);
            }
        }

        public void DrawPlatform(Platform platform)
        {
            if (platform == null) return;

            var bounds = platform.GetBounds();
            var screenRect = _camera.Transform(new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height));

            _graphics.FillRectangle(_platformBrush, screenRect.X, screenRect.Y, screenRect.Width, screenRect.Height);

            if (platform.IsSlippery)
            {
                using (var iceBrush = new SolidBrush(Color.LightBlue))
                {
                    _graphics.FillRectangle(iceBrush, screenRect.X, screenRect.Y, screenRect.Width, 3);
                }
            }
        }

        public void DrawEnemy(Enemy enemy, Player player)
        {
            if (enemy == null) return;
            if (!enemy.IsActive) return;

            var bounds = enemy.GetBounds();
            var screenRect = _camera.Transform(new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height));

            Brush enemyBrush = IsEnemyInLight(enemy, player) ? new SolidBrush(Color.Orange) : _enemyBrush;

            _graphics.FillRectangle(enemyBrush, screenRect.X, screenRect.Y, screenRect.Width, screenRect.Height);

            using (var eyeBrush = new SolidBrush(Color.White))
            using (var pupilBrush = new SolidBrush(Color.Red))
            {
                var eyeSize = screenRect.Width / 4;
                var leftEyeX = screenRect.X + screenRect.Width / 4;
                var rightEyeX = screenRect.X + screenRect.Width * 3 / 4 - eyeSize;
                var eyeY = screenRect.Y + screenRect.Height / 4;

                _graphics.FillEllipse(eyeBrush, leftEyeX, eyeY, eyeSize, eyeSize);
                _graphics.FillEllipse(eyeBrush, rightEyeX, eyeY, eyeSize, eyeSize);

                _graphics.FillEllipse(pupilBrush, leftEyeX + eyeSize / 3, eyeY + eyeSize / 3, eyeSize / 3, eyeSize / 3);
                _graphics.FillEllipse(pupilBrush, rightEyeX + eyeSize / 3, eyeY + eyeSize / 3, eyeSize / 3, eyeSize / 3);
            }
        }

        public void DrawSwitch(LightSwitch lightSwitch)
        {
            if (lightSwitch == null) return;

            var bounds = lightSwitch.GetBounds();
            var screenRect = _camera.Transform(new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height));

            var brush = lightSwitch.IsActivated ? _switchOnBrush : _switchOffBrush;
            _graphics.FillRectangle(brush, screenRect.X, screenRect.Y, screenRect.Width, screenRect.Height);

            using (var buttonBrush = new SolidBrush(Color.DarkGray))
            {
                var buttonSize = screenRect.Width / 2;
                var buttonX = screenRect.X + screenRect.Width / 4;
                var buttonY = screenRect.Y + screenRect.Height / 4;
                _graphics.FillEllipse(buttonBrush, buttonX, buttonY, buttonSize, buttonSize);
            }
        }

        public void DrawOrb(EnergyOrb orb)
        {
            if (orb == null) return;
            if (orb.IsCollected) return;

            var bounds = orb.GetBounds();
            var screenRect = _camera.Transform(new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height));

            using (var outerBrush = new SolidBrush(Color.FromArgb(100, 0, 255, 255)))
            using (var innerBrush = new SolidBrush(Color.Cyan))
            {
                _graphics.FillEllipse(outerBrush, screenRect.X - 2, screenRect.Y - 2, screenRect.Width + 4, screenRect.Height + 4);
                _graphics.FillEllipse(innerBrush, screenRect.X, screenRect.Y, screenRect.Width, screenRect.Height);
            }
        }

        public void DrawLightRadius(Player player)
        {
            if (player == null) return;
            if (!player.IsAlive) return;

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
                    _graphics.FillPath(brush, path);
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
    }
}