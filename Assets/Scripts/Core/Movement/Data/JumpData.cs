using System;
using UnityEngine;

namespace Core.Movement.Data
{
    [Serializable]
    public class JumpData
    {
    
        [field: SerializeField] public float JumpForce;
        [field: SerializeField] public float GravityScale;

    }
}