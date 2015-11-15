using UnityEngine;

class Event : MonoBehaviour {
    bool active;
    protected Body body;

	public Event(){}
    
    public Event(Body body_) {
        body = body_;
    }
    
    void Update() {
        body.bloodAmount -= 1.0F;
    }
    
    /*this is called whenever gameOver happens.
    Cleanly stop current animations and things like that.*/
    public void gameOverSignal() {
    }
}

//Each separate event subclass should be placed in it's own script,
//    on a certain part of the body


class CutOpenEvent : Event {
	public CutOpenEvent(Body body_) {
		body = body_;
	}
}
