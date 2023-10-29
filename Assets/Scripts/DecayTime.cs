using System.Globalization;
using System.Net.Security;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For any objects that needs to have an auto timer to be destroyed
/// </summary>
public class DecayTime : MonoBehaviour
{
    [SerializeField] protected float destroyTime;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
