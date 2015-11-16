
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

class Fiducial {
    public int id;
    public Vector3 position;
    public float rotation;
    public bool active;
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
    public const int FID_BACTERIALJEL = 4;
    public const int FID_SCALPEL = 5;
    public const int FID_BLOODBAG = 6;
    public const float START_BLOOD_AMOUNT = 5000.0F;
    
    
    
    public Fiducial[] fs;
    public List<Fiducial> updatedFiducials;
    string state; //"notstarted", "ingame" or "gameover"
    List<Event> events;
    
    public int numPlayers;
    public float bloodAmount;
    
    void Start() {
        fs = new Fiducial[NUM_FIDUCIALS];
        updatedFiducials = new List<Fiducial>();
        for (int i = 0; i < NUM_FIDUCIALS; i++) {
            fs[i] = new Fiducial(i);
        }
        events = new List<Event>();
        
        numPlayers = 0;
        bloodAmount = START_BLOOD_AMOUNT;
        
        
        changeStateToGameNotStarted();
    }
    
    void changeStateToGameNotStarted() {
        this.state = "notstarted";
    }
    
    void changeStateToIngame() {
        events.Add(new EventCutOpen(this)); //Add initial event
        this.state = "ingame";
    }
    
    void changeStateToGameOver() {
        this.state = "gameover";
        foreach (Event e in events) {
            e.gameOverSignal();
        }
    }
    
    void Update() {
//        foreach (Fiducial f in fs) {
//            f.update();
//        }
		for (int i = 0; i < NUM_FIDUCIALS; i++) {
			fs [i].update ();
		}
        
        if (this.state == "notstarted") {
            updateGameNotStarted();
        } else if (this.state == "ingame") {
            updateIngame();
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
    
    void updateIngame() {
        if (bloodAmount <= 0.0F) {
            changeStateToGameOver();
        }
        
        //Input.mousePosition: "The bottom-left of the screen or window is at (0, 0).
        //The top-right of the screen or window is at (Screen.width, Screen.height).
        
        tableWidth = 2.0;
        tableHeight = 1.85;
        
        float rx = 2.0F * (Input.mousePosition.x / Screen.width - 0.5F);
        //rx is now -1.0 to 1.0, need to rescale so that max is tableWidth/2 = 2.0/2.0 = 1.0, so no change!
        rx *= tableWidth / 2.0F;
        float ry = 2.0F * (Input.mousePosition.y / Screen.width - 0.5F);
        //ry is now -1.0 to 1.0, need to rescale so that max is tableHeight/2 = 1.85/2.0 = 0.925
        ry *= tableHeight / 2.0F;
        Vector3 mouseWorldPos = Vector3(rx, ry, 0.0F);
        

        //debug stuff, use keys to place things such as scalpels and bacterial jels
        if (Input.GetKeyDown(KeyCode.S)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                fs[FID_SCALPEL].active = false;
                Debug.Log("Scalpel de-activated");
            } else {
                fs[FID_SCALPEL].active = true;
                fs[FID_SCALPEL].position = mouseWorldPos;
                Debug.Log("Scalpel activated");
            }
        }
        if (Input.GetKeyDown(KeyCode.J)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                fs[FID_BACTERIALJEL].active = false;
                Debug.Log("Jel de-activated");
            } else {
                fs[FID_BACTERIALJEL].active = true;
                fs[FID_BACTERIALJEL].position = mouseWorldPos;
                Debug.Log("Jel activated");
            }
        }
        if (Input.GetKeyDown(KeyCode.B)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                fs[FID_BLOODBAG].active = false;
                Debug.Log("Bloodbag de-activated");
            } else {
                fs[FID_BLOODBAG].active = true;
                fs[FID_BLOODBAG].position = mouseWorldPos;
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
        Text bloodValueText;
        bloodValueText = GameObject.Find ("value_blood").GetComponent<Text>();
        //bloodValueText.text = "(STATE: " + this.state + ") Blood:" + bloodAmount.ToString();
		bloodValueText.text = "Blood left: " + bloodAmount.ToString() + " ml";

		GameObject.Find ("value_statedebug").GetComponent<Text>().text = "STATE: " + this.state;
    }
}
