using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ProjectWpf.Snack
{
    public class HighScoresManager
    {
        private const string FileName = "highscores.json";
        public List<int> HighScores { get; private set; }

        public HighScoresManager()
        {
            HighScores = LoadHighScores(); // אתחול של HighScores
        }

        private List<int> LoadHighScores()
        {
            if (!File.Exists(FileName))
                return new List<int>();

            string json = File.ReadAllText(FileName);
            return JsonSerializer.Deserialize<List<int>>(json) ?? new List<int>();
        }

        public void SaveHighScores()
        {
            string json = JsonSerializer.Serialize(HighScores);
            File.WriteAllText(FileName, json);
        }

        public void AddScore(int score)
        {
            HighScores.Add(score);
            HighScores.Sort((a, b) => b.CompareTo(a)); // Sort in descending order

            // Keep only the top 5 scores
            if (HighScores.Count > 5)
                HighScores = HighScores.Take(5).ToList();

            SaveHighScores();
        }

    }

}

