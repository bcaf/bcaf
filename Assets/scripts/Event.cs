using UnityEngine;

class Event : MonoBehaviour {
    bool active;
    protected Body body;

    public Event() {}
    
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
    const int STATE_NEEDS_BACTERIAL = 0;
    const int STATE_CUTTING = 1;
    const int STATE_FINAL = 10; //if reached, please remove me!
    int state;
    
    float bacJelAmount; //in cl
    const float BACJEL_ADDED_PER_SECOND = 0.3F;
    float bacJelAmountNeeded;
    
    public CutOpenEvent(Body body_) {
        body = body_;
        
        state = STATE_NEEDS_BACTERIAL;
        bacJelAmount = 0.0F;
        bacJelAmountNeeded = 2.0F + 1.0F*body.numPlayers;
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
        
        float bloodDiff = -1.0F;
        body.bloodAmount += bloodDiff;
        Debug.Log("CutOpenEvent changed bloodAmount with" + bloodDiff + "(" + body.bloodAmount + "cl left");
    }
    
    //TODO: Show some type of animation showing the amount of bacterial jel applied slowly filling up?
    void drawNeedsBacterial() {
        float ratio = bacJelAmount / bacJelAmountNeeded; //might be useful?
    }
    
    void drawIsCutting() {
    }
}
