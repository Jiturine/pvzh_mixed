
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : NetworkBehaviour
{

    private static GameManager _instance;
    public static GameManager Instance
    {
        get; set;
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadMainMenuScene();
    }
    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case UIState.MainMenu:
                break;
            case UIState.SelectCard:
                break;
            case UIState.GamePlay:
                if (myHero == null || !decideTurnOrderComplete) return;
                if (myHero.endTurn && enemyHero.endTurn)
                {
                    turnPhase = TurnPhase.End;
                }
                if (myHero.health == 0 || enemyHero.health == 0)
                {
                    gameState = UIState.GameOver;
                    EndGameMenu.SetActive(true);
                }
                if (Input.GetMouseButtonDown(0) && turnPhase == TurnPhase.MyTurn)
                {
                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Collider2D[] hitColliders = Physics2D.OverlapPointAll(mousePosition);
                    if (hitColliders.Length == 0)
                    {
                        if (selectedCard != null)
                        {
                            selectedCard.isSelected = false;
                            selectedCard = null;
                        }
                    }
                    foreach (var collider in hitColliders)
                    {
                        if (selectedCard == null && collider.name == "myPos")
                        {
                            Line line = collider.GetComponentInParent<Line>();
                            if (line.myEntity.Count != 0 && line.myEntity[0].readyToAttack) //有单位，可触发其攻击
                            {
                                EntityAttackServerRpc(line.myEntity[0].faction, line.number);
                                SwitchPhaseServerRpc();
                            }
                        }
                        else if (selectedCard != null && selectedCard.IsApplicableFor(collider))
                        {
                            ApplyCardServerRpc(myHero.faction, myHandCards.cardList.IndexOf(selectedCard), ColliderManager.GetColliderID(collider));
                            if (selectedCard.type == Card.Type.Entity) SwitchPhaseServerRpc();
                        }
                    }
                }
                for (int i = 0; i < lineCount; i++)
                {
                    List<Entity> toDieEntity = new();
                    foreach (Entity entity in line[i].myEntity)
                    {
                        if (entity.IsDying)
                        {
                            toDieEntity.Add(entity);
                        }
                    }
                    foreach (Entity entity in line[i].enemyEntity)
                    {
                        if (entity.IsDying)
                        {
                            toDieEntity.Add(entity);
                        }
                    }
                    foreach (Entity entity in toDieEntity)
                    {
                        entity.Die();
                    }
                }
                if (turnPhase == TurnPhase.DrawCard)
                {
                    if (IsServer)
                    {
                        int drawCardCount = (currentTurn == 1) ? 5 : 1;
                        if (myHero.turnOrder == 0)
                        {
                            for (int i = 0; i < drawCardCount; i++)
                            {
                                DrawCardServerRpc(myHero.faction, Random.Range(0, myDeck.cardList.Count));
                                DrawCardServerRpc(enemyHero.faction, Random.Range(0, enemyDeck.cardList.Count));
                            }
                        }
                        else
                        {
                            for (int i = 0; i < drawCardCount; i++)
                            {
                                DrawCardServerRpc(enemyHero.faction, Random.Range(0, enemyDeck.cardList.Count));
                                DrawCardServerRpc(myHero.faction, Random.Range(0, myDeck.cardList.Count));
                            }
                        }
                        SwitchPhaseClientRpc();
                    }
                }
                else if (turnPhase == TurnPhase.End) //回合开始
                {
                    for (int i = 0; i < lineCount; i++)
                    {
                        foreach (Entity entity in line[i].myEntity)
                        {
                            if (entity.Atk != 0)
                            {
                                entity.readyToAttack = true;
                                entity.counterAttackCount = 1;
                            }

                        }
                        foreach (Entity entity in line[i].enemyEntity)
                        {
                            if (entity.Atk != 0)
                            {
                                entity.readyToAttack = true;
                                entity.counterAttackCount = 1;
                            }
                        }
                    }
                    turnPhase = TurnPhase.DrawCard;
                    currentTurn++;
                    myHero.endTurn = false;
                    enemyHero.endTurn = false;
                    myHero.totalPoint = currentTurn;
                    enemyHero.totalPoint = currentTurn;
                    OnTurnStartEvent?.Invoke();
                }
                //debug
                // if (turnPhase == TurnPhase.EnemyTurn)
                // {
                //     for (int i = 0; i < lineCount; i++)
                //     {
                //         if (line[i].enemyEntity.Count == 0)
                //         {
                //             Transform transform = line[i].transform.Find("enemyPos");
                //             Entity newEntity = Instantiate(CardDictionary.entity[20001], transform.position, Quaternion.identity, transform).GetComponent<Entity>();
                //             line[i].enemyEntity.Add(newEntity);
                //             newEntity.lineIndex = i;
                //             break;
                //         }
                //     }
                //     SwitchPhase();
                //     enemyHero.endTurn = true;
                // }
                break; //case UIState.GamePlay
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void SwitchPhaseServerRpc()
    {
        SwitchPhaseClientRpc();
    }
    [ClientRpc]
    private void SwitchPhaseClientRpc()
    {
        if (turnPhase == TurnPhase.DrawCard)
        {
            if (myHero.turnOrder == 0)
            {
                turnPhase = TurnPhase.MyTurn;
            }
            else
            {
                turnPhase = TurnPhase.EnemyTurn;
            }
        }
        else if (turnPhase == TurnPhase.MyTurn && !enemyHero.endTurn)
        {
            turnPhase = TurnPhase.EnemyTurn;
        }
        else if (turnPhase == TurnPhase.EnemyTurn && !myHero.endTurn)
        {
            turnPhase = TurnPhase.MyTurn;
        }
    }
    public void OnStartHostBtnClick()
    {
        if (NetworkManager.StartHost())
        {
            Debug.Log("Start Host Success!");
            gameMode = GameMode.Online;
            gameState = UIState.SelectCard;
            Debug.Log(NetworkManager.GetComponent<UnityTransport>().ConnectionData.Address);
            Debug.Log(NetworkManager.GetComponent<UnityTransport>().ConnectionData.Port);
            SceneManager.LoadScene("Select Card");
        }
        else
        {
            Debug.Log("Start Host Failed!");
        }
    }
    public void OnStartClientBtnClick()
    {
        if (NetworkManager.StartClient())
        {
            Debug.Log("Start Client Success!");
            gameMode = GameMode.Online;
            gameState = UIState.SelectCard;
            Debug.Log(NetworkManager.GetComponent<UnityTransport>().ConnectionData.Address);
            Debug.Log(NetworkManager.GetComponent<UnityTransport>().ConnectionData.Port);
            SceneManager.LoadScene("Select Card");
        }
        else
        {
            Debug.Log("Start Client Failed!");
        }
    }
    public void OnStartOfflineBtnClick()
    {
        gameMode = GameMode.Offline;
        gameState = UIState.SelectCard;
        SceneManager.LoadScene("Select Card");
    }
    public void OnStartGameBtnClick()
    {
        if (IsHost && readyPlayerNumber == 2)
        {
            StartGameClientRpc();
        }
    }
    public void OnGetReadyBtnClick()
    {
        if (IsServer)
        {
            GetReadyClientRpc();
        }
        else
        {
            GetReadyServerRpc();
        }
        selectCardSceneManager.getReadyButton.enabled = false;
    }
    [ClientRpc]
    private void GetReadyClientRpc()
    {
        readyPlayerNumber++;
    }
    [ServerRpc(RequireOwnership = false)]
    private void GetReadyServerRpc()
    {
        GetReadyClientRpc();
    }
    public void OnEndTurnBtnClick()
    {
        if (turnPhase == TurnPhase.MyTurn)
        {
            EndTurnServerRpc(myHero.faction);
            SwitchPhaseServerRpc();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void EndTurnServerRpc(Faction faction)
    {
        EndTurnClientRpc(faction);
    }
    [ClientRpc]
    private void EndTurnClientRpc(Faction faction)
    {
        GetHero(faction).endTurn = true;
        if (GetOpponentHero(faction).endTurn == false)
        {
            GetHero(faction).turnOrder = 0;
            GetOpponentHero(faction).turnOrder = 1;
        }
    }
    public void OnSwitchHeroBtnClick()
    {
        if (IsHost)
        {
            SwitchHeroClientRpc();
        }
        else if (IsClient)
        {
            SwitchHeroServerRpc();
        }
    }
    public void OnBackToMainMenuBtnClick()
    {
        BackToMainMenuServerRpc();
    }
    [ServerRpc(RequireOwnership = false)]
    private void BackToMainMenuServerRpc()
    {
        BackToMainMenuClientRpc();
    }
    [ClientRpc]
    private void BackToMainMenuClientRpc()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("Main Menu");
    }

    [ServerRpc(RequireOwnership = false)]
    private void SwitchHeroServerRpc()
    {
        SwitchHeroClientRpc();
    }
    [ClientRpc]
    private void SwitchHeroClientRpc()
    {
        myHeroFaction = (myHeroFaction == Faction.Plant) ? Faction.Zombie : Faction.Plant;
        CardLibrary.Clear();
        myDeck.Clear();
        if (myHeroFaction == Faction.Plant)
        {
            CardLibrary.LoadPlant();
        }
        else
        {
            CardLibrary.LoadZombie();
        }
    }
    [ClientRpc]
    private void StartGameClientRpc()
    {
        gameState = UIState.GamePlay;
        deckInfo = ScriptableObject.CreateInstance<DeckInfo>();
        deckInfo.myCardID = new List<int>();
        deckInfo.enemyCardID = new List<int>();
        foreach (Card _card in myDeck.cardList)
        {
            deckInfo.myCardID.Add(_card.ID);
        }
        deckInfo.faction = myHeroFaction;
        TransmitDeckInfoServerRpc(deckInfo.myCardID.ToArray(), IsServer);
        SceneManager.LoadScene("Battle Field");
    }
    [ServerRpc(RequireOwnership = false)]
    private void TransmitDeckInfoServerRpc(int[] cardID, bool isServer)
    {
        TransmitDeckInfoClientRpc(cardID, isServer);
    }
    [ClientRpc]
    private void TransmitDeckInfoClientRpc(int[] cardID, bool isServer)
    {
        if ((isServer && !IsServer) || (!isServer && IsServer))
        {
            foreach (int ID in cardID)
            {
                deckInfo.enemyCardID.Add(ID);
            }
        }
    }
    public void LoadMainMenuScene()
    {
        gameState = UIState.MainMenu;
        gameMode = GameMode.Offline;
        NetworkManager.OnClientConnectedCallback += (id) =>
        {
            Debug.Log("A new client connected, id = " + id);
        };
        NetworkManager.OnClientDisconnectCallback += (id) =>
        {
            Debug.Log("A new client disconnected, id = " + id);
        };
        NetworkManager.OnServerStarted += () =>
        {
            Debug.Log("Server Start!");
        };
        CardDictionary.card = new Dictionary<int, GameObject>
        {
            [10001] = Resources.Load<GameObject>("Prefabs/PeaShooter_card"),
            [10002] = Resources.Load<GameObject>("Prefabs/SunFlower_card"),
            [10003] = Resources.Load<GameObject>("Prefabs/BerryBlast_card"),
            [10004] = Resources.Load<GameObject>("Prefabs/PeaPod_card"),
            [20001] = Resources.Load<GameObject>("Prefabs/CommonZombie_card"),
            [20002] = Resources.Load<GameObject>("Prefabs/VampireZombie_card")
        };
        CardDictionary.entity = new Dictionary<int, GameObject>
        {
            [10001] = Resources.Load<GameObject>("Prefabs/PeaShooter"),
            [10002] = Resources.Load<GameObject>("Prefabs/Sunflower"),
            [10004] = Resources.Load<GameObject>("Prefabs/PeaPod"),
            [20001] = Resources.Load<GameObject>("Prefabs/CommonZombie"),
            [20002] = Resources.Load<GameObject>("Prefabs/VampireZombie")
        };
        AudioManager.Instance.Start();
        AudioManager.Instance.audioSource.clip = AudioManager.Instance.sceneMusic[0];
        AudioManager.Instance.audioSource.Play();
    }
    public void LoadSelectCardScene()
    {
        selectCardSceneManager = GameObject.Find("SceneManager").GetComponent<SelectCardSceneManager>();
        myDeck = FindAnyObjectByType<Deck>();
        if (IsHost)
        {
            myHeroFaction = Faction.Plant;
            CardLibrary.LoadPlant();
        }
        else
        {
            myHeroFaction = Faction.Zombie;
            CardLibrary.LoadZombie();
            selectCardSceneManager.startGameButton.enabled = false;
        }
        AudioManager.Instance.audioSource.Stop();
        AudioManager.Instance.audioSource.clip = AudioManager.Instance.sceneMusic[1];
        AudioManager.Instance.audioSource.Play();
    }
    [ServerRpc]
    private void DecideTurnOrderServerRpc()
    {
        float value = Random.value;
        decideTurnOrderComplete = false;
        DecideTurnOrderClientRpc(value);
    }
    [ClientRpc]
    private void DecideTurnOrderClientRpc(float randomValue)
    {
        if (IsServer)
        {
            myHero.turnOrder = (randomValue > 0.5f) ? 1 : 0;
        }
        else
        {
            myHero.turnOrder = (randomValue > 0.5f) ? 0 : 1;
        }
        decideTurnOrderComplete = true;
    }
    public void LoadBattleFieldScene()
    {
        selectedCard = null;
        currentTurn = 1;
        lineCount = 5;
        line = new Line[5];
        turnPhase = TurnPhase.DrawCard;
        myHero = GameObject.Find("My Hero").GetComponent<Hero>();
        enemyHero = GameObject.Find("Enemy Hero").GetComponent<Hero>();
        enemyDeck = GameObject.Find("Enemy Hero").GetComponent<Deck>();
        myDeck = GameObject.Find("My Hero").GetComponent<Deck>();
        enemyHandCards = GameObject.Find("Enemy Hero").GetComponent<HandCards>();
        myHandCards = GameObject.Find("Hand Cards").GetComponent<HandCards>();
        myHero.totalPoint = 1;
        enemyHero.totalPoint = 1;
        myHero.faction = deckInfo.faction;
        enemyHero.faction = (deckInfo.faction == Faction.Plant) ? Faction.Zombie : Faction.Plant;
        foreach (int id in deckInfo.myCardID)
        {
            Card _card = Instantiate(CardDictionary.card[id]).GetComponent<Card>();
            _card.location = Card.Location.InDeck;
            myDeck.Add(_card);
        }
        foreach (int id in deckInfo.enemyCardID)
        {
            Card _card = Instantiate(CardDictionary.card[id]).GetComponent<Card>();
            _card.location = Card.Location.InDeck;
            enemyDeck.Add(_card);
        }
        for (int i = 0; i < lineCount; i++)
        {
            line[i] = GameObject.Find($"Line{i + 1}").GetComponent<Line>();
            line[i].number = i;
        }
        if (IsServer)
        {
            DecideTurnOrderServerRpc();
        }
        EndGameMenu = GameObject.Find("End Game Menu");
        EndGameMenu.SetActive(false);
        AudioManager.Instance.audioSource.Stop();
    }
    [ServerRpc(RequireOwnership = false)]
    public void DrawCardServerRpc(Faction faction, int index)
    {
        DrawCardClientRpc(faction, index);
    }
    [ClientRpc]
    public void DrawCardClientRpc(Faction faction, int index)
    {
        GetHandCards(faction).DrawFrom(GetDeck(faction), index);
    }
    [ServerRpc(RequireOwnership = false)]
    public void ApplyCardServerRpc(Faction faction, int index, int colliderID)
    {
        ApplyCardClientRpc(faction, index, colliderID);
    }
    [ClientRpc]
    public void ApplyCardClientRpc(Faction faction, int index, int colliderID)
    {
        GetHandCards(faction).cardList[index].ApplyFor(ColliderManager.GetCollider(colliderID));
    }
    [ServerRpc(RequireOwnership = false)]
    public void EntityAttackServerRpc(Faction faction, int lineIndex)
    {
        EntityAttackClientRpc(faction, lineIndex);
    }
    [ClientRpc]
    public void EntityAttackClientRpc(Faction faction, int lineIndex)
    {
        line[lineIndex].GetEntity(faction)[0].Attack();
    }
    private Hero GetHero(Faction faction)
    {
        if (faction == myHero.faction)
        {
            return myHero;
        }
        else
        {
            return enemyHero;
        }
    }
    private Hero GetOpponentHero(Faction faction)
    {
        if (faction == myHero.faction)
        {
            return enemyHero;
        }
        else
        {
            return myHero;
        }
    }
    private Deck GetDeck(Faction faction)
    {
        if (faction == myHero.faction)
        {
            return myDeck;
        }
        else
        {
            return enemyDeck;
        }
    }
    private HandCards GetHandCards(Faction faction)
    {
        if (faction == myHero.faction)
        {
            return myHandCards;
        }
        else
        {
            return enemyHandCards;
        }
    }
    public enum UIState
    {
        MainMenu,
        SelectCard,
        GamePlay,
        GameOver
    };
    public enum GameMode
    {
        Offline,
        Online
    }
    public enum TurnPhase
    {
        DrawCard,
        MyTurn,
        EnemyTurn,
        End
    }
    public enum Faction
    {
        Plant,
        Zombie
    }
    static public int readyPlayerNumber;
    static public TurnPhase turnPhase;
    static public UIState gameState;
    static public GameMode gameMode;
    static public Hero myHero;
    static public Hero enemyHero;
    static public Faction myHeroFaction;
    static public Deck myDeck;
    static public Deck enemyDeck;
    static public HandCards myHandCards;
    static public HandCards enemyHandCards;
    public DeckInfo deckInfo;
    static public Line[] line;
    static public int lineCount;
    static public int currentTurn;
    static public bool decideTurnOrderComplete;
    static public Card selectedCard;
    static public SelectCardSceneManager selectCardSceneManager;
    public delegate void TurnStartHandler();
    static public event TurnStartHandler OnTurnStartEvent;
    static public GameObject EndGameMenu;
}
