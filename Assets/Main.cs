using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    struct flow
    {
        public Vector3 direction;
        public Vector3 force;
        public float density;
    }

    // [0][0][0] is a 1*1*1 cube with one corner in origin.
    flow[,,] grid;
    int flowXDim = 10;
    int flowYDim = 10;
    int flowZDim = 10;

    void SetupVolGrid()
    {
        grid = new flow[flowXDim,flowYDim,flowZDim];
        for (int x = 0; x < flowXDim; x++)
        {
            for (int y = 0; y < flowYDim; y++)
            {
                for (int z = 0; z < flowZDim; z++)
                {
                    grid[x, y, z].direction = new Vector3(0, 0, 0);
                    grid[x, y, z].force = new Vector3(0, 0, 0);
                    grid[x, y, z].density = 0.0f;
                }
            }
        }
    }

	// Use this for initialization
	void Start () {
        	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
