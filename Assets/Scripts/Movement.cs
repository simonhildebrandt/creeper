using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	public Vector3 host;
	public float scale = 0.5f;
	public float maxSize = 0.1f;

	LayerMask terrain;
	int blobs;
	public SphereToucher toucher;

	public GameObject target;
	public GameObject blobPrefab;
	// Vector3 point;

	void OnDrawGizmos() {
		// Gizmos.color = Color.yellow;
		// Gizmos.DrawSphere(p1, .1f);
		// Gizmos.DrawSphere(p2, .1f);
		// Gizmos.DrawSphere(p3, .1f);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(host, scale);

		// Gizmos.color = Color.red;
		// Gizmos.DrawSphere(point, .1f);
	}

	// Start is called before the first frame update
	void Start()
	{
		terrain = LayerMask.GetMask("Terrain");
		blobs = LayerMask.GetMask("Blobs");
		host = new Vector3(0, 0.5f, 0);
		// toucher = sphere.GetComponent<SphereToucher>();
		StartCoroutine(BlobCoroutine());
	}

	// Update is called once per frame
	void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, 100, blobs)) {
			Debug.Log(hit.transform.position);
			toucher.transform.position = hit.transform.position;
			// sphere.transform.position = hit.point;
		}
		host = toucher.transform.position;
	}

	IEnumerator BlobCoroutine()
	{
		//Print the time of when the function is first called.
		//yield on a new YieldInstruction that waits for 5 seconds.
		while (true) {
			// if (toucher.touches.Length > 0)
			AddBlob();
			yield return new WaitForSeconds(0);
		}
	}

	void AddBlob() {
		List<GameObject> touches = new List<GameObject>(toucher.touches.Values);
		if (touches.Count == 0) return;
		target = touches[Random.Range(0, touches.Count)];
		Vector3 newPoint = GetRandomPointOnMesh(target);
		if (closeToHost(newPoint) && !closeToExistingPoint(newPoint)) {
			CreateBlob(newPoint);
		}
	}

	bool closeToHost(Vector3 newPoint) {
		return Vector3.Distance(newPoint, host) < scale;
	}

	bool closeToExistingPoint(Vector3 newPoint) {
		int i = 0;
		while (i < gameObject.transform.childCount) {
			Vector3 centre = transform.GetChild(i).transform.position;
			if (Vector3.Distance(newPoint, centre) < maxSize) return true;
			i++;
		}
		return false;
	}

	Vector3 GetRandomPointOnMesh(GameObject target) {
		// https://gist.github.com/v21/5378391
		Mesh mesh = target.GetComponent<MeshFilter>().mesh;
		int triangleIndex = Random.Range(0, mesh.triangles.Length) / 3;
		Vector3 p1, p2, p3;
		p1 = target.transform.TransformPoint(mesh.vertices[mesh.triangles[(triangleIndex * 3)]]);
		p2 = target.transform.TransformPoint(mesh.vertices[mesh.triangles[(triangleIndex * 3) + 1]]);
		p3 = target.transform.TransformPoint(mesh.vertices[mesh.triangles[(triangleIndex * 3) + 2]]);
		float r = Random.value;
		float s = Random.value;

		if(r + s >=1)
		{
				r = 1 - r;
				s = 1 - s;
		}

		Vector3 pointOnMesh = p1 + r*(p2 - p1) + s*(p3 - p1);

		return pointOnMesh;
	}

	void CreateBlob(Vector3 point) {
		GameObject g = Instantiate(blobPrefab, point, Quaternion.identity, gameObject.transform);
		g.transform.localScale = new Vector3(0, 0, 0);
		g.GetComponent<Blobber>().manager = this;
		g.layer = LayerMask.NameToLayer("Blobs");
	}
}
