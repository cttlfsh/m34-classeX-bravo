using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    #region VARIABLES
    public static TowerManager Instance;


    public int towerHeight;


    [Header("Tower Prefabs")]
    public GameObject firstTowerBase;
    public GameObject secondTowerBase;
    public GameObject towerFloorPrefab;
    public GameObject towerTop;
    [Space]
    [SerializeField] private Vector3 spawnOffset;

    private List<Floor> firstTowerFloors = new List<Floor>();
    private List<Floor> secondTowerFloors = new List<Floor>();
    private List<string> firstTowerPlayerNicknames = new List<string>();
    private List<string> secondTowerPlayerNicknames = new List<string>();
    private bool isGameOver;
    private int roundsToPlay;

    #endregion

    #region PUBLIC_METHODS
    #region TWITCH_BINDING_METHODS
    /// <summary>
    /// Method bound to the Twitch bot called when a chat member wants to join 
    /// the game. It checks if the player cap hasn't been reached and in that case
    /// where to put the recently joined character.
    /// It stores the players nickname in two lists which will be used later on to
    /// instantiate the GameObjects tower floors
    /// If one of the two lists has already reached the cap, it puts it on the other, otherwise
    /// the tower choice is random.
    /// </summary>
    /// <param name="nickname"> The nickname of the Twitch user to add</param>
    public void JoinGame(string nickname)
    {
        if (!CheckPlayerNickname(nickname))
        {
            int towerIndex = SortPlayerInTowers();
            if (towerIndex == 0)
            {
                firstTowerPlayerNicknames.Add(nickname);
                Debug.Log($"Player: {nickname} joined on the FIRST tower");
                GameManager.Instance.playerInGame += 1;

            }
            else if (towerIndex == 1)
            {
                secondTowerPlayerNicknames.Add(nickname);
                Debug.Log($"Player: {nickname} joined on the SECOND tower");
                GameManager.Instance.playerInGame += 1;
            }
            else
            {
                Debug.Log("Player cap reached");
            }
        }
        else
        {
            Debug.Log($"Player {nickname} has already joined the match");
        }
    }

    /// <summary>
    /// Method bound to the Twitch bot called when the player wants to select a magic
    /// to play. It checks on the two nickname lists if a corresponding player has
    /// previousely joined the game and eventually sets on their Floor reference the
    /// desired move
    /// </summary>
    /// <param name="nickname">Player nickname to search</param>
    /// <param name="move">The selected move</param>
    public void SelectMove(string nickname, int move)
    {
        foreach (Floor floor in firstTowerFloors)
        {
            if (floor.PlayerNickname == nickname)
            {
                floor.Move = move;
                Debug.Log($"Player {nickname} selected {RockPaperScissor.moves[move]} magic");
                return;
            }
        }

        foreach (Floor floor in secondTowerFloors)
        {
            if (floor.PlayerNickname == nickname)
            {
                floor.Move = move;
                Debug.Log($"Player {nickname} selected {RockPaperScissor.moves[move]} magic");
                return;
            }
        }
    }

    /// <summary>
    /// Method bound to the Twitch bot called when the chat needs to choose to which
    /// tower send a modifier (Bonus/Malus).
    /// </summary>
    /// <param name="tower">Tower preference from a chat player</param>
    public void AssignModifier(int tower)
    {

    }
    #endregion

    /// <summary>
    /// Method which resets all the variables at the initial condition. It is
    /// called both when the game starts the first time and each time the game
    /// restarts
    /// </summary>
    public void ResetTowers()
    {
        isGameOver = false;
    }
    #endregion

    #region PRIVATE_METHODS
    /// <summary>
    /// Method which sorts the player in one of the two towers, depending if one
    /// of the two already reached the player cap or not.
    /// </summary>
    /// <returns>the index of the tower, -1 if the player cap has already been reached in both</returns>
    private int SortPlayerInTowers()
    {
        if (firstTowerPlayerNicknames.Count == towerHeight)
        {
            if (secondTowerPlayerNicknames.Count == towerHeight)
            {
                return -1;
            }
            return 1;
        }
        else
        {
            if (secondTowerPlayerNicknames.Count == towerHeight)
            {
                return 0;
            }
        }
        return Random.Range(0, 2);
    }

    /// <summary>
    /// Methods which checks if a player has already joined the match based on its nickname
    /// </summary>
    /// <param name="nickname">Nickname of the player to check</param>
    /// <returns>Returns TRUE if the player has already joined, false otherwise</returns>
    private bool CheckPlayerNickname(string nickname)
    {
        foreach (string ncknm in firstTowerPlayerNicknames)
        { 
            if (ncknm == nickname)
            {
                return true;
            }
        }
        foreach (string ncknm in secondTowerPlayerNicknames)
        {
            if (ncknm == nickname)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Method which generates the two towers. First it Instantiate the first floor at the spawn
    /// position. Then it iterates over the lenght of the `nicknameList` to Instantiate all the
    /// floors. Eventually if add the rooftop.
    /// </summary>
    /// <param name="towerSpawn">Spawn position of the tower</param>
    /// <param name="towerToGenerate">Reference to the tower to generate</param>
    /// <param name="nicknameList">Reference to the list of the palyer nicknames</param>
    /// <param name="towerIndex">Index of the tower to generate</param>
    private void GenerateTower(GameObject towerSpawn, List<Floor> towerToGenerate, List<string> nicknameList, int towerIndex)
    {
        GameObject baseFloor = Instantiate(towerFloorPrefab, towerSpawn.transform.position + spawnOffset, Quaternion.identity);
        baseFloor.transform.parent = towerSpawn.transform;
        Floor baseFloorBehaviour = baseFloor.GetComponent<Floor>();
        baseFloorBehaviour.towerIndex = towerIndex;
        if (nicknameList[0].StartsWith("test"))
        {
            baseFloorBehaviour.isNPC = true;
        }
        baseFloorBehaviour.PlayerNickname = nicknameList[0];
        towerToGenerate.Add(baseFloorBehaviour);

        for (int i = 1; i < nicknameList.Count; i++)
        {
            GameObject floor = Instantiate(towerFloorPrefab, towerToGenerate[i - 1].gameObject.transform.position + spawnOffset, Quaternion.identity);
            floor.transform.parent = towerSpawn.transform;
            Floor floorBehaviour = floor.GetComponent<Floor>();
            floorBehaviour.towerIndex = towerIndex;
            if (nicknameList[i].StartsWith("test"))
            {
                floorBehaviour.isNPC = true;
            }
            floorBehaviour.PlayerNickname = nicknameList[i];
            towerToGenerate.Add(floorBehaviour);
        }

        GameObject topFloor = Instantiate(towerTop, towerToGenerate[towerToGenerate.Count - 1].gameObject.transform.position + spawnOffset, Quaternion.identity);
        topFloor.transform.parent = towerSpawn.transform;
    }

    #region GAME_READY_METHODS
    /// <summary>
    /// Method which prepare the conditions to play the round, checking how many matches there will be to play
    /// </summary>
    private void SetupRound()
    {
        Debug.Log($"Number of rounds to play: {Mathf.Min(firstTowerFloors.Count, secondTowerFloors.Count)}");
        roundsToPlay = Mathf.Min(firstTowerFloors.Count, secondTowerFloors.Count);
    }

    /// <summary>
    /// Method which plays a round of matches between the two tower. It computes how many 
    /// matches it has to simulate depending of the lenght of the smallest tower and
    /// starts the RPS game for each of this matches.
    /// Finally it updates the scores of the winner/draw players and deletes the losers
    /// </summary>
    private void Play()
    {
        SetupRound();
        List<Floor> firstTowerLosers = new List<Floor>();
        List<Floor> secondTowerLosers = new List<Floor>();
        // lancia una partita per ogni coppia di piani
        for (int i = 0; i < roundsToPlay; i++)
        {
            // ricevi i vincitori come interi
            Debug.Log($"MATCH STARTING: Tower 0 (Floor {i}, Move {firstTowerFloors[i].Move})  vs Tower 1 (Floor {i}, Move {secondTowerFloors[i].Move})");
            int winnerTower = RockPaperScissor.PlayRPSTurn(firstTowerFloors[i].Move, secondTowerFloors[i].Move);
            if (winnerTower == 0)
            {
                secondTowerLosers.Add(secondTowerFloors[i]);
                firstTowerFloors[i].Points += 3;
            }
            else if (winnerTower == 1)
            {
                firstTowerLosers.Add(firstTowerFloors[i]);
                secondTowerFloors[i].Points += 3;
            }
            else
            {
                firstTowerFloors[i].Points += 1;
                secondTowerFloors[i].Points += 1;
            }

        }
        // distruggi i perdenti
        Debug.Log($"FIRST TOWER LOSERS COUNT: {firstTowerLosers.Count}");
        List<Floor> firstTMP = firstTowerFloors.Except(firstTowerLosers).ToList();
        firstTowerFloors = firstTMP;
        Debug.Log($"SECOND TOWER LOSERS COUNT: {secondTowerLosers.Count}");
        List<Floor> secondTMP = secondTowerFloors.Except(secondTowerLosers).ToList();
        secondTowerFloors = secondTMP;
        DestroyLoserFloors(firstTowerLosers);
        DestroyLoserFloors(secondTowerLosers);
    }

    /// <summary>
    /// Helper method which destroys the loser floors at each round for a specified tower
    /// </summary>
    /// <param name="losers">The tower where to delete the losers</param>
    private void DestroyLoserFloors(List<Floor> losers)
    {
        for (int i = 0; i < losers.Count; i++)
        {
            Destroy(losers[i].gameObject);
        }
    }

    /// <summary>
    /// Method which checks if the win condition are satisfied at each round
    /// </summary>
    private void CheckWin()
    {
        if (firstTowerFloors.Count < 1)
        {
            print($"SECOND TOWER WINS!");
            isGameOver = true;

        }
        else if (secondTowerFloors.Count < 1)
        {
            print($"FIRST TOWER WINS!");
            isGameOver = true;

        }
    }
    #endregion
    #endregion

    #region UNITY_METHODS

    private void Awake()
    {
        #region SINGLETON
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        #endregion
        ResetTowers();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (GameManager.Instance.gamePhase == "Lobby")
            {
                int npcToAdd = towerHeight * 2 - GameManager.Instance.playerInGame;
                Debug.Log($"NPC to add: {npcToAdd}");
                for (int i = 0; i < npcToAdd; i++)
                {
                    JoinGame($"test{i}");
                }
                GameManager.Instance.gamePhase = "PowerSelection";
                Debug.Log(firstTowerPlayerNicknames.Count);
                Debug.Log(secondTowerPlayerNicknames.Count);
            }
        }
        if (Input.GetKeyDown(KeyCode.H) && GameManager.Instance.gamePhase == "GameReady")
        {
            foreach (Floor floor in firstTowerFloors)
            {
                if (floor.isNPC)
                {
                    floor.Move = Random.Range(0, 3);

                }
            }
            foreach (Floor floor in secondTowerFloors)
            {
                if (floor.isNPC)
                {
                    floor.Move = Random.Range(0, 3);
                }
            }
            GameManager.Instance.gamePhase = "GameReady";
        }
        if (Input.GetKeyDown(KeyCode.G) && GameManager.Instance.gamePhase == "PowerSelection")
        {
            GenerateTower(firstTowerBase, firstTowerFloors, firstTowerPlayerNicknames, 0);
            GenerateTower(secondTowerBase, secondTowerFloors, secondTowerPlayerNicknames, 1);
            GameManager.Instance.gamePhase = "GameReady";
        }
        if (Input.GetKeyDown(KeyCode.P) && GameManager.Instance.gamePhase == "GameReady")
        {
            // rigioca la partita solo con i piani che hanno un nemico
            Play();
        }
    }
    #endregion
}
