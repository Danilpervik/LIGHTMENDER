using System.Xml.Serialization;
using WinFormsApp1.Model.Entities;
using WinFormsApp1.Model.World;

namespace WinFormsApp1.Model.Physics
{   
    internal class CollisionDetector
    {
        internal enum CollisionDirection
        {
            None,
            Top,
            Bottom,
            Left,
            Right
        }

        internal static CollisionDirection CheckPlatformCollision(Player player, Level level)
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
                        return CollisionDirection.Top;
                    if (playerTop >= platformBottom && playerNewTop <= platformBottom)
                        return CollisionDirection.Bottom;
                }
                if (yCollision)
                {
                    if (playerRight <= platformLeft && playerNewRight >= platformLeft)
                        return CollisionDirection.Left;
                    if (playerLeft >= platformRight && playerNewLeft <= platformRight)
                        return CollisionDirection.Right;
                }
            }
            
            return CollisionDirection.None;
        }


    }
}
