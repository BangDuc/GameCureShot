using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace Bang.Lib.Behaviour_Tree
{
    #region Core BT
    public enum NodeState { Success, Failure, Running }


    public abstract class BTNode
    {
        public abstract NodeState Tick();

    }
    public abstract class CompositeNode : BTNode
    {
        protected List<BTNode> children = new();
        public void AddChild(BTNode child) => children.Add(child);
    }

    public class SequenceNode : CompositeNode
    {

        public override NodeState Tick()
        {
            
            for (int i = 0; i < children.Count; i++)
            {
                var state = children[i].Tick();

                if (state == NodeState.Running)
                {
                    
                    return NodeState.Running;
                }

                if (state == NodeState.Failure)
                {
                    return NodeState.Failure;
                }
            }
            return NodeState.Success;
        }
    }

    public class SelectorNode : CompositeNode
    {
        
        public override NodeState Tick()
        {
            for (int i = 0; i < children.Count; i++)
            {
                var state = children[i].Tick();
                if (state == NodeState.Success)
                {
                    return NodeState.Success;
                }
                if (state == NodeState.Running)
                {
                    return NodeState.Running;
                }
            }
            
            return NodeState.Failure;
        }
    }
    #endregion

    #region Leaf Nodes (Condition & Action)
    public class ConditionNode : BTNode
    {
        private Func<bool> condition;
        public ConditionNode(Func<bool> cond) { condition = cond; }
        public override NodeState Tick() => condition() ? NodeState.Success : NodeState.Failure;
    }

    
    public class ActionNode : BTNode
    {
        private Func<NodeState> action;
        public ActionNode(Func<NodeState> act) { action = act; }
        public override NodeState Tick() => action();
    }
    #endregion

    #region Study
    //public class CheckPlayerInRange : BTNode
    //{
    //    private Transform ai, player;
    //    private float range;

    //    public CheckPlayerInRange(Transform ai, Transform player, float range)
    //    {
    //        this.ai = ai;
    //        this.player = player;
    //        this.range = range;
    //    }

    //    public override NodeState Evaluate()
    //    {
    //        float dist = Vector3.Distance(ai.position, player.position);
    //        return dist <= range ? NodeState.Success : NodeState.Failure;
    //    }
    //}
    //public class ChasePlayer : BTNode
    //{
    //    private Transform ai, player;
    //    private float speed;

    //    public ChasePlayer(Transform ai, Transform player, float speed)
    //    {
    //        this.ai = ai;
    //        this.player = player;
    //        this.speed = speed;
    //    }

    //    public override NodeState Evaluate()
    //    {
    //        ai.position = Vector3.MoveTowards(ai.position, player.position, speed * Time.deltaTime);
    //        return NodeState.Running;
    //    }
    //}
    //public class Patrol : BTNode
    //{
    //    private Transform ai;
    //    private Vector3[] points;
    //    private int index;
    //    private float speed;

    //    public Patrol(Transform ai, Vector3[] points, float speed)
    //    {
    //        this.ai = ai;
    //        this.points = points;
    //        this.speed = speed;
    //        index = 0;
    //    }

    //    public override NodeState Evaluate()
    //    {
    //        if (Vector3.Distance(ai.position, points[index]) < 0.1f)
    //            index = (index + 1) % points.Length;

    //        ai.position = Vector3.MoveTowards(ai.position, points[index], speed * Time.deltaTime);
    //        return NodeState.Running;
    //    }
    //}
    //public class AIController : MonoBehaviour
    //{
    //    public Transform player;
    //    public float detectRange = 5f;
    //    public float speed = 2f;
    //    public Vector3[] patrolPoints;

    //    private BTNode root;

    //    void Start()
    //    {
    //        var checkPlayer = new CheckPlayerInRange(transform, player, detectRange);
    //        var chase = new ChasePlayer(transform, player, speed);
    //        var patrol = new Patrol(transform, patrolPoints, speed);

    //        root = new Selector(
    //            new Sequence(checkPlayer, chase),
    //            patrol
    //        );
    //    }

    //    void Update()
    //    {
    //        root.Evaluate();
    //    }
    //}
    #endregion
}
