using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AlanZucconi.AI.BT;
using AlanZucconi.Bots;

[CreateAssetMenu(fileName = "BotAI_iphil011", menuName = "Bots/BotAI_iphil011")]
public class BotAI_iphil011 : BotAI
{
    public Bot target;
    public BotAI_iphil011 leader;
    public Vector2 pos;
    public bool boss;
    public bool squad;
    public List<Bot> MyBotList;
    public AnimationCurve health;

    public BotAI_iphil011(Bot bot) {
        BotAI_iphil011 a = new BotAI_iphil011(bot);
        pos = bot.Position;
    }


    public override void Initialise()
    {
        base.Initialise();
        leader = null;
        MyBotList = new List<Bot>();
        boss = false;
        
        
    }
    public override Node CreateBehaviourTree(Bot bot)
    {

        return new Selector(
            new Filter(
                () => leader = null,
                        new Action(
                            () => Squad(Convert(bot.Allies))
                        )
                ),
            new Filter(
                bot.NoEnemiesInSight,
                new Selector(
                    new Filter(
                        () => bot.Health < 0.8,
                            new Action(bot.Heal)
                            ),
                        new Action(() => bot.Approach(bot.ArenaCentre)
                        )
                    )
                ),
            new Filter(
                bot.EnemiesInSight,
                new Selector(
                    new Filter(
                        () => bot.Health < 0.1,
                        new Action(
                            () => bot.FleeFrom(bot.EnemyBarycenter)
                            )
                        ),
                    new Filter(
                        bot.ReadyToShoot,
                        new Selector(
                        new Filter(
                            () => bot.GetRange(bot.ClosestEnemy) > 0.6f,
                            new Action(
                                () => bot.Approach(bot.ClosestEnemy)
                                )
                            ),
                            new Filter(
                                () => bot.GetRange(bot.WeakestEnemy) < 0.6f,
                                new Action(
                                () => bot.Attack(bot.WeakestEnemy)
                                )
                            ),
                            new Action(
                                ()=> bot.Attack(bot.ClosestEnemy)
                                )
                            )
                        ),
                        new Filter(
                            bot.NotReadyToShoot,
                            new Action(
                                () => bot.Approach(bot.AllyBarycenter)
                            )
                        )
                    )
                )
            );
    }

    void Squad(List<BotAI_iphil011> bots) {
        foreach (BotAI_iphil011 a in bots) {
            if (a.boss)
            {
                leader = a;
                squad = true;
            }
            else {
                boss = true;
                leader = this;
                squad = false;
            }
        }
    }

    List<BotAI_iphil011> Convert(List<Bot> allies) {
        List<BotAI_iphil011> myBots = new List<BotAI_iphil011>();
        foreach (Bot a in allies) {
            BotAI_iphil011 b = new BotAI_iphil011(a);
            myBots.Add(b);
        }
        return myBots;
    }
}
