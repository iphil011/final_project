using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlanZucconi.Bots
{
    public class CreateBots : MonoBehaviour
    {
        [Header("Simulation Parameteres")]
        [Range(1, 100)]
        public float BotsPerTeam = 50;

        [Header("Bots")]
        [Range(1, 50)]
        public float Radius = 10;
        public Bot BotPrefab;
        public Transform BotFolder;

        [Space]
        public BotAI[] AIs;


        [Button(Editor=false)]
        void Run()
        {
            Create();
        }


        //[Button(Editor = true)]
        void Delete ()
        {
            Bot[] bots = BotFolder.GetComponentsInChildren<Bot>(true);
            foreach (Bot bot in bots)
                DestroyImmediate(bot.gameObject);
        }
        //[Button(Editor=true)]
        void Create()
        {
            Delete();

            for (int a = 0; a < AIs.Length; a ++)
            {
                BotAI ai = AIs[a];
                ai.Initialise();

                for (int i = 0; i < BotsPerTeam; i ++)
                {
                    Vector3 position = Random.insideUnitCircle * Radius;
                    Bot bot = Instantiate(BotPrefab, position, Quaternion.identity, BotFolder.transform);
                    bot.name = "Bot (" + ai.name +")";
                    bot.AI = ai;
                    bot.Team = a;
                }
            }
        }
    }
}