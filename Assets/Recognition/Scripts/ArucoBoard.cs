using UnityEngine;
using System.Collections.Generic;

public class ArucoBoard : MonoBehaviour
{
    [SerializeField]
    public class Marker
    {
        public int id = -1;
        public GameObject gameObject = null;
        public GameObject centerObject = null;
        public bool detected = false;
        public float x = -1;
        public float y = -1;

        public Marker(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }

    static private ArucoBoard instance;
	static private readonly Vector3 offset = new Vector3(-0.032f, 0f, 0f);

    private Dictionary<int, Marker> markers_ = new Dictionary<int, Marker>() 
    {
        { 151, new Marker(-2.5f, -1.5f) },
        { 896, new Marker(-1.5f, -1.5f) },
        { 404, new Marker(-0.5f, -1.5f) },
        { 328, new Marker( 0.5f, -1.5f) },
        { 606, new Marker( 1.5f, -1.5f) },
        {   8, new Marker( 2.5f, -1.5f) },
        { 577, new Marker(-2.5f, -0.5f) },
        { 682, new Marker(-1.5f, -0.5f) },
        { 664, new Marker(-0.5f, -0.5f) },
        {  97, new Marker( 0.5f, -0.5f) },
        { 373, new Marker( 1.5f, -0.5f) },
        { 432, new Marker( 2.5f, -0.5f) },
        { 570, new Marker(-2.5f,  0.5f) },
        { 542, new Marker(-1.5f,  0.5f) },
        { 269, new Marker(-0.5f,  0.5f) },
        { 703, new Marker( 0.5f,  0.5f) },
        { 590, new Marker( 1.5f,  0.5f) },
        { 298, new Marker( 2.5f,  0.5f) },
        {  98, new Marker(-2.5f,  1.5f) },
        { 519, new Marker(-1.5f,  1.5f) },
        { 498, new Marker(-0.5f,  1.5f) },
        { 127, new Marker( 0.5f,  1.5f) },
        { 844, new Marker( 1.5f,  1.5f) },
        { 742, new Marker( 2.5f,  1.5f) },
    };

    public Transform center;
    public GameObject centerObjectPrefab;
    public GameObject markerObjectPrefab;
    public Transform targetCamera;
    public float markerScale = 6f;

    public float posThresh = 0.005f;
    public float fwdThresh = 0.01f;
    public float upThresh = 0.01f;

    private Vector3 markersCenter_;
    private Vector3 markersUp_;

    bool IsTargetMareker(int id)
    {
        return markers_.ContainsKey(id);
    }

    void CreateMarkerGameObject(int id)
    {
        if (!IsTargetMareker(id)) return;

        var marker = markers_[id];
        marker.id = id;

        if (!marker.gameObject) {
            marker.gameObject = Instantiate(markerObjectPrefab) as GameObject;
            marker.gameObject.name = "Marker " + id;
            marker.centerObject = Instantiate(centerObjectPrefab) as GameObject;
        }
    }

    static public void Detect(float[] data, int num, int stride)
    {
        if (instance == null) return;
        instance.DetectMarkers(data, num, stride);
    }

    public void DetectMarkers(float[] data, int num, int stride)
    {
        foreach (var pair in markers_) {
            pair.Value.detected = false;
        }

        for (int i = 0; i < num; ++i) {
            var j = stride * i;
            var id = (int)data[j];
            if (!IsTargetMareker(id)) continue;
            var pos = new Vector3(data[j + 1] + offset.x, data[j + 2] + offset.y, data[j + 3] + offset.z);
            var rot = new Quaternion(data[j + 4], data[j + 5], data[j + 6], data[j + 7]);
            rot *= Quaternion.AngleAxis(-90, Vector3.up);
            SetCameraLocalTransform(id, pos, rot);
        }

        foreach (var pair in markers_) {
            var marker = pair.Value;
            CreateMarkerGameObject(pair.Key);
            marker.gameObject.SetActive(marker.detected);
            marker.centerObject.SetActive(marker.detected);
        }
    }

    void SetCameraLocalTransform(int id, Vector3 pos, Quaternion rot)
    {
        if (!markers_.ContainsKey(id)) return;

        var marker = markers_[id];
        CreateMarkerGameObject(id);

        marker.detected = true;
        var tform = marker.gameObject.transform;
        var originalParent = tform.parent;
        tform.parent = targetCamera;
        tform.localPosition = pos;
        tform.localRotation = rot;
        tform.parent = originalParent;
    }

    void Update()
    {
        Calibration();
    }

    void OnEnable()
    {
        instance = this;
    }

    void OnDisable()
    {
        if (instance == this) instance = null;
    }

    void Calibration()
    {
        var n = 0;
        var centerPos = Vector3.zero;
        var centerFwd = Vector3.zero;
        var centerUp  = Vector3.zero;

        foreach (var pair in markers_) {
            var marker = pair.Value;
            if (!marker.detected) continue;

            var tform = marker.gameObject.transform;
            var size = markerScale / 100;
            var dx = tform.right   * marker.x * size;
            var dz = tform.forward * marker.y * size;
            var pos = tform.position - dx + dz;
            centerPos += pos;
            centerFwd += tform.forward;
            centerUp  += tform.up;
            marker.centerObject.transform.position = pos;
            marker.centerObject.transform.rotation = tform.rotation;
            ++n;
        }

        centerPos /= n;
        centerFwd /= n;
        centerUp  /= n;

        var averagePos = centerPos;
        var averageFwd = centerFwd;
        var averageUp  = centerUp;

        n = 0;
        centerPos = Vector3.zero;
        centerFwd = Vector3.zero;
        centerUp  = Vector3.zero;

        foreach (var pair in markers_) {
            var marker = pair.Value;
            if (!marker.detected) continue;
            var center = marker.centerObject.transform;
            if (Vector3.Distance(averagePos, center.position) < posThresh &&
                Vector3.Distance(averageFwd, center.forward)  < fwdThresh &&
                Vector3.Distance(averageUp,  center.up)       < upThresh) {
                centerPos += center.position;
                centerFwd += center.forward;
                centerUp  += center.up;
                ++n;
            }
        }

        centerPos /= n;
        centerFwd /= n;
        centerUp  /= n;

        if (n > 0) {
            center.position = centerPos;
            center.rotation = Quaternion.LookRotation(centerFwd, centerUp);
        }

        if (n > 5) {
            foreach (var pair in markers_) {
                var marker = pair.Value;
                if (!marker.detected) continue;
                var tform = marker.centerObject.transform;
                tform.position = center.position;
                tform.rotation = center.rotation;
            }
        }
    }
}
