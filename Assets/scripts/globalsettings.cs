using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net.Sockets;

public class Fiducial
{
    public int id;
    public Vector3 position;
    public float rotation;
    public bool active;

    public Vector2 get2dPosition()
    {
        return new Vector2(position.x, position.z);
    }

    public Fiducial(int id_)
    {
        id = id_;
        position = new Vector3();
        rotation = 0.0F;
        active = false;
    }

    public Fiducial(int id_, Vector3 position_, float rotation_, bool active_)
    {
        id = id_;
        position = position_;
        rotation = rotation_;
        active = active_;
    }

    public void setPosition(Vector3 position_)
    {
        position = position_;
    }

    public void setRotation(float rotation_)
    {
        rotation = rotation_;
    }

    public void setActive()
    {
        active = true;
    }

    public void setInactive()
    {
        active = false;
    }

    public void update()
    {
        //update position, rotation, active of id=id
    }
}

public class globalsettings : MonoBehaviour {

    public static globalsettings Instance;

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

    public Dictionary<int, Fiducial> fd = new Dictionary<int, Fiducial>();

    bool isPlaying;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {

        isPlaying = false;

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
        catch (SocketException e)
        {
            Debug.Log("SocketException!");
        }
        finally { }
	
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
        ry *= -tableHeight / 2.0F;
        
        ry += 35.0F;
        return new Vector3(rx, 0.0F, ry); //0 is local pos y for Canvas
    }
	
	// Update is called once per frame
	void Update () {

        if (EditorApplication.currentScene.Equals("Assets/scenes/mainmenu.unity") && !isPlaying)
        {
            if (fd.ContainsKey(2))
            {
                Play();
            }
        }

        // Comm paste
        if (clientNew != null && clientNew.Available > 0)
        {
            int ID = Convert.ToInt32(srNew.ReadLine()); // ID is not bound to the fiducal.
            int Tag = Convert.ToInt32(srNew.ReadLine()); // Tag is a 'long' bound to the fiducal
            float X = Convert.ToSingle(srNew.ReadLine());
            float Y = Convert.ToSingle(srNew.ReadLine());
            int Rad = Convert.ToInt32(srNew.ReadLine());
            float Angle = ((float)Rad * 3.1415f / 180.0f) / 100.0f;

            Vector3 position = getFidToWorldPosition(new Vector3(X, 0.0F, Y));
            if (!fd.ContainsKey(Tag))
                fd.Add(Tag, new Fiducial(Tag, position, Angle, true));
            else
            {
                fd[Tag].setPosition(position);
                fd[Tag].setActive();
                fd[Tag].setRotation(Angle);
            }
            Debug.Log("Added fiducal " + Tag + " at (" + X + "," + Y + "), UnityPos: ("
                + position.x + ", " + position.y + ", " + position.z + ")");
        }

        if (clientUpdate != null && clientUpdate.Available > 0)
        {
            int ID = Convert.ToInt32(srUpdate.ReadLine());
            int Tag = Convert.ToInt32(srUpdate.ReadLine());
            float X = Convert.ToSingle(srUpdate.ReadLine());
            float Y = Convert.ToSingle(srUpdate.ReadLine());
            float Rad = Convert.ToSingle(srUpdate.ReadLine()) / 100f;
            float Angle = (Rad * 180f / 3.1415f);

            Vector3 position = getFidToWorldPosition(new Vector3(X, 0.0F, Y));

            //fd.Add(Tag, new Fiducial(Tag, position,Rad, true));
            if (!fd.ContainsKey(Tag))
                fd.Add(Tag, new Fiducial(Tag, position, Angle, true));
            else
            {
                fd[Tag].setPosition(position);
                fd[Tag].setActive();
                fd[Tag].setRotation(Angle);
            }
            
            if (Tag == 7) {
                //GetComponent
                //GameObject.Find("skin").GetComponent<MeshCutting>().simpleMeshCuttingManualFiducial(position);
;
                //GameObject.Find("fiducial_debug").GetComponent<Transform>().position = position;
                //Debug.Log("Updated scalpel fiducial_debug, position: " + position.ToString());
            } else {
                //Debug.Log("Tag = " + Tag.ToString());
            }


            //Debug.Log("Updating fiducal " + Tag + " at (" + X + "," + Y + ")");
        }
        if (clientDelete != null && clientDelete.Available > 0)
        {
            int Tag = Convert.ToInt32(srDelete.ReadLine());
            if (fd.ContainsKey(Tag))
            {
                fd[Tag].setInactive();
            }
            else
            {
                Debug.Log("Tried to delete fiducial " + Tag
                    + " but it's not in fd!");
            }

            Debug.Log("Removed fiducal " + Tag);
        }
	}

    public void Play()
    {
        isPlaying = true;
        Application.LoadLevel("mainscene");
    }
}
