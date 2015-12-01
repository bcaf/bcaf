using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    public class flow
    {
        public Vector3 direction;
        public Vector3 force;
        public float density;
    }

    public class cell
    {
        public Vector3 position = new Vector3();
        public float radius; // Always a box
        public flow flow = new flow();
        public myParticle[] particles;
        public cell cellup = null; // y-pos
        public cell celldown = null; // y-neg
        public cell cellleft = null; // z-pos
        public cell cellright = null; // z-neg
        public cell cellfront = null; // x-pos
        public cell cellback = null; // x-neg

        public float GetSize()
        {
            return radius * 2;
        }

        public cell(float radius) {
            this.radius = radius;
        }
    }

    public class myParticle
    {
        public ParticleSystem.Particle particle;
        public float density;
        public float pressure;
        public Vector3 position;
    }

    // [0][0][0] is a 1*1*1 cube with one corner in origin.
    cell[,,] grid;
    public int gridXDim = 10;
    public int gridYDim = 10;
    public int gridZDim = 10;
    public float boxSize = 1.0f;
    void SetupVolGrid()
    {
        grid = new cell[gridXDim,gridYDim,gridZDim];
        Vector3 origin = transform.position;
        for (int x = 0; x < gridXDim; x++)
        {
            for (int y = 0; y < gridYDim; y++)
            {
                for (int z = 0; z < gridZDim; z++)
                {
                    cell c = new cell(boxSize);
                    // Adjust x- and z-axis to center the floor
                    float newX = (origin.x + c.radius) + (x * c.radius * 2) - (c.radius * 2 * gridXDim/2);
                    float newZ = (origin.z + c.radius) + (z * c.radius * 2) - (c.radius * 2 * gridZDim/2);
                    float newY = (origin.y + c.radius) + (y * c.radius * 2);
                    c.position = new Vector3(newX, newY, newZ);

                    // Neighbour check
                    if (y < (gridYDim - 1))
                        c.cellup = grid[x, y+1, z];
                    if (y > 0)
                        c.celldown = grid[x, y-1, z];
                    if (z < (gridZDim - 1))
                        c.cellup = grid[x, y, z+1];
                    if (z > 0)
                        c.celldown = grid[x, y, z-1];
                    if (x < (gridXDim - 1))
                        c.cellup = grid[x+1, y, z];
                    if (x > 0)
                        c.celldown = grid[x-1, y, z];

                    c.flow.direction = new Vector3(0, 0, 0);
                    c.flow.force = new Vector3(0, 0, 0);
                    c.flow.density = 0.0f;

                    grid[x, y, z] = c;
                }
            }
        }
    }

    public ParticleSystem ps;
    public ParticleSystem.Particle[] particles;

    void SetupParticleSim()
    {
        ps = GameObject.Find("Particle System").GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[ps.maxParticles];
    }

    cell GetGridPos(ParticleSystem.Particle particle)
    {
        float x = particle.position.x;
        float y = particle.position.y;
        float z = particle.position.z;

        // Block 0 reaches from -GridXDim/2 to -GridXDim/2 + 1*size
        foreach(cell c in grid)
        {
            if (x > c.position.x && x < c.position.x + c.GetSize())
            {
                if (y > c.position.y && y < c.position.y + c.GetSize())
                {
                    if (z > c.position.z && z < c.position.x + c.GetSize())
                    {
                        return c;
                    }
                }
            }
        }

        return null;
    } 

    void FindNeighbors()
    {
        ps.GetParticles(particles);
        for (int i = 0; i < ps.maxParticles; i++ )
        {
            // Get the grid cell in which the particle is residing.
            cell c = GetGridPos(particles[i]);
            if (c == null)
            {
                Debug.Log("Got null position on particle");
                continue;
            }
            else
            {
                for (int j = 0; j < ps.maxParticles; j++)
                {
                    if (particles[j].Equals(particles[i]))
                        continue;


                    Vector3 heading = particles[j].position - particles[i].position;
                    float dist = heading.sqrMagnitude;
                    float range = 10.0f; // Magic number. Change later
                    if (dist < (range * range))
                    {
                        particles[i].velocity += heading / (dist * 2);
                    }
                }
            }            
        }

        ps.SetParticles(particles, particles.Length);
    }

    void CalcVelocity()
    {

    }

	// Use this for initialization
	void Start () {
        SetupVolGrid();
        //SetupParticleSim();
	}
	
	// Update is called once per frame
	void Update () {

        //if (Input.GetMouseButtonDown(1))
            //ps.Emit(100);
        //FindNeighbors();
	}

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.2f);            
        for (int x = 0; x < gridXDim; x++)
        {
            for (int y = 0; y < gridYDim; y++)
            {
                for (int z = 0; z < gridZDim; z++)
                {
                    cell c = grid[x, y, z];
                    if (c != null)
                        Gizmos.DrawWireCube(c.position, new Vector3(1, 1, 1) * c.GetSize());
                    else
                        Debug.Log("No cell at (" + x + "," + y + "," + z + ")");
                }
            }
        }
        
    }
}
