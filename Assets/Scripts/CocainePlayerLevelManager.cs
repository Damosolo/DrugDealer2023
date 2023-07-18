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
        public int cost;
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
        int baseRate = 200;

        if (levelRewardMap.ContainsKey(level))
        {
            return baseRate + levelRewardMap[level].moneyPerTask;
        }

        return baseRate;
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
        // Get the maximum defined level
        int maxLevel = 0;
        foreach (var level in levelRewardMap.Keys)
        {
            maxLevel = Mathf.Max(maxLevel, level);
        }

        // Increase the player's level, but do not exceed the maximum defined level
        cocainePlayerLevel = Mathf.Min(cocainePlayerLevel + levelIncrease, maxLevel);
    }
}
