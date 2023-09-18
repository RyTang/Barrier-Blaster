using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITouchReceiver
{
    public void ReceiveSwipeControls(SwipeDirection swipeDirection);

    public void ReceiveTapControls(TapType tapType);
}
