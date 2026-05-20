using WinFormsApp1.Model.Entities;
using WinFormsApp1.Model.World;
using WinFormsApp1.Services;

namespace WinFormsApp1.Controller
{
    public class GameController
    {
        public Player Player => player;
        public Level CurrentLevel => currentLevel;
        public GameState GameState => gameState;

        private LevelLoader levelLoader;
        private Level currentLevel;
        private GameState gameState;
        private Player player;
        private InputHandler inputHandler;
        private float gravity;
        private float jumpPower = 15f;

        public GameController(InputHandler inputHandler)
        {
            this.inputHandler = inputHandler;
            levelLoader = new LevelLoader();
            var playerData = levelLoader.GetPlayersData(0);
            player = new Player(playerData[0], playerData[1], playerData[2], playerData[3], playerData[4], playerData[5]);
            gameState = new GameState();
            currentLevel = levelLoader.LoadLevel(0);
            gravity = 0.8f;
        }

        public void Update()
        {
            if (!gameState.IsPlaying())
                return;

            HandleInput();
            UpdatePlayerPhysics();
            CheckPlatformCollisionAndAdjust();
            UpdateEnergyAndLight();
            UpdateEnemies();
            CheckEnemyCollision();
            CollectEnergyOrbs();
            ActivateSwitches();
            CheckVictory();
        }

        public void HandleInput()
        {
            if (inputHandler.IsLeftPressed())
                player.MoveLeft();
            else if (inputHandler.IsRightPressed())
                player.MoveRight();
            else
                player.StopMove();

            if (inputHandler.IsJumpPressed() && player.IsGrounded)
                player.Jump(jumpPower);
        }

        public void UpdatePlayerPhysics()
        {
            player.ApplyGravity(gravity);
            player.UpdatePosition();
        }

        public void UpdateEnemies()
        {
            foreach (var enemy in currentLevel.Enemies)
            {
                SetEnemyVisible(enemy);
                UpdateEnemyBehavior(enemy);
                enemy.UpdatePosition();
            }
        }

        private void SetEnemyVisible(Enemy enemy)
        {
            if (enemy == null || player == null) return;

            var listOfCorners = new[]
            {
                new Point((int)enemy.X, (int)enemy.Y),
                new Point((int)(enemy.X + enemy.Width), (int)enemy.Y),
                new Point((int)enemy.X, (int)(enemy.Y + enemy.Height)),
                new Point((int)(enemy.X + enemy.Width), (int)(enemy.Y + enemy.Height))
            };

            enemy.IsVisible = listOfCorners.Any(corner =>
            {
                var dx = player.X - corner.X;
                var dy = player.Y - corner.Y;
                var distance = Math.Sqrt(dx * dx + dy * dy);
                return distance < player.LightRadius;
            });
        }

        private void UpdateEnemyBehavior(Enemy enemy)
        {
            if (enemy == null || player == null) return;

            var enemyCenterX = enemy.X + enemy.Width / 2;
            var enemyCenterY = enemy.Y + enemy.Height / 2;

            var playerCenterX = player.X + player.Width / 2;
            var playerCenterY = player.Y + player.Height / 2;

            var dx = playerCenterX - enemyCenterX;
            var dy = playerCenterY - enemyCenterY;
            var distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance <= player.LightRadius)
            {
                if (playerCenterX < enemyCenterX)
                    enemy.MoveLeft();
                else if (playerCenterX > enemyCenterX)
                    enemy.MoveRight();
            }
            else
            {
                enemy.StopMove();
            }
        }

        public void CheckPlatformCollisionAndAdjust()
        {
            var collisionInfo = CollisionService.CheckPlatformCollision(player, currentLevel);

            if (collisionInfo.Direction == ControllerDirection.Direction.None)
                return;

            switch (collisionInfo.Direction)
            {
                case ControllerDirection.Direction.Top:
                    player.Y += collisionInfo.AdjustY;
                    player.IsGrounded = true;
                    player.VelocityY = 0;
                    break;
                case ControllerDirection.Direction.Bottom:
                    player.Y += collisionInfo.AdjustY;
                    player.VelocityY = 0;
                    break;
                case ControllerDirection.Direction.Left:
                    player.X += collisionInfo.AdjustX;
                    player.VelocityX = 0;
                    break;
                case ControllerDirection.Direction.Right:
                    player.X += collisionInfo.AdjustX;
                    player.VelocityX = 0;
                    break;
            }
        }

        public void UpdateEnergyAndLight()
        {
            player.UpdateEnergy(0.2f);
            player.UpdateLightRadius(50, 100);
        }

        public void CheckEnemyCollision()
        {
            var enemyHits = CollisionService.GetCollisions(player, currentLevel.Enemies);
            if (enemyHits.Any())
                gameState.GameOver();
        }

        public void CollectEnergyOrbs()
        {
            var collectedOrbs = CollisionService.GetCollisions(player, currentLevel.EnergyOrbs);
            foreach (var orb in collectedOrbs.ToList())
            {
                player.AddEnergy(orb.EnergyAmount);
                currentLevel.RemoveEnergyOrb(orb);
            }
        }

        public void ActivateSwitches()
        {
            var hitSwitches = CollisionService.GetCollisions(player, currentLevel.LightSwitches);
            foreach (var switchObj in hitSwitches)
                switchObj.Activate();
        }

        public void CheckVictory()
        {
            var allSwitchesActivated = currentLevel.LightSwitches.All(s => s.IsActivated);
            if (allSwitchesActivated)
                NextLevel();
        }

        public void NextLevel()
        {
            gameState.LevelCompleted();
            if (gameState.IsVictory())
                return;

            currentLevel = levelLoader.LoadLevel(gameState.CurrentLevelIndex);
            var playerData = levelLoader.GetPlayersData(gameState.CurrentLevelIndex);
            player = new Player(playerData[0], playerData[1], playerData[2], playerData[3], playerData[4], playerData[5]);
        }

        public void RestartLevel()
        {
            currentLevel = levelLoader.LoadLevel(gameState.CurrentLevelIndex);
            var playerData = levelLoader.GetPlayersData(gameState.CurrentLevelIndex);
            player = new Player(playerData[0], playerData[1], playerData[2], playerData[3], playerData[4], playerData[5]);
            gameState.ResumeGame();
        }

        public void GameOver()
        {
            gameState.GameOver();
        }
    }
}