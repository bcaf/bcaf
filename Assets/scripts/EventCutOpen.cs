using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

class EventCutOpen : Event {
    Vector3 position;
    const int STATE_NEEDS_BACTERIAL = 0;
    const int STATE_CUTTING = 1;
    const int STATE_FINAL = 10; //if reached, please remove me!
    public int state;
    
    public float bacGelAmount; //in cl
    const float BACJEL_ADDED_PER_SECOND = 12.0F;
    //const float BACJEL_ADDED_PER_SECOND = 1.0F;
    float bacGelAmountNeeded;

    float scalpelAmount;
    //const float SCALPEL_ADDED_PER_SECOND = 12.0F;
    const float SCALPEL_ADDED_PER_SECOND = 3.0F;
    float scalpelAmountNeeded;

    List<Vector2> cutPoints = new List<Vector2>();
    Vector2 cutStartPosition;
    Vector2 cutEndPosition;

    public float distBetweenScalpelAndEvent;
    
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

    public Vector3 get2dPositionAsVector3() {
        return new Vector3(this.position.x, 0.0F, this.position.z);
    }
    
    public void update() {
        //Debug.Log("In EventCutOpen.update(), state = " + state.ToString());

        if (state == STATE_NEEDS_BACTERIAL) {
            //Fiducial bacGelFid = body.getFiducial(Body.FID_BACTERIALGEL);
            Fiducial bacGelFid = body.getFiducial(Body.FID_BACTERIALGEL);

            if (bacGelFid.active) {
                Debug.Log("bacgeldist: " + (bacGelFid.position - this.get2dPositionAsVector3()).magnitude.ToString());

                if ((bacGelFid.position - this.get2dPositionAsVector3()).magnitude < 20.0F) {
                    bacGelAmount += BACJEL_ADDED_PER_SECOND * Time.deltaTime;
                }
                
                if (bacGelAmount >= bacGelAmountNeeded) {
                    state = STATE_CUTTING;
                    return;
                }
                
                //Debug.Log("Right after drawNeedsBacterial in if(bacJelFid.active())");
            } else {
                //Debug.Log("bacJelFid.active is false");
            }

            drawNeedsBacterial();
        } else if (state == STATE_CUTTING) {
            //Fiducial scalpel = body.getFiducial(Body.FID_SCALPEL);
            Fiducial scalpel = body.getFiducial(Body.FID_SCALPEL);

            if (scalpel.active) {
                //Debug.Log("scalpel.position (v3) = " + scalpel.position.ToString());
                //GameObject x = GameObject.Find("hej"); //lol crash
                //x.ToString();
                //float 

                distBetweenScalpelAndEvent = (scalpel.position - this.get2dPositionAsVector3()).magnitude;
                Debug.Log("scalp pos: " + scalpel.position.ToString() + "this2dpos:"+this.get2dPositionAsVector3().ToString()
                    + "scalpdist:" + distBetweenScalpelAndEvent.ToString());
                //Debug.Log("scalpdist: " + distBetweenScalpelAndEvent.ToString());
                if (distBetweenScalpelAndEvent < 20.0F) {
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
        float ratio = bacGelAmount / bacGelAmountNeeded;

        //Debug.Log("BacGelAmount ratio: " + ratio.ToString());

        if (ratio < 0.01F) {
            GameObject.Find("label_cutopenevent").GetComponent<Text>().text =
                "Apply Bacterial Gel!";
        } else {
            GameObject.Find("label_cutopenevent").GetComponent<Text>().text =
                "Apply Bacterial Gel!\n" + (100.0F*ratio).ToString("0.0") + "%";
        }
    }
    
    void drawIsCutting() {
        //TODO: add line showing area to cut and plot the last 50 or so cutPoints (as lines?)
        float ratio = scalpelAmount / scalpelAmountNeeded;

        if (ratio < 0.01F) {
            GameObject.Find("label_cutopenevent").GetComponent<Text>().text =
                //"Cut open with Scalpel!";
                "Cut open with Scalpel! scalpeldist:" + distBetweenScalpelAndEvent.ToString("0.0");
        } else {
            GameObject.Find("label_cutopenevent").GetComponent<Text>().text =
                "Keep cutting!\n" + (100.0F*ratio).ToString("0.0") + "%"
                + "\nscalpeldist:" + distBetweenScalpelAndEvent.ToString("0.0");
        }
    }
    
    //this is called whenever gameOver happens.  Cleanly stop current animations and things like that.
    public override void gameOverSignal() {
    }
}
