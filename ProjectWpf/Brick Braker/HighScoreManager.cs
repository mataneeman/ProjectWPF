using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class HighScoreManager
{
    private const string FileName = "highscores.txt";
    private const int MaxScores = 5;

    public List<int> LoadHighScores()
    {
        List<int> scores = new List<int>();

        if (File.Exists(FileName))
        {
            string[] lines = File.ReadAllLines(FileName);
            foreach (string line in lines)
            {
                if (int.TryParse(line, out int score))
                {
                    scores.Add(score);
                }
            }
        }

        return scores.OrderByDescending(score => score).Take(MaxScores).ToList();
    }

    public void SaveHighScore(int score)
    {
        List<int> scores = LoadHighScores();

        if (scores.Count < MaxScores || score > scores.Last())
        {
            if (scores.Count >= MaxScores)
            {
                scores.RemoveAt(scores.Count - 1);
            }

            scores.Add(score);
            scores = scores.OrderByDescending(s => s).ToList();
            File.WriteAllLines(FileName, scores.Select(s => s.ToString()));
        }
    }
}
