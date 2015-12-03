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

    public Dictionary<int, Fiducial> fd;

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
        catch (SocketException e)
        {
            Debug.Log("SocketException!");
        }
        finally { }
	
	}
	
	// Update is called once per frame
	void Update () {
        if (fd.ContainsKey(2))
            Play();
	}

    public void Play()
    {
        Application.LoadLevel("mainscene");
    }
}
