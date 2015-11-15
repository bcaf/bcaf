class Event : MonoBehaviour {
    bool active;
    Body body;
    
    Event(ref Body body_) {
        body = body_;
    }
    
    void Update() {
        body.bloodAmount -= 1.0;
    }
    
    /*this is called whenever gameOver happens.
    Cleanly stop current animations and things like that.*/
    void gameOverSignal() {
    }
}

//Each separate event subclass should be placed in it's own script,
//    on a certain part of the body
class CutOpenEvent : Event {
}
