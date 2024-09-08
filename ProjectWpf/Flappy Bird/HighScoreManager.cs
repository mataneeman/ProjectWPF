using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ProjectWpf.Flappy_Bird
{
    public class HighScoreManager
    {
        private readonly string filePath;
        private List<int> highScores = new List<int>(); 

        public HighScoreManager(string gameIdentifier)
        {
            filePath = $"{gameIdentifier}_highscores.json";
            LoadHighScores();
        }

        public void AddScore(int score)
        {
            highScores.Add(score);
            highScores = highScores.OrderByDescending(s => s).Take(5).ToList();
            SaveHighScores();
        }

        public List<int> GetTopScores()
        {
            return highScores;
        }

        private void LoadHighScores()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                highScores = JsonSerializer.Deserialize<List<int>>(json) ?? new List<int>();
            }
        }

        private void SaveHighScores()
        {
            string json = JsonSerializer.Serialize(highScores);
            File.WriteAllText(filePath, json);
        }
    }
}
