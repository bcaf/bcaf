
class EventDrainBlood : Event {
    const int STATE_UNDRAINED = 0;
    const int STATE_DRAINED = 0;
    
    int state;
    
    float bloodAmount = 20.0F + 10.0F*body.numPlayers;
    const float bloodRemovedPerSecond = 2.0F;
    
    public EventDrainBlood(Body body_) {
        //Event(body_);
		body = body_;
        state = STATE_UNDRAINED;
    }
    
    
    //Unity's Update function
    void Update() {
        if (state == STATE_UNDRAINED) {
            Fiducial syringe = body.fs[Body.FID_SYRINGE];
            if (syringe.active) {
                float distanceToEvent = (syringe.position - this.position).magnitude;
                if (distanceToEvent > 0.5F) {
                    Debug.Log("Syringe is placed, but too far away, at distance " +
                            distanceToEvent);
                } else {
                    this.bloodAmount -= this.bloodRemovedPerSecond * Time.deltaTime;
                }
            }
        }
        
        if (this.bloodAmount <= 0) { //Initiate drainBloodEvent or SutureEvent)
            state = STATE_DRAINED;
            if (body.chance(0.7F)) {
                //this.body.events.Add(new EventDrainPuss());
            } else {
                //this.body.events.Add(new EventSuture());
            }
        }
        
        draw();
    }
    
    //TODO: Add visualization of the blood draining
    void draw() {
    }
    
    //this is called whenever gameOver happens.  Cleanly stop current animations and things like that.
    public override void gameOverSignal() {
    }
}
