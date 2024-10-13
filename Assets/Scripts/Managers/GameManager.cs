
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
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
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
        LoadMainMenuScene();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameState != UIState.MainMenu)
        {
            OptionsMenu.Instance.ToggleMenu();
        }
        switch (gameState)
        {
            case UIState.MainMenu:
                break;
            case UIState.SelectCard:
                break;
            case UIState.GamePlay:
                if (myHero == null || !decideTurnOrderComplete) return;
                if (myHero.health == 0 || enemyHero.health == 0)
                {
                    EndGame();
                }
                if (Input.GetMouseButtonDown(0))
                {
                    if (turnPhase == TurnPhase.MyTurn)
                    {
                        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        Collider2D[] hitColliders = Physics2D.OverlapPointAll(mousePosition);
                        foreach (var collider in hitColliders)
                        {
                            if (selectedCard != null)
                            {
                                if (selectedCard.IsApplicableFor(collider))
                                {
                                    if (gameMode == GameMode.Online)
                                    {
                                        ApplyCardServerRpc(myHero.faction, myHandCards.cardList.IndexOf(selectedCard), ColliderManager.colliderID[collider]);
                                        if (selectedCard is EntityCard entityCard)
                                        {
                                            Destroy(entityCard.curEntity);
                                            if (!selectedCard.needToWait)
                                            {
                                                SwitchPhaseServerRpc();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        selectedCard.ApplyFor(collider);
                                        if (selectedCard is EntityCard entityCard)
                                        {
                                            Destroy(entityCard.curEntity);
                                            if (!selectedCard.needToWait)
                                            {
                                                SwitchPhase();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (selectedCard != null)
                    {
                        if (selectedCard is EntityCard _entityCard)
                        {
                            Destroy(_entityCard.curEntity);
                        }
                        selectedCard.isSelected = false;
                        selectedCard = null;
                    }
                }
                if (playingAnimationCounter == 0)
                {
                    CheckDieEntity();
                }
                if (turnPhase == TurnPhase.DrawCard)
                {
                    int drawCardCount = (currentTurn == 1) ? 5 : 1;
                    if (gameMode == GameMode.Online)
                    {
                        if (IsServer)
                        {

                            if (myHero.turnOrder == 0)
                            {
                                for (int i = 0; i < drawCardCount; i++)
                                {
                                    DrawCardServerRpc(myHero.faction);
                                    DrawCardServerRpc(enemyHero.faction);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < drawCardCount; i++)
                                {
                                    DrawCardServerRpc(enemyHero.faction);
                                    DrawCardServerRpc(myHero.faction);
                                }
                            }
                            SwitchPhaseClientRpc();
                        }
                    }
                    else
                    {
                        if (myHero.turnOrder == 0)
                        {
                            for (int i = 0; i < drawCardCount; i++)
                            {
                                myHandCards.DrawFrom(myDeck);
                                enemyHandCards.DrawFrom(enemyDeck);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < drawCardCount; i++)
                            {
                                enemyHandCards.DrawFrom(enemyDeck);
                                myHandCards.DrawFrom(myDeck);
                            }
                        }
                        SwitchPhase();
                    }
                }
                else if (turnPhase == TurnPhase.End) //回合结束 -> 开始
                {
                    foreach (Line line in lines)
                    {
                        line.EndTurn();
                        if (!line.mySlot.Empty)
                        {
                            if (line.mySlot.FirstEntity != null && line.mySlot.FirstEntity.Atk != 0)
                            {
                                line.mySlot.FirstEntity.ReadyToAttack = true;
                                line.mySlot.FirstEntity.counterAttackCount = 1;
                            }
                            if (line.mySlot.SecondEntity != null && line.mySlot.SecondEntity.Atk != 0)
                            {
                                line.mySlot.SecondEntity.ReadyToAttack = true;
                                line.mySlot.SecondEntity.counterAttackCount = 1;
                            }
                        }
                        if (!line.enemySlot.Empty)
                        {
                            if (line.enemySlot.FirstEntity != null && line.enemySlot.FirstEntity.Atk != 0)
                            {
                                line.enemySlot.FirstEntity.ReadyToAttack = true;
                                line.enemySlot.FirstEntity.counterAttackCount = 1;
                            }
                            if (line.enemySlot.SecondEntity != null && line.enemySlot.SecondEntity.Atk != 0)
                            {
                                line.enemySlot.SecondEntity.ReadyToAttack = true;
                                line.enemySlot.SecondEntity.counterAttackCount = 1;
                            }
                        }
                    }
                    turnPhase = TurnPhase.DrawCard;
                    currentTurn++;
                    BattleFieldSceneManager.Instance.currentTurnText.text = $"当前回合：{currentTurn}";
                    myHero.endTurn = false;
                    enemyHero.endTurn = false;
                    myHero.totalPoint = currentTurn;
                    enemyHero.totalPoint = currentTurn;
                    OnTurnStartEvent?.Invoke();
                }
                break; //case UIState.GamePlay
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void SwitchPhaseServerRpc()
    {
        SwitchPhaseClientRpc();
    }
    [ClientRpc]
    public void SwitchPhaseClientRpc()
    {
        if (turnPhase == TurnPhase.DrawCard)
        {
            if (myHero.turnOrder == 0)
            {
                turnPhase = TurnPhase.MyTurn;
                BattleFieldSceneManager.Instance.ShowTurnPhase("我方行动");
                OnMyHeroTurnBeginEvent?.Invoke();
            }
            else
            {
                turnPhase = TurnPhase.EnemyTurn;
                BattleFieldSceneManager.Instance.ShowTurnPhase("对方行动");
                OnEnemyHeroTurnBeginEvent?.Invoke();
            }
        }
        else if (turnPhase == TurnPhase.MyTurn && !enemyHero.endTurn)
        {
            turnPhase = TurnPhase.EnemyTurn;
            BattleFieldSceneManager.Instance.ShowTurnPhase("对方行动");
            OnEnemyHeroTurnBeginEvent?.Invoke();
        }
        else if (turnPhase == TurnPhase.EnemyTurn && !myHero.endTurn)
        {
            turnPhase = TurnPhase.MyTurn;
            BattleFieldSceneManager.Instance.ShowTurnPhase("我方行动");
            OnMyHeroTurnBeginEvent?.Invoke();
        }
    }
    public void SwitchPhase()
    {
        if (turnPhase == TurnPhase.DrawCard)
        {
            if (myHero.turnOrder == 0)
            {
                turnPhase = TurnPhase.MyTurn;
                BattleFieldSceneManager.Instance.ShowTurnPhase("我方行动");
                OnMyHeroTurnBeginEvent?.Invoke();
            }
            else
            {
                turnPhase = TurnPhase.EnemyTurn;
                BattleFieldSceneManager.Instance.ShowTurnPhase("对方行动");
                OnEnemyHeroTurnBeginEvent?.Invoke();
            }
        }
        else if (turnPhase == TurnPhase.MyTurn && !enemyHero.endTurn)
        {
            turnPhase = TurnPhase.EnemyTurn;
            BattleFieldSceneManager.Instance.ShowTurnPhase("对方行动");
            OnEnemyHeroTurnBeginEvent?.Invoke();
        }
        else if (turnPhase == TurnPhase.EnemyTurn && !myHero.endTurn)
        {
            turnPhase = TurnPhase.MyTurn;
            BattleFieldSceneManager.Instance.ShowTurnPhase("我方行动");
            OnMyHeroTurnBeginEvent?.Invoke();
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
            StartCoroutine(SceneLoader.Instance.LoadScene("Select Card"));
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
            StartCoroutine(SceneLoader.Instance.LoadScene("Select Card"));
        }
        else
        {
            Debug.Log("Start Client Failed!");
        }
    }
    public void OnStartOfflineBtnClick()
    {
        Debug.Log("Start Offline Success!");
        gameMode = GameMode.Offline;
        gameState = UIState.SelectCard;
        StartCoroutine(SceneLoader.Instance.LoadScene("Select Card"));
    }
    public void OnStartGameBtnClick()
    {
        if (gameMode == GameMode.Online)
        {
            if (IsHost && readyPlayerNumber == 2)
            {
                selectCardSceneManager.startGameButton.enabled = false;
                StartGameClientRpc();
            }
        }
        else
        {
            StartGame();
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
        if (gameMode == GameMode.Online)
        {
            if (turnPhase == TurnPhase.MyTurn)
            {
                EndTurnServerRpc(myHero.faction);
                SwitchPhaseServerRpc();
            }
        }
        else
        {
            if (turnPhase == TurnPhase.MyTurn)
            {
                MyHeroEndTurn();
            }
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
        else
        {
            GetHero(faction).turnOrder = 1;
            GetOpponentHero(faction).turnOrder = 0;
            turnPhase = TurnPhase.End;
        }
    }
    public void MyHeroEndTurn()
    {
        myHero.endTurn = true;
        if (enemyHero.endTurn == false)
        {
            myHero.turnOrder = 0;
            enemyHero.turnOrder = 1;
        }
        else
        {
            myHero.turnOrder = 1;
            enemyHero.turnOrder = 0;
            turnPhase = TurnPhase.End;
            EnemyAI.Instance.hasApplicableCard = true;
            EnemyAI.Instance.hasReadyToAttackEntity = true;
        }
        SwitchPhase();
    }
    public void EnemyHeroEndTurn()
    {
        enemyHero.endTurn = true;
        if (myHero.endTurn == false)
        {
            enemyHero.turnOrder = 0;
            myHero.turnOrder = 1;
        }
        else
        {
            enemyHero.turnOrder = 1;
            myHero.turnOrder = 0;
            turnPhase = TurnPhase.End;
            EnemyAI.Instance.hasApplicableCard = true;
            EnemyAI.Instance.hasReadyToAttackEntity = true;
        }
        SwitchPhase();
    }
    public void OnSwitchHeroBtnClick()
    {
        if (gameMode == GameMode.Online)
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
        else
        {
            SwitchHero();
        }
    }
    public void OnEndGameMenuBtnClick()
    {
        if (gameMode == GameMode.Online)
        {
            BackToMainMenuServerRpc();
        }
        else
        {
            StartCoroutine(SceneLoader.Instance.LoadScene("Main Menu"));
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void BackToMainMenuServerRpc()
    {
        BackToMainMenuClientRpc();
    }
    [ClientRpc]
    private void BackToMainMenuClientRpc()
    {
        NetworkManager.Singleton.Shutdown();
        StartCoroutine(SceneLoader.Instance.LoadScene("Main Menu"));
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
    private void SwitchHero()
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
        StartCoroutine(SceneLoader.Instance.LoadScene("Battle Field"));
    }
    private void StartGame()
    {
        gameState = UIState.GamePlay;
        deckInfo = ScriptableObject.CreateInstance<DeckInfo>();
        deckInfo.myCardID = new List<int>();
        deckInfo.enemyCardID = new List<int>();
        foreach (Card card in myDeck.cardList)
        {
            deckInfo.myCardID.Add(card.ID);
        }
        int index = (myHeroFaction == Faction.Plant) ? 2 : 1;
        for (int i = 1; i <= CardDictionary.card.Count(kvp => kvp.Key / 10000 == index); i++)
        {
            for (int j = 0; j < 4; j++)
            {
                deckInfo.enemyCardID.Add(10000 * index + i);
            }
        }
        deckInfo.faction = myHeroFaction;
        StartCoroutine(SceneLoader.Instance.LoadScene("Battle Field"));
        selectCardSceneManager.startGameButton.enabled = false;
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
        CardDictionary.Init();
        SpriteManager.Init();
        AudioManager.Instance.Init();
    }
    public void LoadSelectCardScene()
    {
        selectCardSceneManager = GameObject.Find("SceneManager").GetComponent<SelectCardSceneManager>();
        myDeck = FindAnyObjectByType<Deck>();
        if (gameMode == GameMode.Online)
        {
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
        }
        else
        {
            myHeroFaction = Faction.Plant;
            CardLibrary.LoadPlant();
        }
        AudioManager.Instance.Play(1);
    }
    public void LoadBattleFieldScene()
    {
        selectedCard = null;
        currentTurn = 1;
        BattleFieldSceneManager.Instance.currentTurnText.text = $"当前回合：{currentTurn}";
        lineCount = 5;
        lines = new Line[5];
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
            lines[i] = GameObject.Find($"Line{i + 1}").GetComponent<Line>();
            lines[i].index = i;
            lines[i].mySlot.lineIndex = i;
            lines[i].enemySlot.lineIndex = i;
            lines[i].mySlot.faction = myHero.faction;
            lines[i].enemySlot.faction = enemyHero.faction;
        }
        lines[0].terrain = Line.LineTerrain.Highland;
        for (int i = 1; i <= 3; i++)
        {
            lines[i].terrain = Line.LineTerrain.Plain;
        }
        lines[4].terrain = Line.LineTerrain.Water;
        if (gameMode == GameMode.Online)
        {
            if (IsServer)
            {
                InitRandomSeedServerRpc();
                DecideTurnOrderServerRpc();
            }
        }
        else
        {
            myHero.turnOrder = (Random.value > 0.5f) ? 1 : 0;
            enemyHero.turnOrder = myHero.turnOrder ^ 1;
            decideTurnOrderComplete = true;
            gameObject.AddComponent<EnemyAI>();
        }
        if (myHero.faction == Faction.Plant)
        {
            BattleFieldSceneManager.Instance.handCardsImage.sprite = SpriteManager.plantHandCardsSprite;
        }
        else
        {
            BattleFieldSceneManager.Instance.handCardsImage.sprite = SpriteManager.zombieHandCardsSprite;
        }
        EndGameMenu.Instance.gameObject.SetActive(false);
        AudioManager.Instance.Play(2);
    }
    [ServerRpc]
    private void InitRandomSeedServerRpc()
    {
        int seed = Random.Range(0, int.MaxValue);
        InitRandomSeedClientRpc(seed);
    }
    [ClientRpc]
    private void InitRandomSeedClientRpc(int seed)
    {
        Random.InitState(seed);
    }
    [ServerRpc]
    private void DecideTurnOrderServerRpc()
    {
        decideTurnOrderComplete = false;
        DecideTurnOrderClientRpc();
    }
    [ClientRpc]
    private void DecideTurnOrderClientRpc()
    {
        if (IsServer)
        {
            myHero.turnOrder = (Random.value > 0.5f) ? 1 : 0;
        }
        else
        {
            myHero.turnOrder = (Random.value > 0.5f) ? 0 : 1;
        }
        decideTurnOrderComplete = true;
    }
    [ServerRpc(RequireOwnership = false)]
    public void DrawCardServerRpc(Faction faction)
    {
        DrawCardClientRpc(faction);
    }
    [ClientRpc]
    public void DrawCardClientRpc(Faction faction)
    {
        GetHandCards(faction).DrawFrom(GetDeck(faction));
    }
    [ServerRpc(RequireOwnership = false)]
    public void ApplyCardServerRpc(Faction faction, int index, int colliderID)
    {
        ApplyCardClientRpc(faction, index, colliderID);
    }
    [ClientRpc]
    public void ApplyCardClientRpc(Faction faction, int index, int colliderID)
    {
        GetHandCards(faction).cardList[index].ApplyFor(ColliderManager.colliders[colliderID]);
    }
    [ServerRpc(RequireOwnership = false)]
    public void EntityAttackServerRpc(Faction faction, int lineIndex, int position)
    {
        EntityAttackClientRpc(faction, lineIndex, position);
    }
    [ClientRpc]
    public void EntityAttackClientRpc(Faction faction, int lineIndex, int position)
    {
        lines[lineIndex].GetSlot(faction).entities[position].Attack();
    }
    [ServerRpc(RequireOwnership = false)]
    public void MoveEntityServerRpc(Faction faction, int entityLineIndex, int entityPosition, int targetColliderID)
    {
        MoveEntityClientRpc(faction, entityLineIndex, entityPosition, targetColliderID);
    }
    [ClientRpc]
    public void MoveEntityClientRpc(Faction faction, int entityLineIndex, int entityPosition, int targetColliderID)
    {
        MoveEntity(lines[entityLineIndex].GetSlot(faction).entities[entityPosition], ColliderManager.colliders[targetColliderID]);
    }
    public void MoveEntity(Entity selectedEntity, Collider2D collider)
    {
        Slot targetSlot = collider.GetComponentInParent<Slot>();
        if (targetSlot == selectedEntity.slot)
        {
            Entity anotherEntity = targetSlot.GetEntity(collider);
            anotherEntity.transform.SetParent(targetSlot.GetCollider(selectedEntity).transform, false);
            selectedEntity.transform.SetParent(collider.transform, false);
            (targetSlot.SecondEntity, targetSlot.FirstEntity) = (targetSlot.FirstEntity, targetSlot.SecondEntity);
        }
        else
        {
            if (targetSlot.Empty)
            {
                selectedEntity.slot.RemoveEntity(selectedEntity);
                selectedEntity.slot = targetSlot;
                selectedEntity.transform.SetParent(collider.transform, false);
                targetSlot.FirstEntity = selectedEntity;
            }
            else if (targetSlot.FirstEntity != null)
            {
                if (collider == targetSlot.firstCollider)
                {
                    targetSlot.SecondEntity = targetSlot.FirstEntity;
                    targetSlot.SecondEntity.transform.SetParent(targetSlot.secondCollider.transform, false);
                    selectedEntity.slot.RemoveEntity(selectedEntity);
                    selectedEntity.slot = targetSlot;
                    selectedEntity.transform.SetParent(collider.transform, false);
                    targetSlot.FirstEntity = selectedEntity;
                }
                else
                {
                    selectedEntity.slot.RemoveEntity(selectedEntity);
                    selectedEntity.slot = targetSlot;
                    selectedEntity.transform.SetParent(collider.transform, false);
                    targetSlot.SecondEntity = selectedEntity;
                }
            }
            else
            {
                if (collider == targetSlot.secondCollider)
                {
                    targetSlot.FirstEntity = targetSlot.SecondEntity;
                    targetSlot.FirstEntity.transform.SetParent(targetSlot.firstCollider.transform, false);
                    selectedEntity.slot.RemoveEntity(selectedEntity);
                    selectedEntity.slot = targetSlot;
                    selectedEntity.transform.SetParent(collider.transform, false);
                    targetSlot.SecondEntity = selectedEntity;
                }
                else
                {
                    selectedEntity.slot.RemoveEntity(selectedEntity);
                    selectedEntity.slot = targetSlot;
                    selectedEntity.transform.SetParent(collider.transform, false);
                    targetSlot.FirstEntity = selectedEntity;
                }
            }
        }
        OnMoveEntityCompleteEvent?.Invoke();
    }
    public void CheckDieEntity()
    {
        foreach (Line line in lines)
        {
            List<Entity> toDieEntity = new();
            if (!line.mySlot.Empty)
            {
                if (line.mySlot.FirstEntity != null && line.mySlot.FirstEntity.IsDying)
                {
                    toDieEntity.Add(line.mySlot.FirstEntity);
                }
                if (line.mySlot.SecondEntity != null && line.mySlot.SecondEntity.IsDying)
                {
                    toDieEntity.Add(line.mySlot.SecondEntity);
                }
            }
            if (!line.enemySlot.Empty)
            {
                if (line.enemySlot.FirstEntity != null && line.enemySlot.FirstEntity.IsDying)
                {
                    toDieEntity.Add(line.enemySlot.FirstEntity);
                }
                if (line.enemySlot.SecondEntity != null && line.enemySlot.SecondEntity.IsDying)
                {
                    toDieEntity.Add(line.enemySlot.SecondEntity);
                }
            }
            foreach (Entity entity in toDieEntity)
            {
                entity.Die();
            }
        }
    }
    public void EndGame()
    {
        gameState = UIState.GameOver;
        EndGameMenu.Instance.gameObject.SetActive(true);
        if (enemyHero.health == 0)
        {
            EndGameMenu.Instance.text.text = "你赢了";
            AudioManager.Instance.Play(3);
        }
        else
        {
            EndGameMenu.Instance.text.text = "你输了";
            AudioManager.Instance.Play(4);
        }
    }
    public Hero GetHero(Faction faction)
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
    public Hero GetOpponentHero(Faction faction)
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
    public Deck GetDeck(Faction faction)
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
    public HandCards GetHandCards(Faction faction)
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
    public enum Tag
    {
        Pea, Flower, Nut, Berry, Greenery, Athlete, Pet
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
    static public Line[] lines;
    static public int lineCount;
    static public int currentTurn;
    static public bool decideTurnOrderComplete;
    static public Card selectedCard;
    static public SelectCardSceneManager selectCardSceneManager;
    static public SceneLoader sceneLoader;
    static public event Action OnTurnStartEvent;
    static public event Action OnMoveEntityCompleteEvent;
    static public event Action OnMyHeroTurnBeginEvent;
    static public event Action OnEnemyHeroTurnBeginEvent;
    static public void AddTurnBeginEvent(Faction faction, Action action)
    {
        if (faction == myHero.faction)
        {
            OnMyHeroTurnBeginEvent += action;
        }
        else
        {
            OnEnemyHeroTurnBeginEvent += action;
        }
    }
    static public void RemoveTurnBeginEvent(Faction faction, Action action)
    {
        if (faction == myHero.faction)
        {
            OnMyHeroTurnBeginEvent -= action;
        }
        else
        {
            OnEnemyHeroTurnBeginEvent -= action;
        }
    }
    static public int playingAnimationCounter;
}
