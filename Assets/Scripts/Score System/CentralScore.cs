using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Central Score")]
public class CentralScore : ScriptableObject
{
    [SerializeField] private int score;
    [SerializeField] private int lives;
    [SerializeField] private float durationSurvived = 0f;

    public int highestLifeAmount;
    public int highestScoreAchieved;

    public GameEvent onScoreChangedEvent;
    public GameEvent onLivesChangedEvent;


    /// <summary>
    /// Reduce the Score by a certain amount and triggers the onScoreChangedEvent
    /// </summary>
    /// <param name="subtractAmount">Amount to change the score by</param>
    public void ReduceScoreAmount(int subtractAmount){
        int amountToRemove = score - subtractAmount > 0 ? subtractAmount : score;
        AddScoreAmount(-amountToRemove);
    }

    /// <summary>
    /// Reduce the Score by a certain amount and triggers the onScoreChangedEvent
    /// </summary>
    /// <param name="addAmount">Amount to change the score by</param>
    public void AddScoreAmount(int addAmount){
        score += addAmount;
        highestScoreAchieved = Mathf.Max(score, highestScoreAchieved);
        onScoreChangedEvent.TriggerEvent();
    }

    /// <summary>
    /// Reduces the lives amount settings and triggers onLivesChangedEvent
    /// </summary>
    /// <param name="subtractAmount">Live Amount to change by</param>
    public void ReduceLifeAmount(int subtractAmount){
        int amountToRemove = lives - subtractAmount > 0 ? subtractAmount : lives;

        AddLifeAmount(-amountToRemove);
    }

    /// <summary>
    /// Adds the lives amount settings and triggers onLivesChangedEvent
    /// </summary>
    /// <param name="addAmount">Live Amount to change by</param>
    public void AddLifeAmount(int addAmount){
        lives += addAmount;
        highestLifeAmount = Mathf.Max(lives, highestLifeAmount);
        onLivesChangedEvent.TriggerEvent();
    }

    /// <summary>
    /// Sets the life amount to the amount given
    /// </summary>
    /// <param name="lifeAmount">life Amount</param>
    public void SetLifeAmount(int lifeAmount){
        int amountToSet = Mathf.Max(0, lifeAmount);

        lives = amountToSet;
        onLivesChangedEvent.TriggerEvent();
    }

    /// <summary>
    /// Sets Central Score Amount to amount given
    /// </summary>
    /// <param name="scoreAmount">Amount Given</param>
    public void SetScoreAmount(int scoreAmount){
        int amountToSet = Mathf.Max(0, scoreAmount);

        score = amountToSet;
        onScoreChangedEvent.TriggerEvent();
    }

    /// <summary>
    /// Gets the current Life Amount
    /// </summary>
    /// <returns>life amount</returns>
    public int GetLifeAmount(){
        return lives;
    }

    /// <summary>
    /// Gets the current Score Amount
    /// </summary>
    /// <returns>Score amount</returns>
    public int GetScoreAmount(){
        return score;
    }

    /// <summary>
    /// Returns the time survived for this round
    /// </summary>
    /// <returns>Time in Seconds</returns>
    public float GetDurationSurvived(){
        return durationSurvived;
    }

    /// <summary>
    /// Changes the time survived for this round
    /// </summary>
    public void SetDurationSurvived(float timeSurvived){
        if (timeSurvived >= 0){
            durationSurvived = timeSurvived;
        }
    }
}