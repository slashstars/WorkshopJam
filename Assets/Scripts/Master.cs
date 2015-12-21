using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Master : MonoBehaviour
{
    string[] endTexts = new string[] { "DEAD", "VANQUISHED", "TOASTED", "SLAUGHTERED", "KILLED",
        "DEFEATED", "CREMATED", "NEUTRALIZED", "VAPORIZED", "ANNIHILATED", "DESTROYED", "ROASTED" };

    GameState currentState = GameState.RoundStarting;
    GameState lastState = GameState.None;

    public Transform SmallPlatformPrefab;
    public Transform MediumPlatformPrefab;
    public Transform LargePlatformPrefab;
    public Transform DeadlyFloorPrefab;
    public Transform GuyPrefab;
    public Transform NinjaPrefab;
    public Transform WallPrefab;
    public Transform GuiPrefab;
    public Transform BackgroundPrefab;
    public Transform ClockwiseCog;
    public Transform CounterclockwiseCog;

    private readonly Vector3 basePlatformPosition = new Vector3(-0.805f, -0.5f, 0);
    private readonly Vector3 leftPlatformPosition = new Vector3(-1.115f, -0.05f, 0);
    private readonly Vector3 rightPlatformPosition = new Vector3(0.79f, -0.05f, 0);
    private readonly Vector3 middlePlatformPosition = new Vector3(-0.161f, 0.29f, 0);
    private readonly Vector3 deadlyFloorPosition = new Vector3(-1.925f, -0.86f, 0);
    private readonly Vector3 leftFloorPosition = new Vector3(-2.12f, 0, 0);
    private readonly Vector3 rightFloorPosition = new Vector3(2.12f, 0, 0);
    
    private Vector3[] playerOneSpawningPoints = new Vector3[]
    {
        new Vector3(-0.72f, -0.26f, 0),
        new Vector3(-0.95f, 0.19f, 0)
    };

    private Vector3[] playerTwoSpawningPoints = new Vector3[]
    {
        new Vector3(0.72f, -0.26f, 0),
        new Vector3(0.95f, 0.19f, 0)
    };

    private int playerOneSpawningIndex = 0;
    private int playerTwoSpawningIndex = 1;

    private PlayerController playerOneController;
    private PlayerController playerTwoController;

    private PlayerMeta playerOneMeta;
    private PlayerMeta playerTwoMeta;

    private GUI gui;

    // Use this for initialization
    void Awake()
    {
        Instantiate(BackgroundPrefab, new Vector3(0, 0, 0), transform.rotation);

        Instantiate(ClockwiseCog, new Vector3(1.28f, 0.218f, 0), transform.rotation);
        Instantiate(ClockwiseCog, new Vector3(-0.925f, -0.073f, 0), transform.rotation);

        Instantiate(CounterclockwiseCog, new Vector3(0.871f, 0.315f, 0), transform.rotation);
        Instantiate(CounterclockwiseCog, new Vector3(-0.819f, 0.344f, 0), transform.rotation);

        Instantiate(SmallPlatformPrefab, middlePlatformPosition, transform.rotation);
        Instantiate(SmallPlatformPrefab, leftPlatformPosition, transform.rotation);
        Instantiate(SmallPlatformPrefab, rightPlatformPosition, transform.rotation);
        Instantiate(LargePlatformPrefab, basePlatformPosition, transform.rotation);

        Instantiate(DeadlyFloorPrefab, deadlyFloorPosition, transform.rotation);

        Instantiate(WallPrefab, leftFloorPosition, transform.rotation);
        Instantiate(WallPrefab, rightFloorPosition, transform.rotation);

        gui = ((Transform)Instantiate(GuiPrefab, transform.position, transform.rotation)).GetComponent<GUI>();

        var playerOne = (Transform)Instantiate(NinjaPrefab, playerOneSpawningPoints[playerOneSpawningIndex], transform.rotation);
        var playerTwo = (Transform)Instantiate(GuyPrefab, playerTwoSpawningPoints[playerTwoSpawningIndex], transform.rotation);
        playerOneController = playerOne.GetComponent<PlayerController>();
        playerTwoController = playerTwo.GetComponent<PlayerController>();
        playerOneMeta = InitMeta(playerOne, "Ninja", PlayerID.P1);
        playerTwoMeta = InitMeta(playerTwo, "Thug", PlayerID.P2);
    }

    void Update()
    {
        if (playerOneController.IsDead() || playerTwoController.IsDead())
        {
            currentState = GameState.RoundEnding;
        }

        if (currentState != lastState)
        {
            lastState = currentState;
            if (currentState == GameState.RoundStarting)
            {
                Reset();
            }
            if (currentState == GameState.RoundEnding)
            {
                var deadMeta = playerOneController.IsDead() ? playerOneMeta : playerTwoMeta;
                var aliveMeta = playerOneController.IsDead() ? playerTwoMeta : playerOneMeta;
                aliveMeta.IncrementScore();

                gui.SetTempCenterMessage(deadMeta.playerName + " " + endTexts[Random.Range(0, endTexts.Length)] + "!", 3);

                Invoke("Reset", 3);
            }
        }
    }

    void SpawnPlayer(PlayerController controller, float direction, Vector3 position)
    {
        if (controller.IsDead()) controller.Revive();
        controller.Disarm();
        controller.transform.position = position;
        controller.Flip(direction);
    }

    void Reset()
    {
        gui.SetPlayerScore(playerOneMeta);
        gui.SetPlayerScore(playerTwoMeta);
        playerOneSpawningIndex = (playerOneSpawningIndex + 1) % 2;
        playerTwoSpawningIndex = (playerTwoSpawningIndex + 1) % 2;
        SpawnPlayer(playerOneController, 1, playerOneSpawningPoints[playerOneSpawningIndex]);
        SpawnPlayer(playerTwoController, -1, playerTwoSpawningPoints[playerTwoSpawningIndex]);
        currentState = GameState.RoundInProgress;
    }

    private PlayerMeta InitMeta(Transform player, string name, PlayerID id)
    {
        var meta = player.GetComponent<PlayerMeta>();
        meta.playerName = name;
        meta.playerID = id;
        return meta;
    }
}
