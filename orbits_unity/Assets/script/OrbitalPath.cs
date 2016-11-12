using UnityEngine;
using System.Collections.Generic;

public class OrbitalPath : MonoBehaviour {

	public float semimajorAxis = 1;
	public float eccentricity = 0;
	public float inclination = 0;
	public float periapsisArgument = 0;
	public float nodeLongitude = 0;

	static Mesh _mesh;
	public static Mesh mesh { get {
			if (_mesh == null) {
				Mesh mesh = new Mesh();
				mesh.name = "orbital path mesh";
				int vcount = 64;
				Vector3[] verts = new Vector3[vcount];
				int[] indices = new int[vcount + 1];
				for (int v = 0; v < vcount; v ++) {
					verts[v] = Vector3.right * v / vcount;
					indices[v] = v;
				}
				indices[vcount] = 0;
				mesh.vertices = verts;
				mesh.SetIndices(indices, MeshTopology.LineStrip, 0);
				_mesh = mesh;
			}
			return _mesh;
		}
	}

	static Material _mat;
	public static Material mat {
		get {
			if (_mat == null) {
				_mat = new Material(Shader.Find("Orbital Path"));
			}
			return _mat;
		}
	}

	static int _a = -1;
	public static int semimajorAxisPropertyID {
		get {
			if (_a < 0) {
				_a = Shader.PropertyToID("_A");
			}
			return _a;
		}
	}

	static int _e = -1;
	public static int eccentricityPropertyID {
		get {
			if (_e < 0) {
				_e = Shader.PropertyToID("_E");
			}
			return _e;
		}
	}

	MeshFilter filter;
	MeshRenderer render;
	MaterialPropertyBlock block;

	void Awake() {
		filter = gameObject.AddComponent<MeshFilter>();
		filter.mesh = mesh;
		render = gameObject.AddComponent<MeshRenderer>();
		render.sharedMaterial = mat;
		block = new MaterialPropertyBlock();
		UpdateDisplay();
	}

	void UpdateDisplay() {
		UpdateRotation();
		UpdatePropertyBlock();
	}

	void UpdateRotation() {
		transform.rotation = Quaternion.identity;
		transform.Rotate(0, -periapsisArgument, 0, Space.World);
		transform.Rotate(0, 0, -inclination, Space.World);
		transform.Rotate(0, -nodeLongitude, 0, Space.World);
	}

	void UpdatePropertyBlock() {
		if (render != null && block != null) {
			block.SetFloat(semimajorAxisPropertyID, semimajorAxis);
			block.SetFloat(eccentricityPropertyID, eccentricity);
			render.SetPropertyBlock(block);
		}
	}

	void OnValidate() {
		UpdateDisplay();
	}


}
