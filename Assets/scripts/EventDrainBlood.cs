using UnityEngine;
using UnityEngine.UI;

class EventDrainBlood : Event {
    const int STATE_UNDRAINED = 0;
    const int STATE_DRAINED = 0;

    Image progressBar;
    ParticleSystem ps;
    float psEmissionRate;
    
    int state;
    
    float bloodAmount;
    const float bloodRemovedPerSecond = 2.0F;
    float bloodDrained = 0.0F;
    float bloodloss;
    
    public EventDrainBlood(Body body_) {
        //Event(body_);
		body = body_;
        state = STATE_UNDRAINED;
        bloodAmount = 20.0F + 10.0F*body.numPlayers;
        progressBar = gameObject.GetComponent<Image>();
        ps = gameObject.GetComponent<ParticleSystem>();
        psEmissionRate = ps.emissionRate;
    }

    void Start()
    {
        //body = GameObject.Find("Main Camera").GetComponent<Body>();
        state = STATE_UNDRAINED;
        bloodAmount = 20.0F + 10.0F * body.numPlayers;
        progressBar = gameObject.GetComponent<Image>();
        ps = gameObject.GetComponent<ParticleSystem>();
        psEmissionRate = ps.emissionRate;
        bloodloss = 5.0F;        
    }    
    
    //Unity's Update function
    void Update() {
        
        if (state == STATE_UNDRAINED) {
            Fiducial syringe = body.getFiducial(Body.FID_SYRINGE);
            if (syringe.active) {
                float distanceToEvent = (syringe.position - this.get2dPositionAsVector3()).magnitude;
                if (distanceToEvent < 20.0F) {
                    Debug.Log("Syringe is placed, but too far away, at distance " +
                            distanceToEvent);
                } else {
                    this.bloodDrained += bloodRemovedPerSecond * Time.deltaTime;
                    //Debug.Log("Drained blood: " + bloodDrained);
                }
            }
            body.decreaseBloodAmount(bloodloss);
        }
        
        if (this.bloodDrained >= this.bloodAmount) { //Initiate drainBloodEvent or SutureEvent)
            state = STATE_DRAINED;
            progressBar.enabled = false;
            if (body.chance(0.7F)) {
                //this.body.events.Add(new EventDrainPuss());
            } else {
                //this.body.events.Add(new EventSuture());
            }
            Destroy(this);
        }

        
        //Debug.Log("DrainBlood: " + body.bloodAmount + ", decrease: " + (bloodloss));

        draw();
    }
    
    //TODO: Add visualization of the blood draining
    void draw() {
        float ratio = bloodDrained / bloodAmount;
        ps.emissionRate = psEmissionRate * (1 - ratio);
        progressBar.fillAmount = ratio;
    }
    
    //this is called whenever gameOver happens.  Cleanly stop current animations and things like that.
    public override void gameOverSignal() {
    }
}
