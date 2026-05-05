using WinFormsApp2.Model.Entities;

namespace WinFormsApp1.Model.Entities
{
    internal class Enemy : EnemyAndPlayer
    {
        public bool IsVisible { get; set; }
        public bool IsActive
        {
            get { return IsVisible; }
        }

        public Enemy(double x, double y, double width, double height, double velocityX, double speed) : base(x, y, width, height, velocityX, speed)
        {
            IsVisible = false;
        }

        private double CalculateDistanceToPlayer(Player player, Point point)
        {
            var deltaX = player.X - point.X;
            var deltaY = player.Y - point.Y;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        public void SeekPlayer(Player player)
        {
            if (IsVisible)
            {
                if (player.X < this.X)
                    MoveLeft();
                else if (player.X > this.X)
                    MoveRight();
            }
        }

        public void SetVisible(Player player)
        {
            var listOfCorners = new List<Point>
            {
                new Point((int)this.X, (int)this.Y),
                new Point((int)this.X + (int)this.Width, (int)this.Y),
                new Point((int)this.X, (int)this.Y + (int)this.Height),
                new Point((int)this.X + (int)this.Width, (int)this.Y + (int)this.Height)
            };
            this.IsVisible = listOfCorners.Any(corner => CalculateDistanceToPlayer(player, corner) < player.LightRadius);
        }

        public void UpdatePosition()
        {
            this.X += this.VelocityX;
        }
    }
}