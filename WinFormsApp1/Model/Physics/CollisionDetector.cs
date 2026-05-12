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
                    return new CollisionResult(CollisionDirection.Top, platform);
                if (playerTop >= platformBottom && playerNewTop <= platformBottom)
                    return new CollisionResult(CollisionDirection.Bottom, platform);
            }
            if (yCollision)
            {
                if (playerRight <= platformLeft && playerNewRight >= platformLeft)
                    return new CollisionResult(CollisionDirection.Left, platform);
                if (playerLeft >= platformRight && playerNewLeft <= platformRight)
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

