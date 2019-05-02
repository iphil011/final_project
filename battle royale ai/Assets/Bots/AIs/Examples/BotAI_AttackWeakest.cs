using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AlanZucconi.AI.BT;
using AlanZucconi.Bots;

// Approaches and attack the weakest
[CreateAssetMenu(fileName = "BotAI_AttackWeakest", menuName = "Bots/BotAI_AttackWeakest")]
public class BotAI_AttackWeakest : BotAI
{
    public override Node CreateBehaviourTree(Bot bot)
    {
        return new Filter
        (
            bot.EnemiesInSight,
            new Selector
            (
                // Approaches if far enough
                new Filter
                (
                    () => bot.GetRange(bot.WeakestEnemy) > 0.5f,
                    new Action( () => bot.Approach(bot.WeakestEnemy) )
                ),
                // Shoot if far enough
                new Action( () => bot.Attack(bot.WeakestEnemy) )
            )
        );
    }
}