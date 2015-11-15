using UnityEngine;

class Event : MonoBehaviour {
    bool active;
    protected Body body;

    public Event() {}
    
    public Event(Body body_) {
        body = body_;
    }
    
    void Update() {
        //body.bloodAmount -= 1.0F;
    }
    
    /*this is called whenever gameOver happens.
    Cleanly stop current animations and things like that.*/
    public virtual void gameOverSignal() {
    }
}

/* Copy this template thingy whenever you create a new sub event (that you'll create in a new file)
class EventSubClass : Event {
    public SubEventEvent(Body body_) {
        Event(body_);
    }
    
    void drawState0() {
    }
    
    //Unity's Update function
    void Update() {
        //Fiducial bacJelFid = body.fs[Body.FID_BACTERIALJEL];
    }
    
    //this is called whenever gameOver happens.  Cleanly stop current animations and things like that.
    public override void gameOverSignal() {
    }
}
*/
