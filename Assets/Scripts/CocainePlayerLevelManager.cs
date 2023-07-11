using UnityEngine;
using System.Collections.Generic;

public class CocainePlayerLevelManager : MonoBehaviour
{
    public int cocainePlayerLevel = 1;
    public int xpCost = 10;
    public int xpIncrease = 100;
    public MoneyManager moneyManager;
    public int maxLevel = 5; // Maximum level allowed for the player

    [System.Serializable]
    public class LevelReward
    {
        public int level;
        public int xp;
        public int moneyPerTask;
        public int cost; // Add the 'cost' property
    }

    public List<LevelReward> levelRewards = new List<LevelReward>()
    {
        new LevelReward { level = 2, xp = 100, moneyPerTask = 60, cost = 10 }, // Level 1 reward
        new LevelReward { level = 3, xp = 200, moneyPerTask = 100, cost = 20 }, // Level 2 reward
        new LevelReward { level = 4, xp = 300, moneyPerTask = 110, cost = 30 },  // Level 3 reward
        // Add more levels and rewards as needed
    };

    public void BuyLevel()
    {
        // Check if the player has reached the maximum level
        if (cocainePlayerLevel >= maxLevel)
        {
            Debug.Log("Reached maximum level. No more levels to buy.");
            return;
        }

        // Check if the player has enough money to buy the next level
        int nextLevel = cocainePlayerLevel + 1;
        int levelCost = GetLevelCost(nextLevel);
        if (moneyManager.money >= levelCost)
        {
            // Deduct the cost from the money
            moneyManager.SpendMoney(levelCost);

            // Increase the player's level
            cocainePlayerLevel = nextLevel;

            // Reward the player based on the new level
            RewardPlayer();

            Debug.Log("Level purchased! Player Level: " + cocainePlayerLevel);
        }
        else
        {
            Debug.Log("Not enough money to buy the level. Required cost: " + levelCost);
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
        return 0; // or any default value you prefer
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
        return 0; // Or any default value you prefer
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
        return 0; // or any default value you prefer
    }

    private void RewardPlayer()
    {
        // Check if the current level has an XP reward
        int reward = GetXPReward(cocainePlayerLevel);
        if (reward > 0)
        {
            Debug.Log("Player rewarded at level: " + cocainePlayerLevel + ", XP Reward: " + reward);
            // Add code here to provide the XP reward to the player
        }
        else
        {
            Debug.Log("Player rewarded at level: " + cocainePlayerLevel + ", No XP Reward");
        }

        // Check if the current level has a money reward
        int moneyReward = GetMoneyPerTask(cocainePlayerLevel);
        if (moneyReward > 0)
        {
            Debug.Log("Player rewarded at level: " + cocainePlayerLevel + ", Money Reward: " + moneyReward);
            // Add code here to provide the money reward to the player
            moneyManager.money += moneyReward;

            // Update the money display or perform any other necessary actions
        }
        else
        {
            Debug.Log("Player rewarded at level: " + cocainePlayerLevel + ", No Money Reward");
        }
    }
}
