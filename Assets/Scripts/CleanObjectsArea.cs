using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanObjectsArea : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {   
        // TODO: Consider if there are situations where thing should not be destroyed
        if (other.gameObject.layer == (int) LayerName.CLEANING_EDGE) return;
        Destroy(other.gameObject);
    }
}
