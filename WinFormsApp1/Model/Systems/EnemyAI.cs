using System;
using WinFormsApp1.Model.Entities;

namespace WinFormsApp1.Model.Systems
{
    public class EnemyAI
    {
        public float Speed { get; set; } = 2f;

        public EnemyAI()
        {
        }

        public EnemyAI(float speed)
        {
            Speed = speed;
        }

        public void Update(Enemy enemy, Player player, float deltaTime)
        {
            if (enemy == null) return;
            if (player == null) return;

            var isInLight = IsEnemyInLight(enemy, player);

            if (!isInLight)
            {
                MoveTowardsPlayer(enemy, player, deltaTime);
            }
        }

        public bool IsEnemyInLight(Enemy enemy, Player player)
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

        private void MoveTowardsPlayer(Enemy enemy, Player player, float deltaTime)
        {
            if (enemy == null || player == null) return;

            var enemyCenterX = enemy.X + enemy.Width / 2;
            var enemyCenterY = enemy.Y + enemy.Height / 2;

            var playerCenterX = player.X + player.Width / 2;
            var playerCenterY = player.Y + player.Height / 2;

            var dx = playerCenterX - enemyCenterX;
            var dy = playerCenterY - enemyCenterY;
            var length = (float)Math.Sqrt(dx * dx + dy * dy);

            if (length > 0.01f)
            {
                var moveX = (dx / length) * Speed * deltaTime * 60f;
                var moveY = (dy / length) * Speed * deltaTime * 60f;

                enemy.X += moveX;
                enemy.Y += moveY;
            }
        }
    }
}