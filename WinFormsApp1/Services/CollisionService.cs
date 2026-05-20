using System.Collections.Generic;
using System.Linq;
using WinFormsApp1.Controller;
using WinFormsApp1.Model.Entities;
using WinFormsApp1.Model.World;
using WinFormsApp2.Model.Entities;

namespace WinFormsApp1.Services
{
    public static class CollisionService
    {
        public static ControllerDirection.CollisionInfo CheckPlatformCollision(Player player, Level level)
        {
            // Текущая (уже обновлённая) позиция игрока
            var currTop = player.Y;
            var currLeft = player.X;
            var currRight = player.X + player.Width;
            var currBottom = player.Y + player.Height;

            // Предыдущая позиция игрока (до последнего перемещения)
            var prevTop = player.Y - player.VelocityY;
            var prevLeft = player.X - player.VelocityX;
            var prevRight = prevLeft + player.Width;
            var prevBottom = prevTop + player.Height;

            foreach (var platform in level.Platforms)
            {
                var platformLeft = platform.X;
                var platformRight = platform.X + platform.Width;
                var platformTop = platform.Y;
                var platformBottom = platform.Y + platform.Height;

                // Если игрок до и после перемещения полностью слева/справа/сверху/снизу от платформы — пропускаем
                var conditionWithoutCollision = new List<bool>
                {
                    prevRight < platformLeft && currRight < platformLeft,
                    prevLeft > platformRight && currLeft > platformRight,
                    prevBottom < platformTop && currBottom < platformTop,
                    prevTop > platformBottom && currTop > platformBottom
                };

                if (conditionWithoutCollision.Any(x => x))
                    continue;

                // Проверяем перекрытие по X и Y по комбинации предыдущей/текущей позиции
                var xOverlap = (currRight > platformLeft && currLeft < platformRight) ||
                               (prevRight > platformLeft && prevLeft < platformRight);
                var yOverlap = (currBottom > platformTop && currTop < platformBottom) ||
                               (prevBottom > platformTop && prevTop < platformBottom);

                if (xOverlap)
                {
                    // Удар снизу -> приземление на платформу
                    if (prevBottom <= platformTop && currBottom >= platformTop)
                    {
                        float adjustY = platformTop - currBottom;
                        return new ControllerDirection.CollisionInfo(ControllerDirection.Direction.Top, 0, adjustY);
                    }

                    // Удар сверху -> столкновение с нижней стороной платформы
                    if (prevTop >= platformBottom && currTop <= platformBottom)
                    {
                        float adjustY = platformBottom - currTop;
                        return new ControllerDirection.CollisionInfo(ControllerDirection.Direction.Bottom, 0, adjustY);
                    }
                }

                if (yOverlap)
                {
                    // Столкновение слева
                    if (prevRight <= platformLeft && currRight >= platformLeft)
                    {
                        float adjustX = platformLeft - currRight;
                        return new ControllerDirection.CollisionInfo(ControllerDirection.Direction.Left, adjustX, 0);
                    }

                    // Столкновение справа
                    if (prevLeft >= platformRight && currLeft <= platformRight)
                    {
                        float adjustX = platformRight - currLeft;
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