using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeControls : MonoBehaviour
{
    public GameObject objectReceivingInput;
    [SerializeField] private ITouchReceiver receiver;
    public float holdDurationThreshold;
    private bool fingerDown;
    private int primaryTouch;
    private float holdDuration;

    public bool FingerDown {
        get {return fingerDown;}
        private set {fingerDown = value;}
    }

    public float swipeDistanceThreshold = 30f;
    private Vector2 startPos;

    public void Start()
    {
        receiver = objectReceivingInput.GetComponent<ITouchReceiver>();
        Debug.Assert(receiver != null, objectReceivingInput + " does not have an ITouchReceiever Interface");
    }

    public void Update() {
        DetectFingerMotion();
    }

    private void DetectFingerMotion(){
        // If it is not a touch
        if (Input.touchCount <= 0){
            fingerDown = false;
            holdDuration = 0f;
            return;
        }        

        holdDuration += Time.deltaTime;

        // If touch record results
        if (!fingerDown){
            startPos = Input.touches[0].position;
            primaryTouch = Input.touches[0].fingerId;
            fingerDown = true;
        }
        // Track that it's the same finger
        else if (fingerDown && Input.touches[0].fingerId == primaryTouch && Input.touches[0].phase == TouchPhase.Ended) {
            // Caculate swipe distance
            Vector2 endPos = Input.touches[0].position;
            float swipeDistance = Vector2.Distance(startPos, endPos);

            // If a dragging animation
            if (swipeDistance >= swipeDistanceThreshold) {
                // Identify swipe direction
                bool isXDirection = Mathf.Abs(endPos.x - startPos.x) > Mathf.Abs(endPos.y - startPos.y);
                SwipeDirection direction;
                if (isXDirection){
                    direction = endPos.x > startPos.x ? SwipeDirection.LEFT : SwipeDirection.RIGHT;
                }
                else{
                    direction = endPos.y > startPos.y ? SwipeDirection.DOWN : SwipeDirection.UP;
                }
                receiver.ReceiveSwipeControls(direction);
                fingerDown = false;
            }
            // Else if holding tap
            else{
                receiver.ReceiveTapControls(TapType.SINGLE);
            }

            fingerDown = false;
            holdDuration = 0f;
        }
        
        
        
        
        // TODO: Build a receiver for a Double Tap
        
    }
}

public enum SwipeDirection {
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public enum TapType {
    SINGLE,
    DOUBLE
}
