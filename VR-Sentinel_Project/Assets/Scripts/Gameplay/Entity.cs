using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region UnityInspector

    public enum Type
    {
        Default,
        AbsorbableObject,
        Sentinel,
    }

    public Type type;

    #endregion
}
