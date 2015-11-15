
using UnityEngine;
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
    
    private const int GLOVE0 = 0;
    private const int GLOVE1 = 1;
    private const int GLOVE2 = 2;
    private const int GLOVE3 = 3;
    private const int BACTERIALJEL = 4;
    private const float START_BLOOD_AMOUNT = 5000.0F;
    
    
    
    public Fiducial[] fs;
    public List<Fiducial> updatedFiducials;
    string state; //"notstarted", "ingame" or "gameover"
    List<Event> events;
    
    int numPlayers;
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
        state = "notstarted";
    }
    
    void changeStateToIngame() {
        events.Add(new CutOpenEvent(this)); //Add initial event
        state = "ingame";
    }
    
    void changeStateToGameOver() {
        state = "gameover";
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
        
        if (state == "notstarted") {
            updateGameNotStarted();
        } else if (state == "ingame") {
            updateIngame();
        } else if (state == "ingame") {
        } else if (state == "gameover") {
        }
    }
    
    
    /** Here we update things that happens before the game starts */
    void updateGameNotStarted() {
        bool fingerPressedStart = false; //change this with an actual call
        if (fingerPressedStart) {
//            int numActiveGloves = fs[GLOVE0].active + fs[GLOVE1].active +
//                fs[GLOVE2].active + fs[GLOVE3].active;
			int numActiveGloves = 0;
			numActiveGloves += fs[GLOVE0].active ? 1 : 0;
			numActiveGloves += fs[GLOVE1].active ? 1 : 0;
			numActiveGloves += fs[GLOVE2].active ? 1 : 0;
			numActiveGloves += fs[GLOVE3].active ? 1 : 0;
            numPlayers = NUM_GLOVES - numActiveGloves;
            changeStateToIngame();
        }
    }
    
    void updateIngame() {   
        if (bloodAmount <= 0.0F) {
            changeStateToGameOver();
        }
    }
    
    void updateGameOver() {
    }
    
    //TODO: drawgui
    void drawGUI() {
        //Draws the gui, that shows things like bloodAmount;
    }
}
