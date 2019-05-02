using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

using AlanZucconi.AI.BT;

namespace AlanZucconi.Bots
{
    /*
    // Can be targeted by a bot
    public interface ITargetable
    {
        Vector2 Position
        {
            get;
        }
    }
    
    // Targetable Vector2
    public struct Vector2Target : ITargetable
    {
        public float x, y;

        // Vector2
        public static implicit operator Vector2 (Vector2Target v)
        {
            return v.Position;
        }
        public static implicit operator Vector2Target (Vector2 v)
        {
            return new Vector2Target() { x = v.x, y = v.y };
        }
        // Vector3
        public static implicit operator Vector3(Vector2Target v)
        {
            return v.Position;
        }
        public static implicit operator Vector2Target(Vector3 v)
        {
            return new Vector2Target() { x = v.x, y = v.y };
        }
        // Transform
        public static implicit operator Vector2Target(Transform t)
        {
            return new Vector2Target() { x = t.position.x, y = t.position.y };
        }

        public Vector2 Position
        {
            get
            {
                return new Vector2(x, y);
            }
        }
    }
    */
    public enum BotAction
    {
        None,
        Idle,
        Move,
        Attack,
        Heal
    }

    //public interface IAttackable { }

    //public class Target : MonoBehaviour, ITargetable { }

    public class Bot : MonoBehaviour//, ITargetable
    {
        

        public Rigidbody2D Rigidbody;
        public SpriteRenderer Sprite;
        
        [Header("Status")]
        public BotAction Action = BotAction.None;

        /// <summary>
        /// The health (between 0 and 1) of the current bot.
        /// <param/>
        /// You should only read this value.
        /// </summary>
        [Range(0f, 1f)]
        public float Health = 1f; // HP

        [Header("Move")]
        [Range(0, 10)]
        //[EditorOnly]
        public float MaxForce = 1f; // Newtons

        [Header("Attack")]
        [Range(0f,10f)]
        //[EditorOnly]
        public float MaxAttackDistance = 10; // metres
        [Range(0f,1f)]
        //[EditorOnly]
        public float MaxDamage = 0.1f; // max HP per shoot

        public AnimationCurve DamageCurve; // x: shooting range (0,1), y: damage Normalised (0,1)
        public AnimationCurve HitCurve; // x: distance, y: damage Normalised (0,1)

        [Space]
        public Timer ReloadTimer;

        [Header("Heal")]
        [Range(0,1f)]
        //[EditorOnly]
        public float HealingSpeed = 1f/60f; // HP regenerated per second


        [Header("Perception")]
        [Range(0,20)]
        public float PerceptionRadius = 15; // metres
        public LayerMask BotMask;

        [Header("AI")]
        public BotAI AI;
        private BehaviourTree Tree;

        [Header("Teams")]
        /// <summary>
        /// The team of the current bots.
        /// <param/>
        /// Bots in the same team are allies, and should play together.
        /// <param/>
        /// You should only read this value.
        /// </summary>
        public int Team;

        /// <summary>
        /// A list of all the enemies in sight.
        /// <param/>
        /// You should only read this value.
        /// </summary>
        public List<Bot> Enemies;
        /// <summary>
        /// A list of all the allies in sight.
        /// <param/>
        /// You should only read this value.
        /// </summary>
        public List<Bot> Allies;





        /// <summary>
        /// Returns <see langword="true"/> if there are enemies in sight.
        /// </summary>
        public bool EnemiesInSight ()
        {
            return Enemies.Count > 0;
        }
        /// <summary>
        /// Returns <see langword="true"/> if there are no enemies in sight.
        /// </summary>
        public bool NoEnemiesInSight ()
        {
            return !EnemiesInSight();
        }


        /// <summary>
        /// Returns <see langword="true"/> if there are allies in sight.
        /// </summary>
        public bool AlliesInSight ()
        {
            return Allies.Count > 0;
        }
        /// <summary>
        /// Returns <see langword="true"/> if there are no allies in sight.
        /// </summary>
        public bool NoAlliesInSight ()
        {
            return !AlliesInSight();
        }




        /// <summary>
        /// Returns the damage (measured in health points) that this bot will yield
        /// if it hits a target at a certain <paramref name="distance"/>.
        /// </summary>
        public float DamageAtDistance (float distance)
        {
            if (distance > MaxAttackDistance)
                return 0;

            float x = Mathf.Clamp01(distance / MaxAttackDistance);
            float y = DamageCurve.Evaluate(x); // normalised damage [0,1]

            return MaxDamage * y;
        }
        /// <summary>
        /// Returns the probability (between 0 and 1) of hitting a target
        /// at a certain <paramref name="distance"/>.
        /// </summary>
        public float HitChance (float distance)
        {
            if (distance > MaxAttackDistance)
                return 0;

            float x = Mathf.Clamp01(distance / MaxAttackDistance);
            float y = DamageCurve.Evaluate(x);
            return y;
        }


        // Deals damage to this bot
        public void Damage (float damage)
        {
            Health = Mathf.Clamp01(Health - damage);

            if (Health <= 0)
            {
                gameObject.SetActive(false);
            }
        }



        /// <summary>
        /// A current position of the bot.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return transform.position;
            }
        }

        public static implicit operator Vector2 (Bot bot)
        {
            if (bot == null)
                return Vector2.zero;

            return bot.Position;
        }

        // [0,1]
        // 0: close
        // 1: far
        /// <summary>
        /// Returns the normalised range (between 0 and 1) of a target.
        /// <para/>
        /// Returns 0 when the <paramref name="enemy"/> is close,
        /// 1 when the <paramref name="enemy"/> is out of range.
        /// </summary>
        public float GetRange (Bot enemy)
        {
            return Mathf.Clamp01(DistanceFrom(enemy) / MaxAttackDistance);
        }

        // Use this for initialization
        void Start()
        {
            //List<Bot> bots = new List<Bot>();
            //List<ITargetable> enemies = bots;

            Initialise();
        }

        public void Initialise()
        {
            // Resets the AI
            Node root = AI.CreateBehaviourTree(this);
            Tree = new BehaviourTree(root);

            // Resets the Bot
            Health = 1f;
            ReloadTimer.Start();

            // Resets the rigidbody
            Rigidbody.velocity = Vector2.zero;
            Rigidbody.angularVelocity = 0;

            Action = BotAction.None;
            
            // Cached values
            _ClosestEnemy = new Cached<Bot>(()=> Closest(Enemies));
            _FurthestEnemy = new Cached<Bot>(() => Furthest(Enemies));
            _ClosestAlly = new Cached<Bot>(() => Closest(Allies));
            _FurthestAlly = new Cached<Bot>(() => Furthest(Allies));

            _WeakestEnemy = new Cached<Bot>(() => Weakest(Enemies));
            _StrongestEnemy = new Cached<Bot>(() => Strongest(Enemies));
            _WeakestAlly = new Cached<Bot>(() => Weakest(Allies));
            _StrongestAlly = new Cached<Bot>(() => Strongest(Allies));

            _EnemyBarycenter = new Cached<Vector2>(() => Barycenter(Enemies));
            _AllyBarycenter = new Cached<Vector2>(() => Barycenter(Allies));
        }


        // Cached values
        private Cached<Bot> _ClosestEnemy;
        private Cached<Bot> _FurthestEnemy;
        private Cached<Bot> _ClosestAlly;
        private Cached<Bot> _FurthestAlly;

        private Cached<Bot> _WeakestEnemy;
        private Cached<Bot> _StrongestEnemy;
        private Cached<Bot> _WeakestAlly;
        private Cached<Bot> _StrongestAlly;

        private Cached<Vector2> _EnemyBarycenter;
        private Cached<Vector2> _AllyBarycenter;








        // Update is called once per frame
        void Update()
        {
            DebugDraw.Circle(Position, 0.25f, ColorExtension.RYG(Health).xA(0.5f));

            // Resets the action
            Action = BotAction.None;

            //Sprite.color = ColorExtension.RYG(Health);
            //Sprite.color = Team == 0 ? Color.white : Color.black;
            Color[] colours = { Color.white, Color.black, Color.magenta };
            Sprite.color = colours[Team];


            UpdatePerception();
            Tree.Update();

        }

        private Collider2D[] TempPerception = new Collider2D[128];
        public void UpdatePerception ()
        {
            int count = Physics2D.OverlapCircleNonAlloc
            (
                Position, PerceptionRadius,
                TempPerception,
                BotMask
            );


            var bots = TempPerception
                    .Take(count)
                    .Select(enemy => enemy.GetComponent<Bot>())
                    //.Where(bot => IsEnemy(bot))
                    .Where(bot => bot.IsAlive())
                    .Where(bot => bot != this)
                    .ToLookup(bot => IsEnemy(bot));

            Enemies.Clear();
            Enemies.AddRange(bots[true]);

            Allies.Clear();
            Allies.AddRange(bots[false]);



            // Invalidates cached values
            _ClosestEnemy.Invalidate();
            _FurthestEnemy.Invalidate();
            _ClosestAlly.Invalidate();
            _FurthestAlly.Invalidate();

            _WeakestEnemy.Invalidate();
            _StrongestEnemy.Invalidate();
            _WeakestAlly.Invalidate();
            _StrongestAlly.Invalidate();

            _EnemyBarycenter.Invalidate();
            _AllyBarycenter.Invalidate();
    }

        /// <summary>
        /// Returns <see langword="true"/> if the <paramref name="bot"/> is an enemy.
        /// </summary>
        public bool IsEnemy (Bot bot)
        {
            return bot.Team != Team;
        }
        /// <summary>
        /// Returns <see langword="true"/> if the <paramref name="bot"/> is an ally.
        /// </summary>
        public bool IsAlly (Bot bot)
        {
            return !IsEnemy(bot);
        }


        /// <summary>
        /// Moves in the <paramref name="direction"/>.
        /// <para/>
        /// <paramref name="direction"/> should be a unit vector.
        /// </summary>
        public void Move (Vector2 direction)
        {
            // Only one action per update!
            if (Action != BotAction.None)
            {
                Debug.LogErrorFormat("Bots can only do one action per Update! {0} is requested, but bot '{1}' already executed {2} this Update!",  BotAction.Move, name, Action);
                return;
            }
            Action = BotAction.Move;

            // Direction is normalised to make sure we don't pass a longer vector
            Vector2 force = direction.normalized * MaxForce;
            Rigidbody.AddForce(force);
        }

        /// <summary>
        /// Moves away from the <paramref name="target"/>.
        /// </summary>
        //public void FleeFrom(ITargetable target)
        public void FleeFrom(Vector2 position)
        {
            //if (target == null)
            //    return;

            //Vector2 direction = (target.Position - Position).normalized;
            Vector2 direction = (position - Position).normalized;
            Move(-direction * MaxForce);
            //Vector2 force = direction * MaxForce;
            //Rigidbody.AddForce(-force);

            // TODO: graphics
            //DebugDraw.DashedLine(Position, target.Position, Color.blue.xA(0.25f), 0.25f);
            DebugDraw.DashedLine(Position, position, Color.blue.xA(0.25f), 0.25f);
            DebugDraw.Arrow(Position, Position - direction * 1f, Color.blue);
            //Debug.DrawLine(Position, target.Position, Color.blue.xA(0.5f));
        }
        /// <summary>
        /// Moves closer to the <paramref name="target"/>.
        /// </summary>
        //public void Approach(ITargetable target)
        public void Approach(Vector2 position)
        {
            //if (target == null)
            //    return;

            //Vector2 direction = (target.Position - Position).normalized;
            Vector2 direction = (position - Position).normalized;
            Move(direction * MaxForce);
            //Vector2 force = direction * MaxForce;
            //Rigidbody.AddForce(force);

            // TODO: graphics
            //DebugDraw.DashedLine(Position, target.Position, Color.green.xA(0.25f), 0.25f);
            DebugDraw.DashedLine(Position, position, Color.green.xA(0.25f), 0.25f);
            DebugDraw.Arrow(Position, Position + direction * 1f, Color.green);
            //Debug.DrawLine(Position, target.Position, Color.green.xA(0.5f));
        }
        /// <summary>
        /// Does nothing.
        /// </summary>
        public void Idle()
        {

        }


        /// <summary>
        /// Slowly heals the current bot.
        /// </summary>
        public void Heal ()
        {
            // Only one action per update!
            if (Action != BotAction.None)
            {
                Debug.LogErrorFormat("Bots can only do one action per Update! {0} is requested, but bot '{1}' already executed {2} this Update!", BotAction.Heal, name, Action);
                return;
            }
            Action = BotAction.Heal;


            // TODO: graphics

            Health = Mathf.Clamp01(Health + HealingSpeed * Time.deltaTime);

            DebugDraw.Circle(transform.position, 0.25f + 0.05f,
                Health < 1
                ? Color.cyan
                : Color.cyan.xA(0.5f)
                );
        }

        #region Attack
        /// <summary>
        /// Attacks the enemy <paramref name="bot"/>.
        /// <para/>
        /// You can attack any bot (ally or enemy).
        /// </summary>
        public void Attack(Bot bot)
        {
            // Only one action per update!
            if (Action != BotAction.None)
            {
                Debug.LogErrorFormat("Bots can only do one action per Update! {0} is requested, but bot '{1}' already executed {2} this Update!", BotAction.Attack, name, Action);
                return;
            }
            Action = BotAction.Attack;



            if (bot == null)
                return;

            PreviousTarget = bot;
            Shoot(bot);
        }

        // The last bot that was attacked
        /// <summary>
        /// The bot that was previously targeted for an attack.
        /// <param/>
        /// You should only read this value.
        /// </summary>
        [ReadOnly]
        public Bot PreviousTarget = null;


        public bool Shoot (Bot enemy)
        {
            // Not ready to shoot yet
            if (!ReloadTimer.IsDone())
                return false;

            ReloadTimer.Start();

            // Graphics
            Vector2 halfPoint = Vector2.Lerp(Position, enemy.Position, 1f/3f);
            DebugDraw.Arrow(Position, halfPoint, Color.red.xA(0.25f), 0.25f);
            DebugDraw.Arrow(halfPoint, enemy.Position, Color.red.xA(0.25f), 0.25f);

            // Hit or miss?
            float distance = DistanceFrom(enemy);
            float hitChance = HitChance(distance);
            if (UnityEngine.Random.Range(0f,1f) > hitChance)
            {
                // Miss!
                SegmentFont.Print(halfPoint, "  MISS", Color.red.xA(0.5f), 0.05f, 0.5f);
                return false;
            }




            // Damages the enemy
            float damage = DamageAtDistance(distance);
            enemy.Damage(damage);


            DebugDraw.FadeLine(Position, enemy.Position, Color.red, 0.25f);

            SegmentFont.Print(halfPoint, "  " + Mathf.RoundToInt(damage/MaxDamage * 100f).ToString(), Color.red, 0.05f, 0.5f);


            //DebugDraw.Arrow(Position, enemy.Position, Color.red, 0.5f);
            //Debug.DrawLine(Position, enemy.Position, Color.red, 0.25f);

            return true;
        }

        // In range
        /// <summary>
        /// Returns <see langword="true"/> if the <paramref name="target"/> is in the shooting range.
        /// </summary>
        public bool InRange(Vector2 position)
        {
            return DistanceFrom(position) <= MaxAttackDistance;
        }
        /// <summary>
        /// Returns <see langword="true"/> if the <paramref name="target"/> is out of the shooting range.
        /// </summary>
        public bool OutOfRange(Vector2 position)
        {
            return !InRange(position);
        }

        /// <summary>
        /// Returns <see langword="true"/> if the current bot is ready to shoot.
        /// </summary>
        public bool ReadyToShoot ()
        {
            return ReloadTimer.IsDone();
        }
        /// <summary>
        /// Returns <see langword="true"/> if the current bot is not ready to shoot.
        /// </summary>
        public bool NotReadyToShoot ()
        {
            return !ReadyToShoot();
        }
        #endregion



        /// <summary>
        /// Returns <see langword="true"/> if the current bot is dead.
        /// </summary>
        public bool IsDead ()
        {
            return Health <= 0f;
        }
        /// <summary>
        /// Returns <see langword="true"/> if the current bot is alive.
        /// </summary>
        public bool IsAlive ()
        {
            return !IsDead ();
        }








        #region Distance
        /// <summary>
        /// The distance (in metres) from a target.
        /// Returns <see cref="float.PositiveInfinity"/> if the target is <see langword="null"/>.
        /// </summary>
        public float DistanceFrom(Vector2 position)
        {
            //if (target == null)
            //    return float.PositiveInfinity;

            return Vector3.Distance(Position, position);
        }

        /// <summary>
        /// The closest bot from the list.
        /// Returns <see langword="null"/> if the list is empty.
        /// </summary>
        public Bot Closest (IEnumerable<Bot> bots)
        {
            //if (bots.Count == 0)
            if (!bots.Any())
                return null;

            return bots.MinBy(bot => DistanceFrom(bot));
        }
        /// <summary>
        /// The furthest bot from the list.
        /// Returns <see langword="null"/> if the list is empty.
        /// </summary>
        public Bot Furthest(IEnumerable<Bot> bots)
        {
            //if (bots.Count == 0)
            if (!bots.Any())
                return null;

            return bots.MaxBy(bot => DistanceFrom(bot));
        }


        


        /// <summary>
        /// The closest enemy in sight.
        /// Returns <see langword="null"/> if there are no enemies in sight.
        /// </summary>
        public Bot ClosestEnemy
        {
            get
            {
                return _ClosestEnemy;
                //return Closest(Enemies);
            }
        }
        /// <summary>
        /// The furthest enemy in sight.
        /// Returns <see langword="null"/> if there are no enemies in sight.
        /// </summary>
        public Bot FurthestEnemy
        {
            get
            {
                return _FurthestEnemy;
                //return Furthest(Enemies);
            }
        }

        /// <summary>
        /// The closest ally in sight.
        /// Returns <see langword="null"/> if there are no enemies in sight.
        /// </summary>
        public Bot ClosestAlly
        {
            get
            {
                return _ClosestAlly;
                //return Closest(Allies);
            }
        }
        /// <summary>
        /// The furthest ally in sight.
        /// Returns <see langword="null"/> if there are no enemies in sight.
        /// </summary>
        public Bot FurthestAlly
        {
            get
            {
                return _FurthestAlly;
                //return Furthest(Allies);
            }
        }
        #endregion





        #region Barycenter
        /// <summary>
        /// The barycenter of a group of bots.
        /// Returns the current bot position if the list is empty.
        /// </summary>
        public Vector2 Barycenter(IEnumerable<Bot> bots)
        {
            //if (bots.Count == 0)
            if (! bots.Any())
                return Position;

            return bots
                .Select(bot => bot.Position)
                .Aggregate((p0, p1) => p0 + p1)
                / bots.Count();
        }

        /// <summary>
        /// The barycenter of the enemies in sight.
        /// Returns the current bot position if there are no enemies in sight.
        /// </summary>
        public Vector2 EnemyBarycenter
        {
            get
            {
                return _EnemyBarycenter;
                //return Barycenter(Enemies);
            }
        }
        /// <summary>
        /// The barycenter of the allies in sight.
        /// Returns the current bot position if there are no allies in sight.
        /// </summary>
        public Vector2 AllyBarycenter
        {
            get
            {
                return _AllyBarycenter;
                //return Barycenter(Allies);
            }
        }
        #endregion






        #region Strength
        /// <summary>
        /// The weakest bot from the list.
        /// Returns <see langword="null"/> if the list is empty.
        /// </summary>
        public static Bot Weakest(IEnumerable<Bot> bots)
        {
            //if (bots.Count == 0)
            if (!bots.Any())
                return null;

            return bots.MinBy(bot => bot.Health);
        }
        /// <summary>
        /// The strongest bot from the list.
        /// Returns <see langword="null"/> if the list is empty.
        /// </summary>
        public static Bot Strongest(IEnumerable<Bot> bots)
        {
            //if (bots.Count == 0)
            if (! bots.Any())
                return null;

            return bots.MaxBy(bot => bot.Health);
        }

        /// <summary>
        /// The weakest enemy in sight.
        /// Returns <see langword="null"/> if there are no enemies in sight.
        /// </summary>
        public Bot WeakestEnemy
        {
            get
            {
                return _WeakestEnemy;
                //return Weakest(Enemies);
            }
        }
        /// <summary>
        /// The strongest enemy in sight.
        /// Returns <see langword="null"/> if there are no enemies in sight.
        /// </summary>
        public Bot StrongestEnemy
        {
            get
            {
                return _StrongestEnemy;
                //return Strongest(Enemies);
            }
        }



        /// <summary>
        /// The weakest ally in sight.
        /// Returns <see langword="null"/> if there are no allies in sight.
        /// </summary>
        public Bot WeakestAlly
        {
            get
            {
                return _WeakestAlly;
                //return Weakest(Allies);
            }
        }
        /// <summary>
        /// The strongest ally in sight.
        /// Returns <see langword="null"/> if there are no allies in sight.
        /// </summary>
        public Bot StrongestAlly
        {
            get
            {
                return _StrongestAlly;
                //return Strongest(Allies);
            }
        }



        /// <summary>
        /// Returns <see langword="true"/> if you are weaker than <paramref name="bot"/>.
        /// </summary>
        public bool WeakerThan(Bot bot)
        {
            return Health < bot.Health;
        }
        /// <summary>
        /// Returns <see langword="true"/> if you are stronger than <paramref name="bot"/>.
        /// </summary>
        public bool StrongerThan (Bot bot)
        {
            return Health > bot.Health;
        }
        #endregion


        
        //public 
        /// <summary>
        /// The centre of the arena.
        /// </summary>
        public Vector2 ArenaCentre
        {
            get
            {
                return Vector2.zero;
            }
        }
        

        private void OnDrawGizmosSelected()
        {
            //Gizmos.color = Color.green.xA(0.25f);
            //Gizmos.DrawWireSphere(transform.position, PerceptionRadius);

            //Gizmos.color = Color.red.xA(0.25f);
            //Gizmos.DrawWireSphere(transform.position, MaxAttackDistance);

            DebugDraw.Circle(transform.position, PerceptionRadius, Color.green.xA(0.5f));
            DebugDraw.Circle(transform.position, MaxAttackDistance, Color.red.xA(0.5f));


            // if (Selection.activeGameObject != transform.gameObject) {

            // Highlights the enemies
            //if (EnemiesInSight())
            const float textWidth = 0.05f;
            const float textHeight = textWidth * 2f;
            if (Enemies.Count > 1)
            {
                Color color = Color.red.xA(0.5f);

                SegmentFont.Print(ClosestEnemy.Position + Vector2.down * textHeight * 1f, "Closest", color, textWidth);
                SegmentFont.Print(FurthestEnemy.Position + Vector2.down * textHeight * 1f, "Furthest", color, textWidth);

                SegmentFont.Print(StrongestEnemy.Position + Vector2.down * textHeight * 2f, "Strongest", color, textWidth);
                SegmentFont.Print(WeakestEnemy.Position + Vector2.down * textHeight * 2f, "Weakest", color, textWidth);


                Vector2 barycenter = EnemyBarycenter;
                DebugDraw.Circle(barycenter, 0.25f, color);
                SegmentFont.Print(barycenter, "Barycenter", color, textWidth);
            }


            //if (AlliesInSight())
            if (Allies.Count > 1)
            {
                Color color = Color.green.xA(0.5f);

                SegmentFont.Print(ClosestAlly.Position + Vector2.down * textHeight * 1f, "Closest", color, textWidth);
                SegmentFont.Print(FurthestAlly.Position + Vector2.down * textHeight * 1f, "Furthest", color, textWidth);

                SegmentFont.Print(StrongestAlly.Position + Vector2.down * textHeight * 2f, "Strongest", color, textWidth);
                SegmentFont.Print(WeakestAlly.Position + Vector2.down * textHeight * 2f, "Weakest", color, textWidth);


                Vector2 barycenter = AllyBarycenter;
                DebugDraw.Circle(barycenter, 0.25f, color);
                SegmentFont.Print(barycenter, "Barycenter", color, textWidth);
            }
        }
    }
}