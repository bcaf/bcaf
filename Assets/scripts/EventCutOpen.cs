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

	List<Vector2> cutPoints = new List<Vector2>();
	Vector2 cutStartPosition;
	Vector2 cutEndPosition;
    
    public EventCutOpen(Body body_, Vector3 position_) {
        //Event(body_);
        this.body = body_;
		this.position = position_;
        
        state = STATE_NEEDS_BACTERIAL;
        bacGelAmount = 0.0F;
        bacGelAmountNeeded = 20.0F + 10.0F*body.numPlayers;

		//Cut going in x direction, from -5 to +5
		cutStartPosition = this.get2dPosition() + new Vector2(-5.0F, 0.0F);
		cutEndPosition = this.get2dPosition() + new Vector2(5.0F, 0.0F);

		Debug.Log("EventCutOpen was created!");
    }
    
    public void update() {
		//Debug.Log("In EventCutOpen.update(), state = " + state.ToString());

        if (state == STATE_NEEDS_BACTERIAL) {
            Fiducial bacGelFid = body.fs[Body.FID_BACTERIALGEL];
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
            Fiducial scalpel = body.fs[Body.FID_SCALPEL];
            
            if (scalpel.active) {
				if (cutPoints.Count == 0 || !cutPoints[cutPoints.Count - 1].Equals(scalpel.get2dPosition())) {
					cutPoints.Add(scalpel.get2dPosition());
					if (cutPoints.Count > 600) { //don't store too many points
						cutPoints.RemoveRange(0, 300);
					}
				}

				int numCloseCutPoints = 0;
				Vector2 midCutPosition = cutStartPosition + 0.5F * (cutStartPosition - cutEndPosition);
				for (int i = 0; i < Mathf.Min(cutPoints.Count, 100); i++) {

					if ((cutPoints[i] - midCutPosition).magnitude < 350.0F) {
						numCloseCutPoints += 1;
					}
				}
				if (cutPoints.Count > 0) {
					Debug.Log("dist to last cutPoint:" + 
				          (cutPoints[cutPoints.Count-1] - midCutPosition).magnitude.ToString());
				}
                Debug.Log("numCloseCutPoints: " + numCloseCutPoints.ToString());
                
				if (numCloseCutPoints > 50) {
                    state = STATE_FINAL;

					if (body.chance(0.7F)) {
						if (body.chance(0.5F)) {
							//this.body.events.Add(new EventDrainPuss());
						} else {
							//this.body.events.Add(new EventDrainBlood());
						}
					} else {
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
