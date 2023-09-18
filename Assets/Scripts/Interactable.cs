using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class that all objects that wants to be interactable should Inherit.
/// It is a monobehaviour
/// Interact shold be overwritten
/// </summary>
public abstract class Interactable : MonoBehaviour, IInteractable
{
    [Header("Interactable Settings")]
    public bool isTriggerInstant;

    public PlayerController playerController;    
    protected bool hasInteracted;
    protected GameObject entity;

    /// <summary>
    /// The player controller will call this function and perform the interaction
    /// Checks that the interaction should occur
    /// Calls InteractMechanism()
    /// </summary>
    public void Interact(){
        if (entity == null || hasInteracted){
            return;
        }
        InteractionMechanism();
    }

    /// <summary>
    /// This function should be overwritten by any classes that inherits it. 
    /// Performs whatever mechanism that the interactable needs to be done
    /// </summary>
    public abstract void InteractionMechanism();

    /// <summary>
    /// Returns the current entity that is being interacted with
    /// </summary>
    /// <returns></returns>
    public virtual GameObject GetCurrentEntity(){
        return entity;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        // If interacting with player then trigger
        if (other.CompareTag(TagName.PLAYER) && !hasInteracted){
            entity = this.gameObject;
            playerController.SetLastInteractable(this);
        }
        // If meant to trigger instantly then perform action after setting
        if (isTriggerInstant){
            playerController.OnPressInteract();
        }
    }

    public virtual void OnTriggerStay2D(Collider2D other)
    {
        // If Interacting with Player
        if (other.CompareTag(TagName.PLAYER) && !hasInteracted) {
            entity = this.gameObject;
            playerController.SetLastInteractable(this);
        } 
        // Else if interaction has been completed
        else if(other != null && hasInteracted)
        { 
            entity = null;
            playerController.SetLastInteractable(null);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
        // Remove from last interactable
        entity = null;
        playerController.SetLastInteractable(null);
        Debug.Log("Player Has Left this interactable: " + this);
    }
}
