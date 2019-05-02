using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using AlanZucconi.Data;

namespace AlanZucconi.Bots
{
    public class Automation : MonoBehaviour
    {
        [Header("Game Parameteres")]
        [Range(0f,10f)]
        public float TimeScale = 1f;

        [Header("Simulation Parameteres")]
        [Range(1, 100)]
        public int BotsPerTeam = 50;

        [Range(1,250)]
        public int BattlesPerPair = 100;
        

        [Space]
        [Range(60, 60 * 10)]
        public float MaxBattleTime = 60 * 2; // seconds
        


        


        [Header("Bots")]
        [Range(1, 50)]
        public float Radius;
        public Bot BotPrefab;
        public Transform BotFolder;
        
        //public bool Rendering = true;
        [Space]
        public List<BotAI> AIs;


        [Space]
        public BattleResults Results;
        public bool ClearResults = true;
        


        [Header("Progress")]
        // Tournament progress
        [ProgressBar(label = "TournamentLabel")]
        public float TournamentProgress = 0;
        [HideInInspector]
        public string TournamentLabel;
        // Pair progress
        [ProgressBar(label = "PairLabel")]
        public float PairProgress = 0;
        [HideInInspector]
        public string PairLabel;
        // Battle progress
        [ProgressBar(label = "BattleLabel")]
        public float BattleProgress = 0;
        [HideInInspector]
        public string BattleLabel;


        //[GridPlot(Trace = false)]
        //public GridData Score;

        [Button(Editor=false)]
        void Run()
        {
            enabled = true;

            Time.timeScale = TimeScale;
            StartCoroutine(Automate());
        }

        void Update()
        {
            if (Teams == null)
                return;

            BattleProgress = (Time.time - BattleStartTime) / MaxBattleTime;

            BattleLabel = "";
            foreach (List<Bot> team in Teams)
            {
                BattleLabel += string.Format("{0} ({1}), ", team[0].AI.name, 
                    team.Count(bot=>bot.IsAlive())
                    );
            }
        }

        IEnumerator Automate ()
        {
            // Clears the results
            if (Results != null && ClearResults)
                Results.ClearScores();

            //Score = new int[AIs.Count, AIs.Count];
            //Score = new GridData(AIs.Count, AIs.Count);
            //Score.LabelsR = Score.LabelsC = AIs.Select(ai=>ai.AIName).ToArray();

            // All the pairs
            float totalPairs = (AIs.Count * (AIs.Count - 1)) / 2f;
            int p = 0;
            foreach (List<BotAI> pair in AIs.DistinctPairs())
            {
                TournamentProgress = p / (totalPairs-1);
                TournamentLabel = string.Format("Pair {0} of {1}", p + 1, totalPairs);
                p++;

                // Repeates BattlesPerPair battles
                for (int i = 0; i < BattlesPerPair; i ++)
                {
                    PairProgress = i / (BattlesPerPair - 1f);
                    PairLabel = string.Format("Battle {0} of {1}", i + 1, BattlesPerPair);

                    CreateBots(pair);

                    StartBattle();
                    yield return new WaitUntil(BattleDone); // Wait until battle done
                    EndBattle();
                }
            }

            DeleteBots();
            /*
            // Results
            for (int i = 0; i < Score.Rows; i++)
                for (int j = 0; j < Score.Columns; j++)
                    Debug.LogFormat("{0}\tvs\t{1}:\t{2}", AIs[i].name, AIs[j].name, Score[i,j]);

            // Cumulative results
            for (int i = 0; i < Score.Rows; i++)
            {
                float sum = 0;
                for (int j = 0; j < Score.Columns; j++)
                    sum += Score[i, j];

                // Result
                // ai name, int score, float score (100%=always win and killled all the enemies)
                Debug.LogFormat("AI {0}:\t{1}\t{2}%", AIs[i].name, sum, Mathf.RoundToInt(sum / (float)((AIs.Count-1)*BattlesPerPair*BotsPerTeam)*100));
            }
            */
        }


        private float BattleStartTime;
        public void StartBattle ()
        {
            BattleStartTime = Time.time;
        }
        public void EndBattle ()
        {
            // ASSUMPTION: only 2 teams
            // ASSUMPTION: all AIs in a team have the same BotAI scriptableObject
            // ASSUMPTION: the two AI are different objects

            // Who scored?
            //int teamA = AIs.IndexOf(Teams[0][0].AI);
            //int teamB = AIs.IndexOf(Teams[1][0].AI);

            int teamAscore = Teams[0].Count(bot => bot.IsAlive());
            int teamBscore = Teams[1].Count(bot => bot.IsAlive());

            // The score is the difference in surviving bots
            //Score[teamA, teamB] += teamAscore - teamBscore;
            //Score[teamB, teamA] += teamBscore - teamAscore;
            // TODO: save individual scores so we can get variance etc

            if (Results != null)
            {
                Results.AddBattleResult(Teams[0][0].AI, Teams[1][0].AI, teamAscore, teamBscore);
                Results.AddBattleResult(Teams[1][0].AI, Teams[0][0].AI, teamBscore, teamAscore);
            }


            Teams = null;
        }

        public bool BattleDone ()
        {
            // Time's up!
            if (Time.time >= BattleStartTime + MaxBattleTime)
                return true;

            // There is only one team alive
            // (counts how many teams have at at least one active bot)
            return Teams.Count(team => team.Exists(bot => bot.IsAlive())) <= 1;
        }

        
        private void DeleteBots()
        {
            Bot[] bots = BotFolder.transform.GetComponentsInChildren<Bot>(true);
            foreach (Bot bot in bots)
                DestroyImmediate(bot.gameObject);
        }
        private void CreateBots(List<BotAI> ais)
        {
            DeleteBots();
            Teams = new List<List<Bot>>(); // List of teams
            for (int a = 0; a < ais.Count; a++)
            {
                // All bots in a team
                List<Bot> team = new List<Bot>();
                Teams.Add(team);

                BotAI ai = ais[a];
                ai.Initialise();

                for (int i = 0; i < BotsPerTeam; i++)
                {
                    Vector3 position = Random.insideUnitCircle * Radius;
                    Bot bot = Instantiate(BotPrefab, position, Quaternion.identity, BotFolder.transform);
                    bot.name = "Bot (" + ai.name + ")";
                    bot.AI = ai;
                    bot.Team = a;

                    team.Add(bot);
                }
            }
        }

        private List<List<Bot>> Teams;
    }
}