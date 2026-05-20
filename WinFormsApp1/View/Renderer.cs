using System;
using System.Drawing;
using WinFormsApp1.Model.Entities;

namespace WinFormsApp1.View
{
    public class Renderer
    {
        private readonly Camera camera;

        private readonly Brush playerBrush = new SolidBrush(Color.Yellow);
        private readonly Brush platformBrush = new SolidBrush(Color.Gray);
        private readonly Brush enemyBrush = new SolidBrush(Color.DarkRed);
        private readonly Brush switchOffBrush = new SolidBrush(Color.Red);
        private readonly Brush switchOnBrush = new SolidBrush(Color.Green);
        private readonly Brush orbBrush = new SolidBrush(Color.Cyan);

        public Renderer(Camera camera)
        {
            this.camera = camera;
        }

        public void DrawPlayer(Graphics graphics, Player player)
        {
            if (player == null) return;
            if (!player.IsAlive) return;

            var bounds = player.GetBounds();
            var screenRect = camera.Transform(new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height));

            graphics.FillEllipse(playerBrush, screenRect.X, screenRect.Y, screenRect.Width, screenRect.Height);

            using (var eyeBrush = new SolidBrush(Color.White))
            using (var pupilBrush = new SolidBrush(Color.Black))
            {
                var eyeSize = screenRect.Width / 5;
                var leftEyeX = screenRect.X + screenRect.Width / 4;
                var rightEyeX = screenRect.X + screenRect.Width * 3 / 4 - eyeSize;
                var eyeY = screenRect.Y + screenRect.Height / 4;

                graphics.FillEllipse(eyeBrush, leftEyeX, eyeY, eyeSize, eyeSize);
                graphics.FillEllipse(eyeBrush, rightEyeX, eyeY, eyeSize, eyeSize);

                graphics.FillEllipse(pupilBrush, leftEyeX + eyeSize / 4, eyeY + eyeSize / 4, eyeSize / 2, eyeSize / 2);
                graphics.FillEllipse(pupilBrush, rightEyeX + eyeSize / 4, eyeY + eyeSize / 4, eyeSize / 2, eyeSize / 2);
            }
        }

        public void DrawPlatform(Graphics graphics, Platform platform)
        {
            if (platform == null) return;

            var bounds = platform.GetBounds();
            var screenRect = camera.Transform(new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height));

            graphics.FillRectangle(platformBrush, screenRect.X, screenRect.Y, screenRect.Width, screenRect.Height);

            if (platform.Type == Platform.PlatformType.Slippery)
            {
                using (var iceBrush = new SolidBrush(Color.LightBlue))
                {
                    graphics.FillRectangle(iceBrush, screenRect.X, screenRect.Y, screenRect.Width, 3);
                }
            }
        }

        public void DrawEnemy(Graphics graphics, Enemy enemy, Player player)
        {
            if (enemy == null) return;
            if (!enemy.IsActive) return;

            var bounds = enemy.GetBounds();
            var screenRect = camera.Transform(new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height));

            Brush enemyBrush = IsEnemyInLight(enemy, player) ? new SolidBrush(Color.Orange) : this.enemyBrush;

            graphics.FillRectangle(enemyBrush, screenRect.X, screenRect.Y, screenRect.Width, screenRect.Height);

            using (var eyeBrush = new SolidBrush(Color.White))
            using (var pupilBrush = new SolidBrush(Color.Red))
            {
                var eyeSize = screenRect.Width / 4;
                var leftEyeX = screenRect.X + screenRect.Width / 4;
                var rightEyeX = screenRect.X + screenRect.Width * 3 / 4 - eyeSize;
                var eyeY = screenRect.Y + screenRect.Height / 4;

                graphics.FillEllipse(eyeBrush, leftEyeX, eyeY, eyeSize, eyeSize);
                graphics.FillEllipse(eyeBrush, rightEyeX, eyeY, eyeSize, eyeSize);

                graphics.FillEllipse(pupilBrush, leftEyeX + eyeSize / 3, eyeY + eyeSize / 3, eyeSize / 3, eyeSize / 3);
                graphics.FillEllipse(pupilBrush, rightEyeX + eyeSize / 3, eyeY + eyeSize / 3, eyeSize / 3, eyeSize / 3);
            }
        }

        public void DrawSwitch(Graphics graphics, LightSwitch lightSwitch)
        {
            if (lightSwitch == null) return;

            var bounds = lightSwitch.GetBounds();
            var screenRect = camera.Transform(new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height));

            var brush = lightSwitch.IsActivated ? switchOnBrush : switchOffBrush;
            graphics.FillRectangle(brush, screenRect.X, screenRect.Y, screenRect.Width, screenRect.Height);

            using (var buttonBrush = new SolidBrush(Color.DarkGray))
            {
                var buttonSize = screenRect.Width / 2;
                var buttonX = screenRect.X + screenRect.Width / 4;
                var buttonY = screenRect.Y + screenRect.Height / 4;
                graphics.FillEllipse(buttonBrush, buttonX, buttonY, buttonSize, buttonSize);
            }
        }

        public void DrawOrb(Graphics graphics, EnergyOrb orb)
        {
            if (orb == null) return;
            if (orb.IsCollected) return;

            var bounds = orb.GetBounds();
            var screenRect = camera.Transform(new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height));

            using (var outerBrush = new SolidBrush(Color.FromArgb(100, 0, 255, 255)))
            using (var innerBrush = new SolidBrush(Color.Cyan))
            {
                graphics.FillEllipse(outerBrush, screenRect.X - 2, screenRect.Y - 2, screenRect.Width + 4, screenRect.Height + 4);
                graphics.FillEllipse(innerBrush, screenRect.X, screenRect.Y, screenRect.Width, screenRect.Height);
            }
        }

        public void DrawLightRadius(Graphics graphics, Player player)
        {
            if (player == null) return;
            if (!player.IsAlive) return;

            var centerX = player.X + player.Width / 2;
            var centerY = player.Y + player.Height / 2;
            var screenCenter = camera.Transform(centerX, centerY);

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
                    graphics.FillPath(brush, path);
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