using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Biosearcher.Player.Interactions.Default
{
    public class Insertable : Grabbable, IInsertable
    {
        public event Action<IInsertFriendly>? OnInsert;
        public event Action<IInsertFriendly>? OnAlignStart;
        public event Action<IInsertFriendly>? OnAlignEnd;
        
        public Insertable(Rigidbody rigidbody, Collider collider) : base(rigidbody, collider) { }
        
        public bool CanBeInserted(IInsertFriendly insertFriendly) => true;

        public void HandleInsert(IInsertFriendly insertFriendly)
        {
            SetPhysicsActive(false);
            
            OnInsert?.Invoke(insertFriendly);
        }

        public void HandleAlignStart(IInsertFriendly insertFriendly)
        {
            OnAlignStart?.Invoke(insertFriendly);
        }

        public void HandleAlignEnd(IInsertFriendly insertFriendly)
        {
            OnAlignEnd?.Invoke(insertFriendly);
        }
    }
    
    public abstract class InsertableMonoBehaviour : GrabbableMonoBehaviour, IInsertable
    {
        public event Action<IInsertFriendly> OnInsert;
        public event Action<IInsertFriendly> OnAlignStart;
        public event Action<IInsertFriendly> OnAlignEnd;

        public bool CanBeInserted(IInsertFriendly insertFriendly) => true;

        public void HandleInsert(IInsertFriendly insertFriendly)
        {
            SetPhysicsActive(false);
            
            OnInsert?.Invoke(insertFriendly);
        }

        public void HandleAlignStart(IInsertFriendly insertFriendly)
        {
            OnAlignStart?.Invoke(insertFriendly);
        }

        public void HandleAlignEnd(IInsertFriendly insertFriendly)
        {
            OnAlignEnd?.Invoke(insertFriendly);
        }
    }
}