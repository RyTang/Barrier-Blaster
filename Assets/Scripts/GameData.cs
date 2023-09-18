using System;
[System.Serializable]
public class GameData
{
    // Game Records
    public int highestHighScore;
    public float highestDurationSurvived;
    public int highestLifeAmount;
    
    // Player Scores
    public int points;

    // Personal Details
    public string userName;

    // TODO: Mutators that are needed

    public void UpdateGameData(CentralScore scoreData){
        // Update Game Records
        highestHighScore = Math.Max(highestHighScore, scoreData.highestScoreAchieved);
        highestDurationSurvived = Math.Max(highestDurationSurvived, scoreData.GetDurationSurvived());
        highestLifeAmount = Math.Max(highestLifeAmount, scoreData.highestLifeAmount);

        // Update points
        points += scoreData.GetScoreAmount();
    }
}
