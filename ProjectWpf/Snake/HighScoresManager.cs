using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace ProjectWpf.Snack
{
    public class HighScoresManager
    {
        private const string FileName = "highscores.json";
        private Dictionary<string, List<int>> highScores;

        public HighScoresManager()
        {
            highScores = LoadHighScores(); // אתחול של HighScores
        }

        private Dictionary<string, List<int>> LoadHighScores()
        {
            if (!File.Exists(FileName))
                return new Dictionary<string, List<int>>();

            try
            {
                string json = File.ReadAllText(FileName);
                return JsonSerializer.Deserialize<Dictionary<string, List<int>>>(json) ?? new Dictionary<string, List<int>>();
            }
            catch (JsonException)
            {
                // במידה ויש בעיה בקריאת ה-JSON, נתחיל עם מילון ריק
                return new Dictionary<string, List<int>>();
            }
        }

        public void SaveHighScores()
        {
            try
            {
                string json = JsonSerializer.Serialize(highScores);
                File.WriteAllText(FileName, json);
            }
            catch (IOException ex)
            {
                // טיפול בשגיאות של כתיבה לקובץ אם נדרש
                Console.WriteLine($"Error saving high scores: {ex.Message}");
            }
        }

        public void AddScore(int score, string difficulty)
        {
            if (!highScores.ContainsKey(difficulty))
            {
                highScores[difficulty] = new List<int>();
            }

            highScores[difficulty].Add(score);
            highScores[difficulty].Sort((a, b) => b.CompareTo(a)); // Sort in descending order

            // Keep only the top 5 scores
            if (highScores[difficulty].Count > 5)
                highScores[difficulty] = highScores[difficulty].Take(5).ToList();

            SaveHighScores();
        }

        public List<int> GetHighScores(string difficulty)
        {
            if (highScores.ContainsKey(difficulty))
            {
                return highScores[difficulty];
            }
            return new List<int>();
        }
    }
}
