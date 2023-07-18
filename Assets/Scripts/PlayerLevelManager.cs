using UnityEngine;
using System.Collections.Generic;

public class PlayerLevelManager : MonoBehaviour
{
    public int playerLevel = 1;
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

    public List<LevelReward> levelRewards = new List<LevelReward>()
    {
        new LevelReward { level = 2, xp = 100, moneyPerTask = 60, cost = 10 },
        new LevelReward { level = 3, xp = 200, moneyPerTask = 100, cost = 20 },
        new LevelReward { level = 4, xp = 300, moneyPerTask = 110, cost = 30 }
    };

    public void BuyXP()
    {
        if (moneyManager.money >= xpCost)
        {
            moneyManager.SpendMoney(xpCost);
            IncreaseLevel(xpIncrease);
            RewardPlayer();
            Debug.Log("XP Purchased! Player Level: " + playerLevel);
        }
        else
        {
            Debug.Log("Not enough money to buy XP!");
        }
    }

    public int GetXPReward(int level)
    {
        foreach (var reward in levelRewards)
        {
            if (reward.level == level)
            {
                return reward.xp;
            }
        }
        return 0;
    }

    public int GetMoneyPerTask(int level)
    {
        foreach (var reward in levelRewards)
        {
            if (reward.level == level)
            {
                return reward.moneyPerTask;
            }
        }
        return 0;
    }

    public int GetLevelCost(int level)
    {
        foreach (var reward in levelRewards)
        {
            if (reward.level == level)
            {
                return reward.cost;
            }
        }
        return 0;
    }

    private void RewardPlayer()
    {
        int reward = GetXPReward(playerLevel);
        if (reward > 0)
        {
            Debug.Log("Player rewarded at level: " + playerLevel + ", XP Reward: " + reward);
        }
        else
        {
            Debug.Log("Player rewarded at level: " + playerLevel + ", No XP Reward");
        }

        int moneyReward = GetMoneyPerTask(playerLevel);
        if (moneyReward > 0)
        {
            Debug.Log("Player rewarded at level: " + playerLevel + ", Money Reward: " + moneyReward);
            moneyManager.money += moneyReward;
        }
        else
        {
            Debug.Log("Player rewarded at level: " + playerLevel + ", No Money Reward");
        }
    }

    public void IncreaseLevel(int levelIncrease)
    {
        // Get the maximum defined level
        int maxLevel = 0;
        foreach (var reward in levelRewards)
        {
            maxLevel = Mathf.Max(maxLevel, reward.level);
        }

        // Increase the player's level, but do not exceed the maximum defined level
        playerLevel = Mathf.Min(playerLevel + levelIncrease, maxLevel);
    }
}
