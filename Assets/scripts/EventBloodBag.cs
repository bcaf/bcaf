using UnityEngine;

class EventBloodBag : Event {
    const int STATE_DETACHED = 0;
    const int STATE_ATTACHED = 1;
    
    const float START_BLOOD_AMOUNT = 750.0F; //in ml
    int state;
    float bloodAmountLeftInBag;
    
    public EventBloodBag(Body body_) {
        //Event(body_);
		body = body_;
        
        bloodAmountLeftInBag = START_BLOOD_AMOUNT;
        
        state = STATE_DETACHED;
    }
    
    
    //Unity's Update function
    void Update() {
        Fiducial bloodBagFiducial = body.fs[Body.FID_BLOODBAG];
        if (bloodBagFiducial.active) {
            float distanceToEvent = (bloodBagFiducial.position - this.position).magnitude;
            if (distanceToEvent > 0.5F) {
                Debug.Log("Blood Bag is placed, but too far away, at distance " +
                        distanceToEvent);
            } else {
                float bloodAmountPerSecond = (bloodAmountLeftInBag / START_BLOOD_AMOUNT) * 50.0F;
                this.body.bloodAmount += bloodAmountPerSecond * Time.deltaTime;
            }
        }
        
        draw();
    }
    
    //TODO: Add visualization of the blood bag somehow?
    void draw() {
    }
    
    //this is called whenever gameOver happens.  Cleanly stop current animations and things like that.
    public override void gameOverSignal() {
    }
}
