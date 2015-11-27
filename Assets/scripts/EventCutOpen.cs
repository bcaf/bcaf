using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

class EventCutOpen : Event {
	Vector3 position;
    const int STATE_NEEDS_BACTERIAL = 0;
    const int STATE_CUTTING = 1;
    const int STATE_FINAL = 10; //if reached, please remove me!
    int state;
    
    float bacGelAmount; //in cl
    const float BACJEL_ADDED_PER_SECOND = 12.0F;
    float bacGelAmountNeeded;

    float scalpelAmount;
    const float SCALPEL_ADDED_PER_SECOND = 12.0F;
    float scalpelAmountNeeded;

	List<Vector2> cutPoints = new List<Vector2>();
	Vector2 cutStartPosition;
	Vector2 cutEndPosition;
    
    public EventCutOpen(Body body_, Vector3 position_) {
        //Event(body_);
        this.body = body_;
		this.position = position_;
        
        state = STATE_NEEDS_BACTERIAL;
        bacGelAmount = 0.0F;
        //bacGelAmountNeeded = 20.0F + 10.0F*body.numPlayers;
        bacGelAmountNeeded = 30.0F;

        scalpelAmount = 0.0F;
        scalpelAmountNeeded = 50.0F;

		Debug.Log("EventCutOpen was created!");
    }
    
    public void update() {
		//Debug.Log("In EventCutOpen.update(), state = " + state.ToString());

        if (state == STATE_NEEDS_BACTERIAL) {
            //Fiducial bacGelFid = body.getFiducial(Body.FID_BACTERIALGEL);
            Fiducial bacGelFid = body.getFiducial(Body.FID_BACTERIALGEL);

            if (bacGelFid.active) {
                bacGelAmount += BACJEL_ADDED_PER_SECOND * Time.deltaTime;
                
                if (bacGelAmount >= bacGelAmountNeeded) {
                    state = STATE_CUTTING;
                    return;
                }
                
                drawNeedsBacterial();
				//Debug.Log("Right after drawNeedsBacterial in if(bacJelFid.active())");
            } else {
				//Debug.Log("bacJelFid.active is false");
			}
        } else if (state == STATE_CUTTING) {
            //Fiducial scalpel = body.getFiducial(Body.FID_SCALPEL);
            Fiducial scalpel = body.getFiducial(Body.FID_SCALPEL);
            
            if (scalpel.active) {

                if ((scalpel.position - this.position).magnitude < 20) {
                    scalpelAmount += SCALPEL_ADDED_PER_SECOND * Time.deltaTime;
                }

                
				if (scalpelAmount >= scalpelAmountNeeded) {
                    state = STATE_FINAL;

					if (body.chance(0.7F)) {
						if (body.chance(0.5F)) {
                            Debug.Log("Changed event to EventDrainPuss");
							//this.body.events.Add(new EventDrainPuss());
						} else {
                            Debug.Log("Changed event to EventDrainBlood");
							//this.body.events.Add(new EventDrainBlood());
						}
					} else {
                        Debug.Log("Changed event to EventApplyScrewsToBones");
						//this.body.events.Add(new EventApplyScrewsToBones());
					}
                }
            }

			body.bloodAmount -= 1.0F * Time.deltaTime;
            
            drawIsCutting();
        } else if (state == STATE_FINAL) {


			body.bloodAmount -= 1.0F * Time.deltaTime;
		} else {
			Debug.LogWarning("In EventCutOpen, unknown state:" + state.ToString());
		}
        
        //Debug.Log("CutOpenEvent changed bloodAmount with" + bloodDiff + "(" + body.bloodAmount + "cl left");
    }
    
    //TODO: Show some type of animation showing the amount of bacterial jel applied slowly filling up?
    void drawNeedsBacterial() {
        float ratio = bacGelAmount / bacGelAmountNeeded; //might be useful?

		//Debug.Log("BacGelAmount ratio: " + ratio.ToString());

		GameObject.Find("label_cutopenevent").GetComponent<Text>().text =
			"Bacterial Gel: " + (100.0F*ratio).ToString() + "%";
    }
    
    void drawIsCutting() {
		//TODO: add line showing area to cut and plot the last 50 or so cutPoints (as lines?)
    }
    
    //this is called whenever gameOver happens.  Cleanly stop current animations and things like that.
    public override void gameOverSignal() {
    }
}
