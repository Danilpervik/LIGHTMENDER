using WinFormsApp1.Model.Entities;
using WinFormsApp1.Model.World;
using WinFormsApp2.Model.Entities;

namespace WinFormsApp1.Model.Physics;

public class CollisionDetector
{
    public struct CollisionResult
    {
        public CollisionDirection Direction;
        public Platform Platform;

        public CollisionResult(CollisionDirection direction, Platform platform)
        {
            Direction = direction;
            Platform = platform;
        }
    }

    public enum CollisionDirection
    {
        None,
        Top,
        Bottom,
        Left,
        Right
    }

    public static CollisionResult CheckPlatformCollision(Player player, Level level)
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

            // Проверка: есть ли пересечение вообще
            if (playerNewRight <= platformLeft ||
                playerNewLeft >= platformRight ||
                playerNewBottom <= platformTop ||
                playerNewTop >= platformBottom)
            {
                continue;  // Нет столкновения
            }

            // Исправлено: правильное определение горизонтального пересечения
            bool xCollision = playerNewRight > platformLeft && playerNewLeft < platformRight;
            bool yCollision = playerNewBottom > platformTop && playerNewTop < platformBottom;

            // Проверка столкновения сверху (Top)
            if (playerBottom <= platformTop && playerNewBottom >= platformTop && xCollision)
            {
                return new CollisionResult(CollisionDirection.Top, platform);
            }

            // Проверка столкновения снизу (Bottom)
            if (playerTop >= platformBottom && playerNewTop <= platformBottom && xCollision)
            {
                return new CollisionResult(CollisionDirection.Bottom, platform);
            }

            // Проверка столкновения слева (Left)
            if (playerRight <= platformLeft && playerNewRight >= platformLeft && yCollision)
            {
                return new CollisionResult(CollisionDirection.Left, platform);
            }

            // Проверка столкновения справа (Right)
            if (playerLeft >= platformRight && playerNewLeft <= platformRight && yCollision)
            {
                return new CollisionResult(CollisionDirection.Right, platform);
            }
        }

        return new CollisionResult(CollisionDirection.None, null);
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
        return GetIntersectingObjects(player, level.EnergyOrbs.Cast<GameObject>().ToList()).Cast<EnergyOrb>().ToList();
    }

    public static List<LightSwitch> GetLightSwitchCollision(Player player, Level level)
    {
        return GetIntersectingObjects(player, level.LightSwitches.Cast<GameObject>().ToList()).Cast<LightSwitch>().ToList();
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

