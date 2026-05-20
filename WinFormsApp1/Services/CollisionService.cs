using WinFormsApp1.Controller;
using WinFormsApp1.Model.Entities;
using WinFormsApp1.Model.World;

namespace WinFormsApp1.Services
{
    public static class CollisionService
    {
        public static ControllerDirection.CollisionInfo CheckPlatformCollision(Player player, Level level)
        {
            var playerBottom = player.Y + player.Height;
            var playerTop = player.Y;
            var playerLeft = player.X;
            var playerRight = player.X + player.Width;
            var playerNewTop = player.Y + player.VelocityY;
            var playerNewLeft = player.X + player.VelocityX;
            var playerNewRight = playerRight + player.VelocityX;
            var playerNewBottom = playerBottom + player.VelocityY;

            foreach (var platform in level.Platforms)
            {
                var platformLeft = platform.X;
                var platformRight = platform.X + platform.Width;
                var platformTop = platform.Y;
                var platformBottom = platform.Y + platform.Height;

                var conditionWithoutCollision = new List<bool>
                {
                    playerNewRight < platformLeft,
                    playerNewLeft > platformRight,
                    playerNewBottom < platformTop,
                    playerNewTop > platformBottom
                };

                var xCollision = playerNewLeft >= platformLeft || playerNewRight <= platformRight;
                var yCollision = playerNewBottom > platformTop && playerNewTop < platformBottom;

                if (conditionWithoutCollision.Any(x => x))
                    continue;

                if (xCollision)
                {
                    if (playerBottom <= platformTop && playerNewBottom >= platformTop)
                    {
                        float adjustY = platformTop - playerBottom;
                        return new ControllerDirection.CollisionInfo(ControllerDirection.Direction.Top, 0, adjustY);
                    }
                    if (playerTop >= platformBottom && playerNewTop <= platformBottom)
                    {
                        float adjustY = platformBottom - playerTop;
                        return new ControllerDirection.CollisionInfo(ControllerDirection.Direction.Bottom, 0, adjustY);
                    }
                }

                if (yCollision)
                {
                    if (playerRight <= platformLeft && playerNewRight >= platformLeft)
                    {
                        float adjustX = platformLeft - playerRight;
                        return new ControllerDirection.CollisionInfo(ControllerDirection.Direction.Left, adjustX, 0);
                    }
                    if (playerLeft >= platformRight && playerNewLeft <= platformRight)
                    {
                        float adjustX = platformRight - playerLeft;
                        return new ControllerDirection.CollisionInfo(ControllerDirection.Direction.Right, adjustX, 0);
                    }
                }
            }

            return new ControllerDirection.CollisionInfo(ControllerDirection.Direction.None, 0, 0);
        }

        public static bool CheckEnemyCollision(Player player, Level level)
        {
            var playerRectangle = player.GetBounds();
            foreach (var enemy in level.Enemies)
            {
                var enemyRectangle = enemy.GetBounds();
                if (playerRectangle.IntersectsWith(enemyRectangle))
                    return true;
            }
            return false;
        }

        public static List<EnergyOrb> GetEnergyOrbCollision(Player player, Level level)
        {
            var intersectingObjects = GetIntersectingObjects(player, level.EnergyOrbs.Cast<GameObject>().ToList());
            return intersectingObjects.Cast<EnergyOrb>().ToList();
        }

        public static List<LightSwitch> GetLightSwitchCollision(Player player, Level level)
        {
            var intersectingObjects = GetIntersectingObjects(player, level.LightSwitches.Cast<GameObject>().ToList());
            return intersectingObjects.Cast<LightSwitch>().ToList();
        }

        private static List<GameObject> GetIntersectingObjects(Player player, List<GameObject> objects)
        {
            var intersectingObjects = new List<GameObject>();
            var playerRectangle = player.GetBounds();

            foreach (var obj in objects)
            {
                var objRectangle = obj.GetBounds();
                if (playerRectangle.IntersectsWith(objRectangle))
                    intersectingObjects.Add(obj);
            }

            return intersectingObjects;
        }
    }
}