using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Master : MonoBehaviour
{
    public Transform SmallPlatformPrefab;
    public Transform MediumPlatformPrefab;
    public Transform LargePlatformPrefab;
    public Transform DeadlyFloorPrefab;
    public Transform PlayerPrefab;
    public Transform WallPrefab;
    public Transform GuiPrefab;

    private readonly Vector3 basePlatformPosition = new Vector3(-0.805f, -0.5f, 0);
    private readonly Vector3 leftPlatformPosition = new Vector3(-1.115f, -0.05f, 0);
    private readonly Vector3 rightPlatformPosition = new Vector3(0.79f, -0.05f, 0);
    private readonly Vector3 middlePlatformPosition = new Vector3(-0.161f, 0.29f, 0);
    private readonly Vector3 deadlyFloorPosition = new Vector3(-1.925f, -0.86f, 0);
    private readonly Vector3 leftFloorPosition = new Vector3(-2.12f, 0, 0);
    private readonly Vector3 rightFloorPosition = new Vector3(2.12f, 0, 0);
    private readonly Vector3 playerOneSpawnPosition = new Vector3(-0.72f, -0.26f, 0);
    private readonly Vector3 playerTwoSpawnPosition = new Vector3(0.72f, -0.26f, 0);

    private PlayerController playerOneController;
    private PlayerController playerTwoController;
    private GUI gui;

    // Use this for initialization
    void Awake()
    {
        Instantiate(SmallPlatformPrefab, middlePlatformPosition, transform.rotation);
        Instantiate(SmallPlatformPrefab, leftPlatformPosition, transform.rotation);
        Instantiate(SmallPlatformPrefab, rightPlatformPosition, transform.rotation);
        Instantiate(LargePlatformPrefab, basePlatformPosition, transform.rotation);
        Instantiate(DeadlyFloorPrefab, deadlyFloorPosition, transform.rotation);
        Instantiate(WallPrefab, leftFloorPosition, transform.rotation);
        Instantiate(WallPrefab, rightFloorPosition, transform.rotation);
        Transform canvas = (Transform)Instantiate(GuiPrefab, transform.position, transform.rotation);
        gui = canvas.GetComponent<GUI>();

        playerOneController = ((Transform)Instantiate(PlayerPrefab, playerOneSpawnPosition, transform.rotation)).GetComponent<PlayerController>();
        playerTwoController = ((Transform)Instantiate(PlayerPrefab, playerTwoSpawnPosition, transform.rotation)).GetComponent<PlayerController>();
    }

    void Start()
    {
        Reset();
    }

    void Update()
    {
        if (playerOneController.IsDead() || playerTwoController.IsDead())
        {
            var deadP = playerOneController.IsDead() ? playerOneController : playerTwoController;
            var liveP = playerOneController.IsDead() ? playerTwoController : playerOneController;

            liveP.IncrementScore();
            gui.SetTempCenterMessage(deadP + " DEAD!", 3);

            Invoke("Reset", 3);
        }
    }

    void SpawnPlayer(PlayerController controller, float direction, Vector3 position, string inputSet)
    {
        if (controller.IsDead()) controller.Revive();
        controller.Disarm();
        controller.transform.position = position;
        controller.Flip(direction);
        controller.inputSet = inputSet;
    }

    void Reset()
    {
        SpawnPlayer(playerOneController, 1, playerOneSpawnPosition, "P1");
        SpawnPlayer(playerTwoController, -1, playerTwoSpawnPosition, "P2");
    }
}
