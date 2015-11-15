class EventCutOpen : Event {
    const int STATE_NEEDS_BACTERIAL = 0;
    const int STATE_CUTTING = 1;
    const int STATE_FINAL = 10; //if reached, please remove me!
    int state;
    
    float bacJelAmount; //in cl
    const float BACJEL_ADDED_PER_SECOND = 3.0F;
    float bacJelAmountNeeded;
    
    public EventCutOpen(Body body_) {
        Event(body_);
        //body = body_;
        
        state = STATE_NEEDS_BACTERIAL;
        bacJelAmount = 0.0F;
        bacJelAmountNeeded = 20.0F + 10.0F*body.numPlayers;
    }
    
    void Update() {
        if (state == STATE_NEEDS_BACTERIAL) {
            Fiducial bacJelFid = body.fs[Body.FID_BACTERIALJEL];
            if (bacJelFid.active) {
                bacJelAmount += BACJEL_ADDED_PER_SECOND * Time.deltaTime;
                
                if (bacJelAmount >= bacJelAmountNeeded) {
                    state = STATE_CUTTING;
                    return;
                }
                
                drawNeedsBacterial();
            }
        } else if (state == STATE_CUTTING) {
            Fiducial scalpel = body.fs[Body.FID_SCALPEL];
            
            if (scalpel.active) {
                
                if (false) {
                    state = STATE_FINAL;
                }
            }
            
            drawIsCutting();
        }
        
        //float bloodDiff = -1.0F;
        //body.bloodAmount += bloodDiff;
        //Debug.Log("CutOpenEvent changed bloodAmount with" + bloodDiff + "(" + body.bloodAmount + "cl left");
    }
    
    //TODO: Show some type of animation showing the amount of bacterial jel applied slowly filling up?
    void drawNeedsBacterial() {
        float ratio = bacJelAmount / bacJelAmountNeeded; //might be useful?
    }
    
    void drawIsCutting() {
    }
    
    //this is called whenever gameOver happens.  Cleanly stop current animations and things like that.
    public override void gameOverSignal() {
    }
}
