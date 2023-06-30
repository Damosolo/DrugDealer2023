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
        public int cost; // Add the 'cost' property
    }

    public List<LevelReward> levelRewards = new List<LevelReward>()
    {
        new LevelReward { level = 2, xp = 100, moneyPerTask = 60, cost = 10 }, // Level 1 reward
        new LevelReward { level = 3, xp = 200, moneyPerTask = 100, cost = 20 }, // Level 2 reward
        new LevelReward { level = 4, xp = 300, moneyPerTask = 110, cost = 30 }  // Level 3 reward
        // Add more levels and rewards as needed
    };

    public void BuyXP()
    {
        // Check if the player has enough money to buy XP
        if (moneyManager.money >= xpCost)
        {
            // Deduct the cost from the money
            moneyManager.SpendMoney(xpCost);

            // Increase the player's level
            playerLevel += xpIncrease;

            // Reward the player based on the new level
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
        int reward = GetXPReward(playerLevel);
        if (reward > 0)
        {
            Debug.Log("Player rewarded at level: " + playerLevel + ", XP Reward: " + reward);
            // Add code here to provide the XP reward to the player
        }
        else
        {
            Debug.Log("Player rewarded at level: " + playerLevel + ", No XP Reward");
        }

        // Check if the current level has a money reward
        int moneyReward = GetMoneyPerTask(playerLevel);
        if (moneyReward > 0)
        {
            Debug.Log("Player rewarded at level: " + playerLevel + ", Money Reward: " + moneyReward);
            // Add code here to provide the money reward to the player
            moneyManager.money += moneyReward;

            // Update the money display or perform any other necessary actions
        }
        else
        {
            Debug.Log("Player rewarded at level: " + playerLevel + ", No Money Reward");
        }
    }

    public void IncreaseLevel(int level)
    {
        playerLevel = level;
    }
}
