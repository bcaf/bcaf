
class Fiducial {
    int id;
    Vector3 position;
    float rotation;
    bool active;
    Fiducial(int id) {
        id = 0;
        position = Vector3();
        rotation = 0.0F;
        active = false;
    }
    
    void update() {
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
    
    
    
    public fs[] fs;
    public List<Fiducial> updatedFiducials;
    string state; //"notstarted", "ingame" or "gameover"
    List<Event> events;
    
    int numPlayers;
    float bloodAmount;
    
    void Setup() {
        fs = new Fiducial[NUM_FIDUCIALS];
        updatedFiducials = new List<Fiducial>();
        for (int i = 0; i < NUM_FIDUCIALS; i++) {
            fs.Add(Fiducial(i));
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
        events.Add(new CutOpenEvent()); //Add initial event
        state = "ingame";
    }
    
    void changeStateToGameOver() {
        state = "gameover";
    }
    
    void Update() {
        foreach (Fiducial f in fs) {
            f.update();
        }
        
        if (state == "notstarted") {
            updateGameNotStarted();
        } else if (state == "ingame") {
            updateInGame();
        } else if (state == "ingame") {
        } else if (state == "gameover") {
        }
    }
    
    
    /** Here we update things that happens before the game starts */
    void updateGameNotStarted() {
        bool fingerPressedStart = false; //change this with an actual call
        if (fingerPressedStart) {
            int numActiveGloves = fs[GLOVE0].active + fs[GLOVE1].active +
                fs[GLOVE2].active + fs[GLOVE3].active;
            numPlayers = NUM_GLOVES - numActiveGloves;
            changeStateToIngame();
        }
    }
    
    void updateIngame() {
        
        if (bloodAmount <= 0.0F) {
        }
    }
    
    void updateGameOver() {
    }
    
    //TODO: drawgui
    void drawGUI() {
        //Draws the gui, that shows things like bloodAmount;
    }
}
