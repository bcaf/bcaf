
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net.Sockets;

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
    
    public float numAliveSeconds = 0.0F;

    public GameObject BleedEventPrefab;
    public GameObject GelEventPrefab;
    public GameObject PussEventPrefab;
    public GameObject CutEventPrefab;
    
    public Fiducial[] fs;
    string state; //"notstarted", "ingame" or "gameover"
    public List<Event> events;
    
    public int numPlayers;
    public volatile float bloodAmount;

    /*
    public StreamReader srNew;
    public StreamWriter swNew;
    public Stream sNew;
    public StreamReader srUpdate;
    public StreamWriter swUpdate;
    public Stream sUpdate;
    public StreamReader srDelete;
    public StreamWriter swDelete;
    public Stream sDelete;
    public TcpClient clientNew;
    public TcpClient clientUpdate;
    public TcpClient clientDelete;
     * */

    public Dictionary<int, Fiducial> fd;
    
    void Start() {

        events = new List<Event>();
        
        numPlayers = 0;
        bloodAmount = START_BLOOD_AMOUNT;
        
        
        changeStateToGameNotStarted();

        fd = globalsettings.Instance.fd;
        /*
        fd = new Dictionary<int, Fiducial>();

        try
        {
            clientNew = new TcpClient("127.0.0.1", 1111);
            clientUpdate = new TcpClient("127.0.0.1", 1112);
            clientDelete = new TcpClient("127.0.0.1", 1113);

            sNew = clientNew.GetStream();
            srNew = new StreamReader(sNew);
            swNew = new StreamWriter(sNew);
            swNew.AutoFlush = true; // enable automatic flushing
            sUpdate = clientUpdate.GetStream();
            srUpdate = new StreamReader(sUpdate);
            swUpdate = new StreamWriter(sUpdate);
            swUpdate.AutoFlush = true; // enable automatic flushing
            sDelete = clientDelete.GetStream();
            srDelete = new StreamReader(sDelete);
            swDelete = new StreamWriter(sDelete);
            swDelete.AutoFlush = true; // enable automatic flushing

        }
        catch (SocketException e) {
            Debug.Log("SocketException!");
        }
        finally { }
         * */
    }

    public void decreaseBloodAmount(float amount)
    {
        bloodAmount -= amount;
    }
    
    void changeStateToGameNotStarted() {
        this.state = "notstarted";
    }
    
    void changeStateToIngame() {
        //events.Add(new EventCutOpen(this, new Vector3(-75.0F, 58.0F, -38.0F))); //Add initial event
		this.state = "ingame";
		Debug.Log ("Game started!");
		StartCoroutine (RandomizeEvent ());

        createBleedEvent();
    }
    
    void changeStateToGameOver() {
        this.state = "gameover";
        foreach (Event e in events) {
            e.gameOverSignal();
        }
        Debug.Log("I am dead");
        
        //Application.LoadLevel("mainmenu");
    }
    
    void Update() {
        if (this.state == "notstarted") { updateGameNotStarted();
        } else if (this.state == "ingame") { updateIngame();
        } else if (this.state == "gameover") {
        }
    }
    
    /** Here we update things that happens before the game starts */
    void updateGameNotStarted() {
        //bool gameStarted = false;
        bool gameStarted = true;

        //debug: press space to start
        if (Input.GetKeyDown(KeyCode.Space)) {
            gameStarted = true;
        }

        if (gameStarted) {
//            int numActiveGloves = getFiducial(GLOVE0).active + getFiducial(GLOVE1).active +
//                getFiducial(GLOVE2).active + getFiducial(GLOVE3).active;
            int numActiveGloves = 0;
            numActiveGloves += getFiducial(FID_GLOVE0).active ? 1 : 0;
            numActiveGloves += getFiducial(FID_GLOVE1).active ? 1 : 0;
            numActiveGloves += getFiducial(FID_GLOVE2).active ? 1 : 0;
            numActiveGloves += getFiducial(FID_GLOVE3).active ? 1 : 0;
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

    Vector3 getFidToWorldPosition(Vector3 fidPosition)
    {
        float tableWidth = 225.0F;
        float tableHeight = 400.0F;

        float rx = 2.0F * (fidPosition.x / Screen.width - 0.5F);
        //rx is now -1.0 to 1.0, need to rescale so that max is tableWidth/2 = 2.0/2.0 = 1.0, so no change!
        rx *= tableWidth / 2.0F;
        float ry = 2.0F * (fidPosition.z / Screen.height - 0.5F);
        //ry is now -1.0 to 1.0, need to rescale so that max is tableHeight/2 = 1.85/2.0 = 0.925
        ry *= tableHeight / 2.0F;
        return new Vector3(rx, 0.0F, ry); //0 is local pos y for Canvas
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
        
        GameObject.Find("alivetimer").GetComponent<Text>().text =
            "Alive for\n" + Time.realtimeSinceStartup.ToString("0.0") + " seconds";
        
        Vector3 mouseWorldPos = getMouseToWorldPosition();
        //Debug.Log("mouseWorldPos: " + mouseWorldPos.ToString());
        
        if (bloodAmount <= 0.0F) {
            numAliveSeconds = Time.realtimeSinceStartup;
            changeStateToGameOver();
        }

        //debug stuff, press keys to add things such as scalpels and bacterial gels to the position of the mouse.
        if (Input.GetKeyDown(KeyCode.S)) {
            Fiducial scalpel = getFiducial(FID_SCALPEL);
            if (Input.GetKey(KeyCode.LeftShift)) {                
                scalpel.active = false;
                updateDebugObject("scalpel_debug", mouseWorldPos, false);
                scalpel.position = mouseWorldPos;
                Debug.Log("Scalpel de-activated");
            } else {
                scalpel.active = true;
                scalpel.position = mouseWorldPos;
                //scalpel.position = mouseWorldPos;
                Debug.Log("Scalpel activated");
                updateDebugObject("scalpel_debug", mouseWorldPos, true);
            }
        }
        if (Input.GetKeyDown(KeyCode.G)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                getFiducial(FID_BACTERIALGEL).active = false;
                getFiducial(FID_BACTERIALGEL).position = mouseWorldPos;
                updateDebugObject("bacterialgel_debug", mouseWorldPos, false);
                Debug.Log("Gel de-activated");
            } else {
                getFiducial(FID_BACTERIALGEL).active = true;
                getFiducial(FID_BACTERIALGEL).position = mouseWorldPos;
                updateDebugObject("bacterialgel_debug", mouseWorldPos, true);
                Debug.Log("Gel activated");
            }
        }
        if (Input.GetKeyDown(KeyCode.B)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                getFiducial(FID_BLOODBAG).active = false;
                Debug.Log("Bloodbag de-activated");
            } else {
                getFiducial(FID_BLOODBAG).active = true;
                Debug.Log("Bloodbag activated");
            }
        }
    }
    
    void updateGameOver() {
        GameObject.Find("alivetimer").GetComponent<Text>().text =
            "DEAD!\nSurvived\n" + numAliveSeconds.ToString("0.0") + " seconds";
    }
    
    
    //TODO: drawgui
    void drawGUI() {
        //Draws the gui, that shows things like bloodAmount;
    }
    
    void OnGUI() {
        GameObject.Find("label_blood").GetComponent<Text>().text =
            "Blood left:\n" + this.bloodAmount.ToString("0.0") + " ml";
        GameObject.Find("blood_bar").GetComponent<Image>().fillAmount = this.bloodAmount / START_BLOOD_AMOUNT;

        string label_cutopenevent_text = "(cutopenevent\nnot active)";

        /*foreach (Event e in this.events) {
            if (e.GetType() == typeof(EventCutOpen)) {
                if (((EventCutOpen)e).bacGelAmount == 0.0F) {
                    label_cutopenevent_text = "(cutopenevent\nACTIVE!)";
                    //Debug.Log("Changed label_cutopenevent text");
                }
            }
        }
        */

        GameObject.Find("label_cutopenevent").GetComponent<Text>().text = label_cutopenevent_text;

        GameObject.Find ("label_statedebug").GetComponent<Text>().text = "STATE: " + this.state;
    }

    public bool chance(float value) { //helper function
        return UnityEngine.Random.Range(0.0F, 1.0F) < value;
    }

    public Fiducial getFiducial(int ID_)
    {
        // Check if fiducal is in dictionary
        if (fd.ContainsKey(ID_))
        {
            return fd[ID_];
        }
        // Else return a dummy object
        else
        {
            Fiducial dummy = new Fiducial(ID_);
            fd.Add(ID_, dummy);
            return dummy;
        }
    }

    // Canvas X: (-50,50) Y: (70,-25)

	public void createBleedEvent () {
		Debug.Log ("Drain some blood!");
		GameObject eventBleed = Instantiate(BleedEventPrefab);
		//eventBleed.transform.parent = GameObject.Find("Canvas").transform;
		eventBleed.transform.SetParent(GameObject.Find("Canvas").transform, false);
		eventBleed.GetComponent<EventDrainBlood>().setBody(this);
        int x = UnityEngine.Random.Range(-50, 50);
        int y = UnityEngine.Random.Range(-25, 70);
        eventBleed.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
	}

	public void createGelEvent () {
		// Todo: Create gel event here
	}

	public void createCutEvent () {
		Debug.Log ("Cut and remove infected organ!");
		//GameObject eventGel = Instantiate (GelEventPrefab);
		GameObject eventCut = Instantiate (CutEventPrefab);
		eventCut.transform.SetParent(GameObject.Find("Canvas").transform, false);
		eventCut.GetComponent<EventCutOpen>().setBody(this);

	}

	public void createPussEvent () {
		Debug.Log ("Drain puss!");
		GameObject eventPuss = Instantiate (PussEventPrefab);
		eventPuss.transform.SetParent(GameObject.Find("Canvas").transform, false);
		eventPuss.GetComponent<EventDrainPuss>().setBody(this);
        int x = UnityEngine.Random.Range(-50, 50);
        int y = UnityEngine.Random.Range(-25, 70);
        eventPuss.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
	}

	public IEnumerator RandomizeEvent() {
        //yield return new WaitForSeconds(UnityEngine.Random.Range(5.0f, 10.0f));
        if (Time.realtimeSinceStartup < 20.0F) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(1.5f, 3.5f));
        } else if (Time.realtimeSinceStartup < 35.0F) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(1.5f, 3.4f));
        } else if (Time.realtimeSinceStartup < 50.0F) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(1.0f, 2.8f));
        } else {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.3f, 2.4f));
        }
        
		var randomInt = UnityEngine.Random.Range (0, 3);
        Debug.Log("New event");

		// Start a random event
		if (randomInt == 0) {
			createBleedEvent();
		}

		if (randomInt == 1) {
			//createCutEvent();
			createBleedEvent();
		}

		if (randomInt == 2) {
			createPussEvent();
		}

		StartCoroutine(RandomizeEvent());
	}
}
