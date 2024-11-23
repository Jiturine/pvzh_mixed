
using System;
using System.Collections.Generic;
using System.Linq;
using static Game;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; set; }
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
    void Update()
    {
        if (gameState == GameState.GamePlay)
        {
            if (myHero == null || !decideTurnOrderComplete) return;
            if (myHero.health == 0 || enemyHero.health == 0)
            {
                EndGame();
            }
            ActionSequence.Update();
            if (!ActionSequence.HasAction<AttackAction>())
            {
                CheckDieEntity();
            }
            if (ActionSequence.Empty())
            {
                if (turnStateMachine.currentState is DrawCardState drawCardState && drawCardState.ended)
                {
                    SwitchPhase();
                }
                else if (Game.State is EndTurnState endTurnState && endTurnState.ended)
                {
                    turnStateMachine.SwitchState<DrawCardState>();
                }
            }
        }
    }
    public void SwitchPhase()
    {
        if (Game.State is DrawCardState)
        {
            if (myHero.turnOrder == 0)
            {
                turnStateMachine.SwitchState<MyTurnState>();
            }
            else
            {
                turnStateMachine.SwitchState<EnemyTurnState>();
            }
        }
        else if (Game.State is MyTurnState)
        {
            if (enemyHero.EndTurn)
            {
                turnStateMachine.SwitchState<MyTurnState>();
            }
            else
            {
                turnStateMachine.SwitchState<EnemyTurnState>();
            }

        }
        else if (Game.State is EnemyTurnState)
        {
            if (myHero.EndTurn)
            {
                turnStateMachine.SwitchState<EnemyTurnState>();
            }
            else
            {
                turnStateMachine.SwitchState<MyTurnState>();
            }
        }
    }
    public void OnStartOfflineBtnClick()
    {
        AudioManager.Instance.PlaySFX("ButtonClick");
        gameMode = GameMode.Offline;
        gameState = GameState.SelectCard;
        SceneManager.LoadScene("Select Card");
    }
    public void OnStartGameBtnClick()
    {
        AudioManager.Instance.PlaySFX("ButtonClick");
        if (myDeck.cardList.Count != 40)
        {
            var messageBoxPanel = UIManager.Instance.TryOpenPanel<MessageBoxPanel>();
            messageBoxPanel.ShowMessage("牌数不符合要求！", 3f);
        }
        else if (gameMode == GameMode.Online)
        {
            if (IsHost && readyPlayerNumber == 2)
            {
                InitRandomSeedServerRpc();
                StartGameClientRpc();
            }
        }
        else
        {
            StartGame();
        }
    }
    public void OnToggleReadyBtnClick()
    {
        if (myDeck.cardList.Count != 40 && !isReady)
        {
            var messageBoxPanel = UIManager.Instance.TryOpenPanel<MessageBoxPanel>();
            messageBoxPanel.ShowMessage("牌数不符合要求！", 3f);
        }
        else
        {
            isReady = !isReady;
            if (IsServer)
            {
                ToggleReadyClientRpc(isReady);
            }
            else
            {
                ToggleReadyServerRpc(isReady);
            }
        }
    }
    [ClientRpc]
    private void ToggleReadyClientRpc(bool _isReady)
    {
        if (_isReady) readyPlayerNumber++;
        else readyPlayerNumber--;
    }
    [ServerRpc(RequireOwnership = false)]
    private void ToggleReadyServerRpc(bool _isReady)
    {
        ToggleReadyClientRpc(_isReady);
    }
    public void EndTurn(Faction faction)
    {
        GetHero(faction).EndTurn = true;
        if (GetOpponentHero(faction).EndTurn == false)
        {
            GetHero(faction).turnOrder = 0;
            GetOpponentHero(faction).turnOrder = 1;
            ActionSequence.actionSequence.AddFirst(new SwitchPhaseAction());
        }
        else
        {
            GetHero(faction).turnOrder = 1;
            GetOpponentHero(faction).turnOrder = 0;
            turnStateMachine.SwitchState<EndTurnState>();
        }
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
            if (gameObject.TryGetComponent<EnemyAI>(out var enemyAI))
            {
                Destroy(enemyAI);
            }
            SceneManager.LoadScene("Main Menu");
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
        tempFaction = (tempFaction == Faction.Plant) ? Faction.Zombie : Faction.Plant;
        CardLibrary.Clear();
        myDeck.Clear();
        if (tempFaction == Faction.Plant)
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
        tempFaction = (tempFaction == Faction.Plant) ? Faction.Zombie : Faction.Plant;
        CardLibrary.Clear();
        myDeck.Clear();
        if (tempFaction == Faction.Plant)
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
        gameState = GameState.GamePlay;
        deckInfo = ScriptableObject.CreateInstance<DeckInfo>();
        deckInfo.myCardID = new List<int>();
        deckInfo.enemyCardID = new List<int>();
        foreach (Card _card in myDeck.cardList)
        {
            deckInfo.myCardID.Add(_card.ID);
        }
        deckInfo.faction = tempFaction;
        TransmitDeckInfoServerRpc(deckInfo.myCardID.ToArray(), IsServer);
        SceneLoader.Instance.LoadSceneAsync("Battle Field");
    }
    private void StartGame()
    {
        gameState = GameState.GamePlay;
        deckInfo = ScriptableObject.CreateInstance<DeckInfo>();
        deckInfo.myCardID = new List<int>();
        deckInfo.enemyCardID = new List<int>();
        foreach (Card card in myDeck.cardList)
        {
            deckInfo.myCardID.Add(card.ID);
        }
        int index = (tempFaction == Faction.Plant) ? 2 : 1;
        for (int i = 1; i <= CardDictionary.card.Count(kvp => kvp.Key / 10000 == index); i++)
        {
            for (int j = 0; j < 4; j++)
            {
                deckInfo.enemyCardID.Add(10000 * index + i);
            }
        }
        deckInfo.faction = tempFaction;
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
        gameState = GameState.MainMenu;
        gameMode = GameMode.Offline;
    }
    public void LoadSelectCardScene(SelectCardPanel selectCardPanel)
    {
        CardLibrary.Init();
        myDeck = selectCardPanel.myDeck;
        if (gameMode == GameMode.Online)
        {
            if (IsHost)
            {
                selectCardPanel.joinCodeText.text = $"邀请码:{RelayManager.Instance.joinCode}";
                tempFaction = Faction.Plant;
                CardLibrary.LoadPlant();
            }
            else
            {
                selectCardPanel.joinCodeText.enabled = false;
                tempFaction = Faction.Zombie;
                CardLibrary.LoadZombie();
            }
        }
        else
        {
            selectCardPanel.joinCodeText.enabled = false;
            tempFaction = Faction.Plant;
            CardLibrary.LoadPlant();
        }
        isReady = false;
    }
    public void LoadBattleFieldScene(BattleFieldPanel battleFieldPanel, HandCardsPanel handCardsPanel)
    {
        SelectedCard = null;
        currentTurn = 0;
        lines = new Line[lineCount];
        ActionSequence.Init();
        myHero = battleFieldPanel.myHero;
        enemyHero = battleFieldPanel.enemyHero;
        myDeck = battleFieldPanel.myDeck;
        enemyDeck = battleFieldPanel.enemyDeck;
        enemyHandCards = battleFieldPanel.enemyHandCards;
        myHandCards = handCardsPanel.myHandCards;
        myHero.faction = deckInfo.faction;
        enemyHero.faction = (deckInfo.faction == Faction.Plant) ? Faction.Zombie : Faction.Plant;
        foreach (int id in deckInfo.myCardID)
        {
            myDeck.Add(id);
        }
        foreach (int id in deckInfo.enemyCardID)
        {
            enemyDeck.Add(id);
        }
        for (int i = 0; i < lineCount; i++)
        {
            lines[i] = GameObject.Find($"Line{i + 1}").GetComponent<Line>();
            lines[i].mySlot.Faction = myHero.faction;
            lines[i].enemySlot.Faction = enemyHero.faction;
        }
        if (gameMode == GameMode.Online)
        {
            if (IsServer)
            {
                DecideTurnOrderServerRpc();
            }
        }
        else
        {
            myHero.turnOrder = (Random.value > 0.5f) ? 1 : 0;
            enemyHero.turnOrder = myHero.turnOrder ^ 1;
            decideTurnOrderComplete = true;
            if (!gameObject.TryGetComponent<EnemyAI>(out var enemyAI))
            {
                gameObject.AddComponent<EnemyAI>();
            }
        }
        turnStateMachine.SwitchState<DrawCardState>();
    }
    [ServerRpc(RequireOwnership = false)]
    public void AddActionServerRpc(GameAction.Type type, int[] args)
    {
        AddActionClientRpc(type, args);
    }
    [ClientRpc]
    public void AddActionClientRpc(GameAction.Type type, int[] args)
    {
        Type _type = System.Type.GetType(GameAction.stringDict[type]);
        GameAction gameAction = System.Activator.CreateInstance(_type, args) as GameAction;
        ActionSequence.actionSequence.AddLast(gameAction);
    }
    [ServerRpc(RequireOwnership = false)]
    public void AddInstantActionServerRpc(GameAction.Type type, int[] args)
    {
        AddInstantActionClientRpc(type, args);
    }
    [ClientRpc]
    public void AddInstantActionClientRpc(GameAction.Type type, int[] args)
    {
        Type _type = System.Type.GetType(GameAction.stringDict[type]);
        GameAction gameAction = System.Activator.CreateInstance(_type, args) as GameAction;
        ActionSequence.actionSequence.AddFirst(gameAction);
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
    public void ActionSequenceUnlockServerRpc()
    {
        ActionSequenceUnlockClientRpc();
    }
    [ClientRpc]
    public void ActionSequenceUnlockClientRpc()
    {
        ActionSequence.Unlock();
    }
    public void PlaceEntity(Entity entity, Collider2D collider)
    {
        Slot slot = collider.GetComponentInParent<Slot>();
        entity.slot = slot;
        if (slot.Empty)
        {
            slot.FirstEntity = entity;
        }
        else if (slot.FirstEntity != null)
        {
            if (collider == slot.FirstCollider)
            {
                slot.SecondEntity = slot.FirstEntity;
                slot.SecondEntity.transform.SetParent(slot.SecondCollider.transform, false);
                slot.FirstEntity = entity;
            }
            else
            {
                slot.SecondEntity = entity;
            }
        }
        else
        {
            if (collider == slot.FirstCollider)
            {
                slot.FirstEntity = entity;
            }
            else
            {
                slot.FirstEntity = slot.SecondEntity;
                slot.FirstEntity.transform.SetParent(slot.FirstCollider.transform, false);
                slot.SecondEntity = entity;
            }
        }
        entity.Place();
        if (entity is not GravestoneEntity)
        {
            OnPlaceEntityEvent?.Invoke(entity);
        }
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
        gameState = GameState.GameOver;
        var endGamePanel = UIManager.Instance.OpenPanel<EndGamePanel>();
        if (enemyHero.health == 0)
        {
            endGamePanel.winOrLoseText.text = "你赢了";
            AudioManager.Instance.PlayBGM("GameWon");
        }
        else
        {
            endGamePanel.winOrLoseText.text = "你输了";
            AudioManager.Instance.PlayBGM("Lost");
        }
    }
    public Hero GetHero(Faction faction) => (faction == myHero.faction) ? myHero : enemyHero;
    public Hero GetOpponentHero(Faction faction) => (faction == myHero.faction) ? enemyHero : myHero;
    public Deck GetDeck(Faction faction) => (faction == myHero.faction) ? myDeck : enemyDeck;
    public HandCards GetHandCards(Faction faction) => (faction == myHero.faction) ? myHandCards : enemyHandCards;
    public void OnTurnStart()
    {
        OnTurnStartEvent?.Invoke();
    }
    public void OnMyHeroTurnStart()
    {
        OnMyHeroTurnStartEvent?.Invoke();
    }
    public void OnEnemyHeroTurnStart()
    {
        OnEnemyHeroTurnStartEvent?.Invoke();
    }
    public void OnEndTurn()
    {
        OnEndTurnEvent?.Invoke();
    }
    public void OnApplyCard(Card card)
    {
        OnApplyCardEvent?.Invoke(card);
    }
    public void OnMoveEntityComplete()
    {
        OnMoveEntityCompleteEvent?.Invoke();
    }
    [ServerRpc(RequireOwnership = false)]
    public void SetNameServerRpc(bool isServer, string name)
    {
        SetNameClientRpc(isServer, name);
    }
    [ClientRpc]
    public void SetNameClientRpc(bool isServer, string name)
    {
        if (isServer) hostName = name;
        else clientName = name;
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public DeckInfo deckInfo;
    static private bool decideTurnOrderComplete;
    static public Card selectedCard;
    static public Card SelectedCard
    {
        get => selectedCard;
        set
        {
            if (selectedCard != null)
            {
                if (selectedCard is EntityCard entityCard)
                {
                    Destroy(entityCard.curEntity);
                }
                selectedCard.CencelSelect();
            }
            selectedCard = value;
            if (selectedCard != null)
            {
                selectedCard.Select();
            }
        }
    }
    static public event Action OnTurnStartEvent;
    static public event Action OnMoveEntityCompleteEvent;
    static public event Action OnMyHeroTurnStartEvent;
    static public event Action OnEndTurnEvent;
    static public event Action OnEnemyHeroTurnStartEvent;
    static public event Action<Card> OnApplyCardEvent;
    static public event PlaceEntityHandler OnPlaceEntityEvent;
    public delegate void PlaceEntityHandler(Entity entity);
    static public void AddTurnStartEvent(Faction faction, Action action)
    {
        if (faction == myHero.faction)
        {
            OnMyHeroTurnStartEvent += action;
        }
        else
        {
            OnEnemyHeroTurnStartEvent += action;
        }
    }
    static public void RemoveTurnStartEvent(Faction faction, Action action)
    {
        if (faction == myHero.faction)
        {
            OnMyHeroTurnStartEvent -= action;
        }
        else
        {
            OnEnemyHeroTurnStartEvent -= action;
        }
    }
    static public int playingAnimationCounter;
    static public bool isReady;
    static public Faction tempFaction;
    static public string hostName;
    static public string clientName;
}
