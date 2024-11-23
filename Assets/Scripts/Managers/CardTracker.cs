using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static Game;

public class CardTracker : Singleton<CardTracker>
{
    protected override void Awake()
    {
        base.Awake();
        historyActionList = new List<HistoryAction>();
    }
    public void Add<T>(T historyAction) where T : HistoryAction
    {
        historyActionList.Add(historyAction);
        if (cardTrackerPanel != null)
        {
            cardTrackerPanel.Add(historyAction);
        }
    }
    public List<HistoryAction> historyActionList;
    public CardTrackerPanel cardTrackerPanel;
    public class HistoryAction { }
    public class EntityAttackAction : HistoryAction
    {
        public EntityAttackAction(Entity attacker, Hero targetHero = null, params Entity[] targets)
        {
            attackerInfo = new AttackerInfo(attacker.ID, attacker.health, attacker.atk);
            if (targets != null && targets.Any())
            {
                targetInfos = targets.Select(target => new TargetInfo(target.ID, target.health, target.atk)).ToList();
            }
            if (targetHero != null)
            {
                targetHeroInfo = new TargetHeroInfo(targetHero.health, targetHero.faction);
            }
        }
        public struct AttackerInfo
        {
            public AttackerInfo(int entityID, int health, int atk)
            {
                this.entityID = entityID;
                this.health = health;
                this.atk = atk;
            }
            public Faction Faction => (entityID / 10000 == 1) ? Faction.Plant : Faction.Zombie;
            public int entityID;
            public int health;
            public int atk;
        }
        public struct TargetInfo
        {
            public TargetInfo(int entityID, int health, int atk)
            {
                this.entityID = entityID;
                this.health = health;
                this.atk = atk;
            }
            public int entityID;
            public int health;
            public int atk;
        }
        public class TargetHeroInfo
        {
            public TargetHeroInfo(int health, Faction faction)
            {
                this.health = health;
                this.faction = faction;
            }
            public int health;
            public Faction faction;
        }
        public AttackerInfo attackerInfo;
        public List<TargetInfo> targetInfos;
        public TargetHeroInfo targetHeroInfo;
    }
    public class CardApplyAction : HistoryAction
    {
        public enum TargetType
        {
            Position, Entity, Any
        }
        public CardApplyAction(Card card, Collider2D collider, TargetType targetType)
        {
            cardID = card.ID;
            colliderID = ColliderManager.colliderID[collider];
            this.targetType = targetType;
        }
        public Faction Faction => (cardID / 10000 == 1) ? Faction.Plant : Faction.Zombie;
        public int cardID;
        public int colliderID;
        public TargetType targetType;
    }
}
