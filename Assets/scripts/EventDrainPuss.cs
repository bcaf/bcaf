using UnityEngine;
using UnityEngine.UI;

class EventDrainPuss : Event {
    const int STATE_UNDRAINED = 0;
    const int STATE_DRAINED = 0;
    
    int state;

    Image progressBar;
    ParticleSystem ps;
    float psEmissionRate;
    
    float pussAmount;
    const float pussRemovedPerSecond = 2.0F;
    float pussDrained = 0.0F;
    
    public EventDrainPuss(Body body_) {
        //Event(body_);
		body = body_;
        state = STATE_UNDRAINED;
        pussAmount = 20.0F + 10.0F * body.numPlayers;
    }

    void Start()
    {
        state = STATE_UNDRAINED;
        pussAmount = 20.0F + 10.0F * body.numPlayers;
        progressBar = gameObject.GetComponent<Image>();
        ps = gameObject.GetComponent<ParticleSystem>();
        psEmissionRate = ps.emissionRate;
    }
    
    
    //Unity's Update function
    void Update() {
        if (state == STATE_UNDRAINED) {
            Fiducial syringe = body.getFiducial(Body.FID_SYRINGE);
            if (syringe.active) {
                float distanceToEvent = (syringe.position - this.position).magnitude;
                if (distanceToEvent > 0.5F) {
                    Debug.Log("Syringe is placed, but too far away, at distance " +
                            distanceToEvent);
                } else {
                    this.pussDrained += pussRemovedPerSecond * Time.deltaTime;
                }
            }
        }
        
        if (this.pussDrained >= this.pussAmount) { //Initiate drainBloodEvent or SutureEvent)
            state = STATE_DRAINED;
            if (body.chance(0.7F)) {
                //this.body.events.Add(new EventDrainBlood());
            } else {
                //this.body.events.Add(new EventSuture());
            }
        }
        
        draw();
    }
    
    //TODO: Add visualization of the puss draining
    void draw() {
        float ratio = pussDrained / pussAmount;
        ps.emissionRate = psEmissionRate * (1 - ratio);
        progressBar.fillAmount = ratio;
    }
    
    //this is called whenever gameOver happens.  Cleanly stop current animations and things like that.
    public override void gameOverSignal() {
    }
}
