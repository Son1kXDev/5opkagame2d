using System;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
namespace Enjine
{
    public class Enemy : Health
    {

        [field: Header("Config")]
        [field: SerializeField, StatusIcon] public EnemyData Data { get; private set; }

        [field: Header("Patrol System")]
        [field: SerializeField] public List<Transform> PatrolPoints { get; private set; }

        public EnemyStateMachine StateMachine { get; private set; }
        public EnemyPatrolState PatrolState { get; private set; }
        public EnemyFollowState FollowState { get; private set; }
        public EnemySearchState SearchState { get; private set; }
        public EnemyAttackState AttackState { get; private set; }
        public Transform Player { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }
        public Collider2D Collider { get; private set; }
        public Path Path { get; set; }
        public Seeker Seeker { get; private set; }

        private void OnValidate()
        {
            PatrolPoints ??= new List<Transform>();
            if (Seeker == null)
                if (TryGetComponent(out Seeker _) == false)
                    Seeker = gameObject.AddComponent(typeof(Seeker)) as Seeker;
                else Seeker = GetComponent<Seeker>();

            if (Rigidbody == null)
                if (TryGetComponent(out Rigidbody2D _) == false)
                    Rigidbody = gameObject.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
                else Rigidbody = GetComponent<Rigidbody2D>();

            if (Collider == null)
                if (TryGetComponent(out Collider2D _) == false)
                    Collider = gameObject.AddComponent(typeof(CircleCollider2D)) as Collider2D;
                else Collider = GetComponent<Collider2D>();
        }

        private void Awake() => InitializeEnemy();

        public void InitializeEnemy()
        {
            if (Data == null)
                throw new NullReferenceException("Enemy Config is null");

            InitializeHealth(Data.MaximumHealth);

            Player = GameObject.FindGameObjectWithTag("Player").transform;

            StateMachine = new EnemyStateMachine();
            PatrolState = new EnemyPatrolState(this, StateMachine);
            FollowState = new EnemyFollowState(this, StateMachine);
            SearchState = new EnemySearchState(this, StateMachine);
            AttackState = new EnemyAttackState(this, StateMachine);

            StateMachine.Initialize(PatrolState);

            InvokeRepeating(nameof(UpdateEnemy), 0f, Data.PathUpdateSeconds);
        }

        public void UpdateEnemy() => StateMachine.CurrentEnemyState.UpdateState();

        protected override void FixedRun() => StateMachine.CurrentEnemyState.FixedUpdateState();

        protected override void VisualizeHealth()
        {
            Debug.SetColor(Color.blue);
            Debug.Log("Taking damage! Current Health: " + _currentHealth);
        }

        protected override void OnDeath()
        {
            Destroy(transform.parent.gameObject);
            Debug.Log("Enemy died!");
        }

        private void OnDrawGizmos()
        {
            if (Data == null) return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, Data.ActivationDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Data.AttackDistance);
        }
    }
}
