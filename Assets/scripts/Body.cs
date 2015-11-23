
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

class Fiducial {
    public int id;
    public Vector3 position;
    public float rotation;
    public bool active;

	public Vector2 get2dPosition() {
		return new Vector2(position.x, position.z);
    }

    public Fiducial(int id_) {
        id = id_;
        position = new Vector3();
        rotation = 0.0F;
        active = false;
    }
    
    public void update() {
        //update position, rotation, active of id=id
    }
}

class Body : MonoBehaviour {
    private const int NUM_FIDUCIALS = 50;
    private const int NUM_GLOVES = 4;
    
    public const int FID_GLOVE0 = 0;
    public const int FID_GLOVE1 = 1;
    public const int FID_GLOVE2 = 2;
    public const int FID_GLOVE3 = 3;
    public const int FID_BACTERIALGEL = 4;
    public const int FID_SCALPEL = 5;
    public const int FID_BLOODBAG = 6;
    public const int FID_SYRINGE = 7;
    public const float START_BLOOD_AMOUNT = 5000.0F;
    
    public Fiducial[] fs;
    string state; //"notstarted", "ingame" or "gameover"
    public List<Event> events;
    
    public int numPlayers;
    public float bloodAmount;
    
    void Start() {
        //fs = new Fiducial[NUM_FIDUCIALS];
        /*for (int i = 0; i < NUM_FIDUCIALS; i++) {
            fs[i] = new Fiducial(i);
        }
        */
        
        events = new List<Event>();
        
        numPlayers = 0;
        bloodAmount = START_BLOOD_AMOUNT;
        
        
        changeStateToGameNotStarted();
    }
    
    void changeStateToGameNotStarted() {
        this.state = "notstarted";
    }
    
    void changeStateToIngame() {
        events.Add(new EventCutOpen(this, new Vector3(-75.0F, 58.0F, -38.0F))); //Add initial event
        this.state = "ingame";
    }
    
    void changeStateToGameOver() {
        this.state = "gameover";
        foreach (Event e in events) {
            e.gameOverSignal();
        }
    }
    
    void Update() {
        Communication comm = victim.GetComponent<Communication>();
        fids = comm.updateFiducials();
		//for (int i = 0; i < NUM_FIDUCIALS; i++) { fs[i].update(); }
		//foreach (Fiducial f in fs)       { f.update(); }
		foreach (Event e in this.events) {
			//Debug.Log("event type: " + e.GetType().ToString());

			//TODO: there has to be a better way of doing this...
			if (e is EventCutOpen) {
				((EventCutOpen)e).update();
			}
		}
        
        if (this.state == "notstarted") { updateGameNotStarted();
        } else if (this.state == "ingame") { updateIngame();
        } else if (this.state == "ingame") {
        } else if (this.state == "gameover") {
        }
    }
    
    
    /** Here we update things that happens before the game starts */
    void updateGameNotStarted() {
        bool fingerPressedStart = false; //change this with an actual call

		//debug: press space to start
		if (Input.GetKeyDown(KeyCode.Space)) {
			fingerPressedStart = true;
		}

        if (fingerPressedStart) {
//            int numActiveGloves = fs[GLOVE0].active + fs[GLOVE1].active +
//                fs[GLOVE2].active + fs[GLOVE3].active;
			int numActiveGloves = 0;
			numActiveGloves += fs[FID_GLOVE0].active ? 1 : 0;
            numActiveGloves += fs[FID_GLOVE1].active ? 1 : 0;
            numActiveGloves += fs[FID_GLOVE2].active ? 1 : 0;
            numActiveGloves += fs[FID_GLOVE3].active ? 1 : 0;
            numPlayers = NUM_GLOVES - numActiveGloves;
            changeStateToIngame();
        }
    }

	Vector3 getMouseToWorldPosition() {
		//Input.mousePosition: "The bottom-left of the screen or window is at (0, 0).
		//The top-right of the screen or window is at (Screen.width, Screen.height).
        
		float tableWidth = 225.0F;
		float tableHeight = 400.0F;
        
		float rx = 2.0F * (Input.mousePosition.x / Screen.width - 0.5F);
		//rx is now -1.0 to 1.0, need to rescale so that max is tableWidth/2 = 2.0/2.0 = 1.0, so no change!
		rx *= tableWidth / 2.0F;
		float ry = 2.0F * (Input.mousePosition.y / Screen.height - 0.5F);
		//ry is now -1.0 to 1.0, need to rescale so that max is tableHeight/2 = 1.85/2.0 = 0.925
		ry *= tableHeight / 2.0F;
		return new Vector3(rx, 15.0F, ry);
	}

	/*Updates the GameObject called objectName, moving it to newPosition and
	* enabling the mesh renderer (+the children's mesh renderers) if setOn*/
	void updateDebugObject(string objectName, Vector3 newPosition, bool setOn) {
		GameObject debug_object = GameObject.Find(objectName);
		debug_object.GetComponent<Renderer>().enabled = setOn;
		debug_object.GetComponent<Transform>().position = newPosition;

		//Don't forget to enable/disable the rendering of the object's children
		foreach (Renderer r in debug_object.GetComponentsInChildren<Renderer>()) {
			r.enabled = setOn;
		}
	}
    
    void updateIngame() {
		Vector3 mouseWorldPos = getMouseToWorldPosition();
		//Debug.Log("mouseWorldPos: " + mouseWorldPos.ToString());


        
        if (bloodAmount <= 0.0F) {
            changeStateToGameOver();
        }

		//debug stuff, press keys to add things such as scalpels and bacterial gels to the position of the mouse.
		if (Input.GetKeyDown(KeyCode.S)) {
			if (Input.GetKey(KeyCode.LeftShift)) {
				fs[FID_SCALPEL].active = false;
				updateDebugObject("scalpel_debug", mouseWorldPos, false);
				Debug.Log("Scalpel de-activated");
			} else {
				fs[FID_SCALPEL].active = true;
				fs[FID_SCALPEL].position = mouseWorldPos;
				Debug.Log("Scalpel activated");
				updateDebugObject("scalpel_debug", mouseWorldPos, true);
			}
		}
		if (Input.GetKeyDown(KeyCode.G)) {
			if (Input.GetKey(KeyCode.LeftShift)) {
				fs[FID_BACTERIALGEL].active = false;
				updateDebugObject("bacterialgel_debug", mouseWorldPos, false);
				Debug.Log("Gel de-activated");
			} else {
				fs[FID_BACTERIALGEL].active = true;
				Debug.Log("Gel activated");
				updateDebugObject("bacterialgel_debug", mouseWorldPos, true);
			}
		}
		if (Input.GetKeyDown(KeyCode.B)) {
			if (Input.GetKey(KeyCode.LeftShift)) {
				fs[FID_BLOODBAG].active = false;
				Debug.Log("Bloodbag de-activated");
			} else {
				fs[FID_BLOODBAG].active = true;
				Debug.Log("Bloodbag activated");
			}
		}

    }
    
    void updateGameOver() {
    }
    
    //TODO: drawgui
    void drawGUI() {
        //Draws the gui, that shows things like bloodAmount;
    }
    
    void OnGUI() {
		GameObject.Find("label_blood").GetComponent<Text>().text = 
			"Blood left:\n" + this.bloodAmount + " ml";

		string label_cutopenevent_text = "(cutopenevent\nnot active)";

		foreach (Event e in this.events) {
			if (e.GetType() == typeof(EventCutOpen)) {
				label_cutopenevent_text = "(cutopenevent\nACTIVE!)";
			}
		}

		GameObject.Find("label_cutopenevent").GetComponent<Text>().text = label_cutopenevent_text;

		GameObject.Find ("label_statedebug").GetComponent<Text>().text = "STATE: " + this.state;
    }

	public bool chance(float value) { //helper function
		return Random.Range(0.0F, 1.0F) < value;
	}
}
