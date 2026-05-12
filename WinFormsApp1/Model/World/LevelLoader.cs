using WinFormsApp1.Model.Entities;
using static WinFormsApp1.Model.Entities.Platform;

namespace WinFormsApp1.Model.World;

public class LevelLoader
{   
    public string LevelsFolder { get; set; } = "Levels";
    public string PlayersFolder { get; set; } = "DataOfPlayers";
    public float[] GetPlayersData(int playerIndex) 
    {
        var levelFilePath = string.Format("{0}/Player{1}.txt", PlayersFolder, playerIndex);
        if (!File.Exists(levelFilePath))
            throw new FileNotFoundException($"Файл игрока не найден: {levelFilePath}");
        var lines = File.ReadAllLines(levelFilePath);
        var linesWithoutCommentsAndWhitespace = lines.
            Select(line => line.Trim()).
            Where(x => x[0] != '#')
            .Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        var playerData = linesWithoutCommentsAndWhitespace[0].Split(',', StringSplitOptions.RemoveEmptyEntries);
        return new float[]
        {
            float.Parse(playerData[0]),
            float.Parse(playerData[1]),
            float.Parse(playerData[2]),
            float.Parse(playerData[3]),
            float.Parse(playerData[4]),
            float.Parse(playerData[5])
        };
    }
    public Level LoadLevel(int levelIndex)
    {   
        if (levelIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(levelIndex), "Номер уровня не может быть отрицательным");
        int totalLevels = Directory.GetFiles(LevelsFolder, "*.txt").Length;
        if (levelIndex >= totalLevels)
            throw new ArgumentOutOfRangeException(nameof(levelIndex), "Номер уровня превышает доступные уровни.");
        var levelFilePath = string.Format("{0}/level{1}.txt", LevelsFolder, levelIndex);
        if (!File.Exists(levelFilePath))
        {
            throw new FileNotFoundException($"Файл уровня не найден: {levelFilePath}");
        }
        var level = new Level();
        var lines = File.ReadAllLines(levelFilePath);
        var linesWithoutCommentsAndWhitespace = lines.
            Select(line => line.Trim()).
            Where(x => x[0] != '#')
            .Where(x => !string.IsNullOrWhiteSpace(x));
        foreach (var line in linesWithoutCommentsAndWhitespace)
        {   
            var tokens = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
            switch (tokens[0].ToLower())
            {
                case "platform":
                    var typeValue = int.Parse(tokens[5]);
                    var platformType = (PlatformType)typeValue;
                    var platform = new Platform
                    (
                        float.Parse(tokens[1]),
                        float.Parse(tokens[2]),
                        float.Parse(tokens[3]),
                        float.Parse(tokens[4]),
                        platformType
                    );
                    level.AddPlatform(platform);
                    break;
                case "enemy":
                    var enemy = new Enemy
                    (
                        float.Parse(tokens[1]),
                        float.Parse(tokens[2]),
                        float.Parse(tokens[3]),
                        float.Parse(tokens[4]),
                        float.Parse(tokens[5]),
                        float.Parse(tokens[6])
                    );
                    level.AddEnemy(enemy);
                    break;
                case "energy_orb":
                    var energyOrb = new EnergyOrb
                    (
                        float.Parse(tokens[1]),
                        float.Parse(tokens[2]),
                        float.Parse(tokens[3]),
                        float.Parse(tokens[4]),
                        float.Parse(tokens[5])
                    );
                    level.AddEnergyOrb(energyOrb);
                    break;
                case "light_switch":
                    var lightSwitch = new LightSwitch
                    (
                        float.Parse(tokens[1]),
                        float.Parse(tokens[2]),
                        float.Parse(tokens[3]),
                        float.Parse(tokens[4]),
                        float.Parse(tokens[5])
                    );
                    level.AddLightSwitch(lightSwitch);
                    break;
            }
        }
        return level;
    }
}
