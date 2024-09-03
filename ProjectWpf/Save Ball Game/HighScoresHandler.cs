using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ProjectWpf.Save_Ball_Game
{
    public class HighScoresHandler
    {
        private readonly string highScoresFilePath;

        public HighScoresHandler(string identifier)
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SaveBallGame");
            Directory.CreateDirectory(folderPath);
            highScoresFilePath = Path.Combine(folderPath, $"highscores_{identifier}.json");
        }

        public void SaveHighScore(int score)
        {
            List<int> highScores = LoadHighScores();
            highScores.Add(score);
            highScores.Sort((a, b) => b.CompareTo(a));
            File.WriteAllText(highScoresFilePath, JsonSerializer.Serialize(highScores));
        }

        public List<int> GetTopHighScores(int topN)
        {
            List<int> highScores = LoadHighScores();
            return highScores.GetRange(0, Math.Min(topN, highScores.Count));
        }

        private List<int> LoadHighScores()
        {
            if (!File.Exists(highScoresFilePath))
            {
                return new List<int>();
            }

            string jsonString = File.ReadAllText(highScoresFilePath);
            return JsonSerializer.Deserialize<List<int>>(jsonString) ?? new List<int>();
        }
    }
}
