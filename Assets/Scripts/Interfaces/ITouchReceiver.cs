using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Any Object that wants to receive touch events will need to implement this interface to receive the events
/// </summary>
public interface ITouchReceiver
{
    public void ReceiveSwipeControls(SwipeDirection swipeDirection);

    public void ReceiveTapControls(TapType tapType);
}
