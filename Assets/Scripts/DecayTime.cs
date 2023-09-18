using System.Globalization;
using System.Net.Security;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayTime : MonoBehaviour
{
    [SerializeField] protected float destroyTime;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
