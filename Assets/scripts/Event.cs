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
    const int STATE_CUTTING = 0;
    int state;
    
    float bacJelAmount; //in cl
    const float BACJEL_ADDED_PER_SECOND = 0.2;
    float bacJelAmountNeeded;
    
    public CutOpenEvent(Body body_) {
            body = body_;
            state = STATE_NEEDS_BACTERIAL;
            bacJelAmount = 0.0F;
            bacJelAmountNeeded = 2.0F + 1.0*body.numPlayers;
    }
        
    void Update() {
        if (state == STATE_NEEDS_BACTERIAL) {
            Fiducial bacJelFid = body.fs[body.BACTERIALJEL];
            if (bacJelFid.active) {
                bacJelAmount += BACJEL_ADDED_PER_SECOND * Time.deltaTime;
                
                if (bacJelAmount > bacJelAmountNeeded) {
                    state = STATE_CUTTING;
                    return;
                }
                
                drawNeedsBacterial();
            }
        } else if (state == STATE_CUTTING) {
            drawIsCutting();
        }
        
        float bloodDiff = -1.0F;
        body.bloodAmount += bloodDiff;
        Debug.Log("CutOpenEvent changed bloodAmount with" + bloodDiff + "(" + bloodAmount + "cl left");
    }
    
    void drawNeedsBacterial() {
    }
    
    void drawIsCutting() {
    }
}
