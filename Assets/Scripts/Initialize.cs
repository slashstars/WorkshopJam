using UnityEngine;
using System.Collections;

public class Initialize : MonoBehaviour {

    public Transform SmallPlatformPrefab;
    public Transform MediumPlatformPrefab;
    public Transform LargePlatformPrefab;
    public Transform DeadlyFloorPrefab;

    private Vector3 BasePlatformPosition = new Vector3(-0.805f, -0.5f, 0);
    private Vector3 LeftPlatformPosition = new Vector3(-1.115f, -0.05f, 0);
    private Vector3 RightPlatformPosition = new Vector3(0.79f, -0.05f, 0);
    private Vector3 MiddlePlatformPosition = new Vector3(-0.161f, 0.29f, 0);
    private Vector3 DeadlyFloorPosition = new Vector3(-1.925f, -0.86f, 0);

    // Use this for initialization
    void Awake () {
        Instantiate(SmallPlatformPrefab, MiddlePlatformPosition, transform.rotation);
        Instantiate(SmallPlatformPrefab, LeftPlatformPosition, transform.rotation);
        Instantiate(SmallPlatformPrefab, RightPlatformPosition, transform.rotation);
        Instantiate(LargePlatformPrefab, BasePlatformPosition, transform.rotation);
        Instantiate(DeadlyFloorPrefab, DeadlyFloorPosition, transform.rotation);
    }
}
