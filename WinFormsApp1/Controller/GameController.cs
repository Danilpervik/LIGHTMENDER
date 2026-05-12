using WinFormsApp1.Model.Entities;
using WinFormsApp1.Model.Physics;
using WinFormsApp1.Model.World;
using static WinFormsApp1.Model.Physics.CollisionDetector;

namespace WinFormsApp1.Controller;
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
            enemy.SetVisible(player);
            enemy.SeekPlayer(player);
            enemy.UpdatePosition();
        }
    }

    public void CheckPlatformCollisionAndAdjust() 
    { 
        var collisionResult = CollisionDetector.CheckPlatformCollision(player, currentLevel);
        if (collisionResult.Direction == CollisionDirection.None)
            return;
        switch (collisionResult.Direction)
        {
            case CollisionDirection.Top:
                player.Y = collisionResult.Platform.Y - player.Height;
                player.IsGrounded = true;
                player.VelocityY = 0;
                break;
            case CollisionDirection.Bottom:
                player.Y = collisionResult.Platform.Y + collisionResult.Platform.Height;
                player.VelocityY = 0;
                break;
            case CollisionDirection.Left:
                player.X = collisionResult.Platform.X - player.Width;
                player.VelocityX = 0;
                break;
            case CollisionDirection.Right:
                player.X = collisionResult.Platform.X + collisionResult.Platform.Width;
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
        if (CollisionDetector.CheckEnemyCollision(player, currentLevel))
            gameState.GameOver();
    }
    
    public void CollectEnergyOrbs() 
    { 
        var collectedOrbs = CollisionDetector.GetEnergyOrbCollision(player, currentLevel);
        foreach (var orb in collectedOrbs)
        {
            player.AddEnergy(orb.EnergyAmount);
            currentLevel.EnergyOrbs.Remove(orb);
        }
    }
    
    public void ActivateSwitches() 
    { 
        foreach (var switchObj in CollisionDetector.GetLightSwitchCollision(player, currentLevel))
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
