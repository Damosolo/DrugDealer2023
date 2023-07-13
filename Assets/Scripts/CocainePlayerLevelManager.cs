using UnityEngine;
using System.Collections.Generic;

public class CocainePlayerLevelManager : MonoBehaviour
{
    public int cocainePlayerLevel = 1;
    public int xpCost = 10;
    public int xpIncrease = 100;
    public MoneyManager moneyManager;

    [System.Serializable]
    public class LevelReward
    {
        public int level;
        public int xp;
        public int moneyPerTask;
        public int cost; // Add the 'cost' property
    }

    public List<LevelReward> levelRewards = new List<LevelReward>();

    private Dictionary<int, LevelReward> levelRewardMap = new Dictionary<int, LevelReward>();

    private void Start()
    {
        foreach (var reward in levelRewards)
        {
            levelRewardMap.Add(reward.level, reward);
        }
    }

    public void BuyXP()
    {
        if (moneyManager.money >= xpCost)
        {
            moneyManager.SpendMoney(xpCost);
            IncreaseLevel(xpIncrease);
            RewardPlayer();
            Debug.Log("XP Purchased! Cocaine Player Level: " + cocainePlayerLevel);
        }
        else
        {
            Debug.Log("Not enough money to buy XP!");
        }
    }

    public int GetXPReward(int level)
    {
        if (levelRewardMap.ContainsKey(level))
        {
            return levelRewardMap[level].xp;
        }
        return 0;
    }

    public int GetMoneyPerTask(int level)
    {
        int baseRate = 200; // Set the base rate to 200
        int additionalRate = 0; // Add any additional rate based on the player's level

        if (levelRewardMap.ContainsKey(level))
        {
            additionalRate = levelRewardMap[level].moneyPerTask;
        }

        return baseRate + additionalRate;
    }


    public int GetLevelCost(int level)
    {
        if (levelRewardMap.ContainsKey(level))
        {
            return levelRewardMap[level].cost;
        }
        return 0;
    }

    private void RewardPlayer()
    {
        int reward = GetXPReward(cocainePlayerLevel);
        if (reward > 0)
        {
            Debug.Log("Player rewarded at cocaine level: " + cocainePlayerLevel + ", XP Reward: " + reward);
        }
        else
        {
            Debug.Log("Player rewarded at cocaine level: " + cocainePlayerLevel + ", No XP Reward");
        }

        int moneyReward = GetMoneyPerTask(cocainePlayerLevel);
        if (moneyReward > 0)
        {
            Debug.Log("Player rewarded at cocaine level: " + cocainePlayerLevel + ", Money Reward: " + moneyReward);
            moneyManager.EarnMoney(moneyReward);
        }
        else
        {
            Debug.Log("Player rewarded at cocaine level: " + cocainePlayerLevel + ", No Money Reward");
        }
    }

    public void IncreaseLevel(int levelIncrease)
    {
        cocainePlayerLevel += levelIncrease;
    }
}
