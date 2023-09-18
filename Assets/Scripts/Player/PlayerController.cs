using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ITouchReceiver, IDamageable
{
    [Header("General Settings")]
    // public float movementSpeed = 1f;
    public Transform head;
    public float moveDelay = 0.2f;
    public float GridCellSize = 1f;

    [Header("Score System")]
    public CentralScore centralScore;

    [Header("Event Systems")]
    public GameEvent onPlayerHurtEvent;
    public GameEvent onPlayerDeathEvent;
    public GameEvent onPressInteractEvent;
    public GameEvent onSetLastInteractableEvent;


    [Header("Extension Settings")]
    [SerializeField] private int initialBodyParts = 3;
    [SerializeField] private GameObject snakeBodyBlockPrefab;
    [SerializeField] private int maxLimit = 10;
    public List<GameObject> bodyParts = new List<GameObject>();

    [Header("Body Shield Settings")]
    [SerializeField] private GameObject bodyShieldPrefab;


    [Header("Boundary Response")]
    public float avoidanceAngle = 90;
    public float raycastAngle = 30;
    public float raycastDistance = 1;

    private Vector2 movementDirection = Vector2.right;

    private float timeSinceLastMove;

    private Interactable lastInteractable;


    private void Start()
    {
        ExtendBodyLength(initialBodyParts);
        centralScore.SetLifeAmount(initialBodyParts);
    }

    private void Update()
    {
        HandleMovementInput();
        HandleKeyboardInteractInput();
    }

    private void FixedUpdate()
    {
        MoveSnake();
    }

    /// <summary>
    /// Detect player input for changing the movement direction
    /// Takes into consideration that a snake can't immediately 180 into itself, hence the check for not moving in opposite
    /// </summary>
    private void HandleMovementInput(){
        if (Input.GetAxis("Vertical") > 0 && movementDirection != Vector2.down)
        {
            movementDirection = Vector2.up;
        }
        else if (Input.GetAxis("Vertical") < 0 && movementDirection != Vector2.up)
        {
            movementDirection = Vector2.down;
        }
        else if (Input.GetAxis("Horizontal") < 0 && movementDirection != Vector2.right)
        {
            movementDirection = Vector2.left;
        }
        else if (Input.GetAxis("Horizontal") > 0 && movementDirection != Vector2.left)
        {
            movementDirection = Vector2.right;
        }
    }

    /// <summary>
    /// Creates movements of snake and position it will move to
    /// </summary>
    private void MoveSnake(){
        // Detect for boundaries first if there is anything blocking
        DetectBoundaries();

        // Update the time since the last movement
        timeSinceLastMove += Time.fixedDeltaTime;

        // Check if enough time has passed for the snake to move
        if (timeSinceLastMove >= moveDelay)
        {
            // Reset the time since the last movement
            timeSinceLastMove = 0f;

            // Store the previous position of the head
            Vector3 previousHeadPosition = head.position;

            Vector2 targetPosition = (Vector2) head.position + (movementDirection * GridCellSize);

            // Move the head position based on the movement direction
            head.position = targetPosition;

            if (bodyParts.Count > 0){
                // Move each body block to the position of the previous block
                for (int i = bodyParts.Count - 1; i > 0; i--)
                {
                    bodyParts[i].transform.position = bodyParts[i-1].transform.position;
                }

                // Move the first body block to the previous head position
                bodyParts[0].transform.position = previousHeadPosition;
            }  
        }
    }

    /// <summary>
    /// Adjust movement of the snake to take the next possible route if detecting a wall in front of it
    /// </summary>
    private void DetectBoundaries(){
        string[] layers = {"Wall", "Shield", "Map Edge", "Player"};
        LayerMask obstacleLayerMask = LayerMask.GetMask(layers);
        
        Vector2 checkPosition = (Vector2) head.position + movementDirection;

        RaycastHit2D hitForward = Physics2D.Raycast(checkPosition, Vector2.zero, raycastDistance, obstacleLayerMask.value);
        
        // FIXME: Might be able to make the player get into a deadend situation, make them die if the deadend themselves?
        if (hitForward.collider != null){
            RaycastHit2D hitRight = Physics2D.Raycast(head.position, Quaternion.Euler(0f, 0f, raycastAngle) * movementDirection, raycastDistance, obstacleLayerMask);
            RaycastHit2D hitLeft = Physics2D.Raycast(head.position, Quaternion.Euler(0f, 0f, -raycastAngle) * movementDirection, raycastDistance, obstacleLayerMask);

            if (hitRight.collider != null)
            {
                // Obstacle detected on the right, adjust movement direction to the left
                movementDirection = Quaternion.Euler(0f, 0f, -avoidanceAngle) * movementDirection;
            }
            else if (hitLeft.collider != null)
            {
                // Obstacle detected on the left, adjust movement direction to the right
                movementDirection = Quaternion.Euler(0f, 0f, avoidanceAngle) * movementDirection;
            }
        }        
    }

    /// <summary>
    /// Extend Body Length of Snake by a certain amount. 
    /// </summary>
    /// <param name="sizeAmount">Amount of body parts to be extended</param>
    private void ExtendBodyLength(int sizeAmount){
        if (bodyParts.Count >= maxLimit){
            return;
        }
        Transform lastBody;
        GameObject newPart;            

        // Check the number of body parts to add
        int bodyToAdd = bodyParts.Count + sizeAmount > maxLimit ? maxLimit - bodyParts.Count : sizeAmount;

        for (int i = 0; i < bodyToAdd; i++){
            lastBody = bodyParts.Count > 0 ? bodyParts[bodyParts.Count - 1].transform : head;
            newPart = Instantiate(snakeBodyBlockPrefab, head.parent);
            newPart.transform.SetParent(head.parent);
            newPart.tag = gameObject.tag;
            newPart.layer = gameObject.layer;
            newPart.transform.position = lastBody.transform.position;
            bodyParts.Add(newPart);    
        }

        centralScore.AddLifeAmount(bodyToAdd);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // If collided with food perform eating action
        if (other.transform.CompareTag("Food")){
            Destroy(other.gameObject);
            ExtendBodyLength(1);
        }
    }

    /// <summary>
    /// A function used to return the corresponding amount that actually got deposited
    /// </summary>
    /// <param name="depositAmount">Amount requested to be deposited</param>
    /// <returns>Actual amount tha can be deposited or used</returns>
    public int DepositBody(int depositAmount){
        if (bodyParts.Count <= 1){
            return 0;
        }

        int amountDeposited = depositAmount > bodyParts.Count - 1 ? bodyParts.Count - 1: depositAmount; 
        int startPos = depositAmount > bodyParts.Count ? 0 : bodyParts.Count - depositAmount;

        for (int i = startPos; i < startPos + amountDeposited; i++){
            GameObject bodyPart = bodyParts[i];
            Destroy(bodyPart);
        }
        
        bodyParts.RemoveRange(startPos, amountDeposited);    
        centralScore.ReduceLifeAmount(amountDeposited);    
        return amountDeposited;
    }

    /// <summary>
    /// Method that splits the shield by the certain amount and loads the corresponding logic
    /// </summary>
    /// <param name="splitAmount">Amount of body parts to split</param>
    private void SplitBody(int splitAmount){
        // Leave one Body Part Remaining At least
        if (bodyParts.Count <= 1){
            return;
        }
        // TODO: Need to toggle interaction key on and off

        int amountToSplit = splitAmount > bodyParts.Count  - 1 ? bodyParts.Count - 1 : splitAmount; 
        int startPos = splitAmount > bodyParts.Count ? 0 : bodyParts.Count - splitAmount;

        // Instantiate any needed part according to the situation
        for (int i = startPos; i < startPos + amountToSplit; i++){
            GameObject bodyPart = bodyParts[i];

            GameObject spawnedShield = Instantiate(bodyShieldPrefab, bodyPart.transform.position, Quaternion.identity);

            Destroy(bodyPart);
        }
        // Drop Body parts
        bodyParts.RemoveRange(startPos, amountToSplit);

        centralScore.ReduceLifeAmount(amountToSplit);
    }

    /// <summary>
    /// Takes Damage correspondingly
    /// </summary>
    /// <param name="damageAmount">Damage Amount to be taken</param>
    public bool TakeDamage(int damageAmount){
        if (damageAmount <= 0) return false;

        int damageToTake = damageAmount > bodyParts.Count ? bodyParts.Count : damageAmount;
        int startPos = damageAmount > bodyParts.Count ? 0 : bodyParts.Count - damageAmount;

        for (int i = startPos; i < bodyParts.Count; i++){
            Destroy(bodyParts[i]);
        }

        bodyParts.RemoveRange(startPos, damageToTake);

        onPlayerHurtEvent.TriggerEvent();
        centralScore.ReduceLifeAmount(damageToTake);

        if (bodyParts.Count <= 0){
            StartDeathEvent();
        }
        return true;
    }

    /// <summary>
    /// Triggers Player Death Events to the whole game
    /// </summary>
    public void StartDeathEvent(){
        // TODO: Create Player Death Animations
        Debug.Log("PLAYER IS DEAD");
        onPlayerDeathEvent.TriggerEvent();
    }

    /// <summary>
    /// Sets the object that should be interactable by the Player
    /// </summary>
    /// <param name="interactable">Can be null in the case where there is no interactable nearby</param>
    public void SetLastInteractable(Interactable interactable){
        lastInteractable = interactable;
        onSetLastInteractableEvent.TriggerEvent();
        // TODO: Maybe double check that there must be an interactable and that the UI is properly shown
    }

    /// <summary>
    /// Performs any interaction of the object that the player is in reach of, if there is any
    /// </summary>
    public void OnPressInteract(){
        // If there are interactables interact, interact first
        if (lastInteractable != null){
            lastInteractable.Interact();
            onPressInteractEvent.TriggerEvent();
        }  
        else {
            // TODO: Show when body can be split on interact button
            SplitBody(1);
        }
    }

    /// <summary>
    /// Reads the input of dropping, if true then triggers it
    /// </summary>
    private void HandleKeyboardInteractInput(){
        if (Input.GetKeyDown(KeyCode.Space)) {
            OnPressInteract();
            // TODO: Figure out user input on handling how to drop multiple at one time
        }
    }

    // SWIPE CONTROLS
    public void ReceiveSwipeControls(SwipeDirection swipeDirection)
    {
        switch (swipeDirection){
            case SwipeDirection.UP:
                movementDirection = Vector2.up;
                break;
            case SwipeDirection.DOWN:
                movementDirection = Vector2.down;
                break;
            case SwipeDirection.LEFT:
                movementDirection = Vector2.left;
                break;
            case SwipeDirection.RIGHT:
                movementDirection = Vector2.right;
                break;
        }
    }

    public void ReceiveTapControls(TapType tapType)
    {
        switch (tapType){
            case TapType.SINGLE:
                OnPressInteract();
                break;
        }
    }
}
