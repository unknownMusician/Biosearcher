using System;
using UnityEngine;

namespace Biosearcher.Player
{
    public interface IInsertFriendly
    {
        Type[] GetInsertableType();
        Vector3 GetAlignmentPosition();

        void Insert(IInsertable insertable);
    }
}
