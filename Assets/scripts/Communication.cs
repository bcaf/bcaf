using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net.Sockets;

public class Fiducial {
    public int id;
    public Vector3 position;
    public float rotation;
    public bool active;

	public Vector2 get2dPosition() {
		return new Vector2(position.x, position.z);
    }

    public Fiducial(int id_, Vector3 position_, float rotation_, bool active_) {
        id = id_;
        position = position_;
        rotation = rotation_;
        active = active_;
    }
    
    public void update() {
        //update position, rotation, active of id=id
    }
}

public class Communication : MonoBehaviour {
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
    public GameObject[] mirrors;
    
    public Dictionary<int, Fiducial> fd;

    private float Xo = -20f;
    private float Yo = 4.5f;
    private float Xdist = 19f + 20f;
    private float Ydist = 4.5f - (-17.5f);

    private int objects = 1;
    /*public void addObject(GameObject g){
        mirrors [objects] = g;
        objects++;
    }*/
    /*public void addGlass(GameObject g){
        mirrors [0] = g;
    }*/

    // Use this for initialization
    void Start () {
        mirrors = new GameObject[10];
        //fs = new Fiducial[NUM_FIDUCIALS];
        fd = new Dictionary<int, Fiducial>();

        try{
        clientNew = new TcpClient("127.0.0.1",1111);
        clientUpdate = new TcpClient("127.0.0.1",1112);
        clientDelete = new TcpClient("127.0.0.1",1113);

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

        }finally{
            //client.Close();
        }
    }

    // Update is called once per frame by Unity
    void Update () {
        if (clientNew.Available > 0) {
            int ID = Convert.ToInt32 (srNew.ReadLine ());
            int Tag = Convert.ToInt32 (srNew.ReadLine ());
            float X = Convert.ToSingle (srNew.ReadLine ());
            float Y = Convert.ToSingle (srNew.ReadLine ());
            int Rad = Convert.ToInt32(srNew.ReadLine());
            float Angle = ((float)Rad*3.1415f/180.0f)/100.0f;
            
            Vector3 position = new Vector3(X, 0.0F, Y);
            fd.Add(ID, new Fiducial(ID, position,Rad, true));
            
            /*X = Xo + (X / 1920f) * Xdist;
            Y = Yo - (Y / 1080f) * Ydist;
            Vector3 tmp = new Vector3(X,1.1f,Y);
            Vector3 tmpAng = new Vector3(0,Angle+90,0);
            if(Tag<objects){
                mirrors[Tag].transform.position = tmp;
                mirrors[Tag].transform.eulerAngles = tmpAng;
            }
            else{
                mirrors[0].transform.position = tmp;
                mirrors[0].transform.eulerAngles = tmpAng;
            }
            swNew.WriteLine("done1");
            */
        }
        if (clientUpdate.Available > 0) {
            int ID = Convert.ToInt32(srUpdate.ReadLine());
            int Tag = Convert.ToInt32(srUpdate.ReadLine());
            float X = Convert.ToSingle(srUpdate.ReadLine());
            float Y = Convert.ToSingle(srUpdate.ReadLine());
            float Rad = Convert.ToSingle(srUpdate.ReadLine())/100f;
            float Angle = (Rad*180f/3.1415f);
            
            Vector3 position = new Vector3(X, 0.0F, Y);
            fd.Add(ID, new Fiducial(ID, position,Rad, true));
            
            /*X = Xo+(X/1920f)*Xdist;
            Y = Yo-(Y/1080f)*Ydist;
            Vector3 tmp = new Vector3(X,1.1f,Y);
            Vector3 tmpAng = new Vector3(0,Angle+90,0);
            if(Tag<objects){
                mirrors[Tag].transform.position = tmp;
                mirrors[Tag].transform.eulerAngles = tmpAng;
            }
            else{
                mirrors[0].transform.position = tmp;
                mirrors[0].transform.eulerAngles = tmpAng;
            }
            swUpdate.WriteLine("done2");*/
        }
        if (clientDelete.Available > 0) {
            int ID = Convert.ToInt32(srDelete.ReadLine());
            if (fd.ContainsKey(ID)) {
                fd[ID].active = false;
            } else {
                Debug.Log("Tried to delete fiducial " + ID
                    + " but it's not in fd!");
            }
                
            
            /*Vector3 position = new Vector3(X, 0.0F, Y);
            
            int Tag = Convert.ToInt32(srDelete.ReadLine());
            Debug.Log(Tag);
            Vector3 tmp = new Vector3(0,-10f,0);
            if(Tag<objects){
                mirrors[Tag].transform.position = tmp;
            }
            else{
                mirrors[0].transform.position = tmp;
            }
            */
        }
    }
}
