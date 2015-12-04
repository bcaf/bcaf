using UnityEngine;

class Event : MonoBehaviour {
    bool active;
    protected static Body body;
	public Vector3 position;

    public Event() {}
    
    public Event(Body body_) {
        body = body_;
		position = new Vector3();
    }
    
    public virtual void update() {
		Debug.Log("We're in Event.Update()");
        //body.bloodAmount -= 1.0F;
    }
	
	public Vector2 get2dPosition() {
		return new Vector2(position.x, position.z);
    }

    public Vector3 get2dPositionAsVector3()
    {
        return new Vector3(gameObject.transform.position.x, 0.0F, gameObject.transform.position.z);
    }

    public void setBody(Body body_)
    {
        body = body_;
    }
    
    /*this is called whenever gameOver happens.
    Cleanly stop current animations and things like that.*/
    public virtual void gameOverSignal() {
    }
}

/* Copy this template thingy whenever you create a new sub event (that you'll create in a new file)
class EventSubClass : Event {
    public SubEventEvent(Body body_) {
        Event(body_);
    }
    
    void drawState0() {
    }
    
    //Unity's Update function
    void Update() {
        //Fiducial bacJelFid = body.getFiducial(Body.FID_BACTERIALJEL);
    }
    
    //this is called whenever gameOver happens.  Cleanly stop current animations and things like that.
    public override void gameOverSignal() {
    }
}
*/
