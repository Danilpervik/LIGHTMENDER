using WinFormsApp1.Model.Entities;
using WinFormsApp1.Model.World;

namespace WinFormsApp1.Controller;
public class GameController
{
    public LevelLoader LevelLoader { get; set; }
    public Level CurrentLevel { get; set; }
    public GameState GameState { get; set; }
    public Player Player { get; set; }
    public InputHandler InputHandler { get; set; }

    public GameController()
    {
        _levelLoader = new LevelLoader();
    }
    public Level LoadLevel(int levelIndex)
    {
        return _levelLoader.LoadLevel(levelIndex);
    }
    public float[] GetPlayerData(int playerIndex)
    {
        return _levelLoader.GetPlayersData(playerIndex);
    }
}
