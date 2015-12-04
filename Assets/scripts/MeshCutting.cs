using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshCutting : MonoBehaviour {

	public GameObject skin;
	public GameObject tool;
	private Mesh mesh;
	private List<Vector3> vertices;
	private int[] originalFaces;
	private List<int> triangles;
	private List<Vector3> vertsToAlign;
	private List<Vector3> alignedVerts;
	private float shortestDistance;
	private float previousDistance;
	private Vector3 nearestPoint;

	List<Vector3> linePoints = new List<Vector3>();
	LineRenderer lineRenderer;
	public float startWidth = 1.0f;
	public float endWidth = 1.0f;
	public float threshold = 1.0f;
	int lineCount = 0;
	
	Vector3 lastPos = Vector3.one * float.MaxValue;
	
	
	void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}



	void Start () {
		// Initialize everything
		previousDistance = Mathf.Infinity;
		vertsToAlign = new List<Vector3>();
		alignedVerts = new List<Vector3>();
		vertices = new List<Vector3>();
		triangles = new List<int>();
		mesh = skin.GetComponent<MeshFilter>().mesh;
		originalFaces = mesh.triangles;

		foreach (Vector3 vertice in mesh.vertices) {
			vertices.Add (vertice);
		}

		foreach (int triangle in mesh.triangles) {
			triangles.Add (triangle);
		}
			
	}

	void Update () {

		// Mesh code
		mesh = skin.GetComponent<MeshFilter>().mesh;

		// Events
		if (Input.GetMouseButton (0)) {
			//draw();
			bool useMouse = true;
			//simpleMeshCutting(useMouse);
			simpleMeshCutting();
		}

		if (Input.GetMouseButtonUp (0)) {
			//alignVertices();
			//removeTriangles();

			//updateMesh();
		}

		if (Input.GetMouseButtonDown (0)) {

			//simpleMeshCutting();

		}

		if (Input.GetKeyDown("r")) {
			triangles = new List<int>();
			foreach (int triangle in originalFaces) {
				triangles.Add (triangle);
			}

			updateMesh();
			
		}

		if (Input.GetKeyDown("y")) {
			var testing = Instantiate(tool, transform.position,Quaternion.identity);

			
		}

		if (Input.GetKeyDown("t")) {
			Destroy(tool);
			
		}
		
	}

	void UpdateLine()
	{
		// draw lines on the screen
		lineRenderer.SetWidth(startWidth, endWidth);
		lineRenderer.SetVertexCount(linePoints.Count);
		
		for(int i = lineCount; i < linePoints.Count; i++)
		{
			lineRenderer.SetPosition(i, linePoints[i]);
		}
		lineCount = linePoints.Count;
	}

	void draw()
	{

		// mouse draw event
		Vector3 mouseWorld = tool.transform.position;
		
		float d = Vector3.Distance(lastPos, mouseWorld);
		if(d <= threshold)
			return;
		
		lastPos = mouseWorld;
		if(linePoints == null)
			linePoints = new List<Vector3>();
		linePoints.Add(mouseWorld);
		vertsToAlign.Add(mouseWorld);
		
		UpdateLine();

	}

	void alignVertices()
	{
		
		// Align vertices
		linePoints = new List<Vector3>();
		foreach (Vector3 vert in vertsToAlign) {
			Vector3 nearestPoint = NearestVertexTo(vert);
			alignedVerts.Add(nearestPoint);
			linePoints.Add(nearestPoint);
			UpdateLine();
		}

		vertsToAlign = new List<Vector3>();
		
	}

	void removeTriangles()
	{
		var counter = 0;
		var letsBreak = false;

		for(int i = 0; i < triangles.Count-2; i+=3){

			var vertPos1 = triangles[i];
			var vertPos2 = triangles[i+1];
			var vertPos3 = triangles[i+2];
			// Work in progress here, integrate algorithm for detecting triangles within the drawn figure
			for(int n = 0; n < alignedVerts.Count-2; n+=3){
				if((vertices[vertPos1] == alignedVerts[n] && vertices[vertPos2] == alignedVerts[n+1]) || (vertices[vertPos1] == alignedVerts[n] && vertices[vertPos3] == alignedVerts[n+1]) || (vertices[vertPos2] == alignedVerts[n] && vertices[vertPos3] == alignedVerts[n+1])){
					//counter+=1;
					// Calculate which triangles to remove here
					triangles.RemoveAt(i);
					triangles.RemoveAt(i);
					triangles.RemoveAt(i);
					letsBreak = true;
					break;
					// print (n);
				}
			}
			if(letsBreak)
				break;
		}

	}

	void updateMesh()
	{
		// Todo add mesh.uv here
		mesh.Clear();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
	}

	void simpleMeshCutting()
	{
		// Simple click mech cutting below
		
		var n = 0;
			Vector3 mouseClickPos = tool.transform.position;
			//print ("mouse: " + mouseClickPos.ToString ());
			Vector3 nearestPoint = NearestVertexTo(mouseClickPos);
			Debug.Log("nearestPoint:" + nearestPoint.ToString());
			//print ("nearest: " + nearestPoint.ToString ());
		
		for(int i = 0; i < triangles.Count-2; i+=3){
			var vertPos1 = triangles[i];
			var vertPos2 = triangles[i+1];
			var vertPos3 = triangles[i+2];
			if((vertices[vertPos1] == nearestPoint) || (vertices[vertPos2] == nearestPoint) || (vertices[vertPos3] == nearestPoint))
			{
				Debug.Log("removeAt(i) where i = " + i.ToString());
				triangles.RemoveAt(i);
				triangles.RemoveAt(i);
				triangles.RemoveAt(i);
				break;
			}
		}
		
		updateMesh();
	}

	public Vector3 NearestVertexTo(Vector3 point)
	{
		//Debug.Log ("point: " + point.ToString ());
		//point = point + new Vector3(0.0F, 0.0F, -11.0F);

		float minDistanceSqr = Mathf.Infinity;
		Vector3 nearestVertex = Vector3.zero;

		foreach (Vector3 vertex in vertices)
		{
			Vector3 diff = point - vertex;
			float distSqr = diff.sqrMagnitude;
			if (distSqr < minDistanceSqr)
			{
				minDistanceSqr = distSqr;
				nearestVertex = vertex;
			}
		}

		return nearestVertex;
		
	}
}
