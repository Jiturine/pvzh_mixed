using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static Game;
using UnityEngine;
using static GameManager;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI Instance
    {
        get; set;
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        hasApplicableCard = true;
        hasReadyToAttackEntity = true;
    }
    void Update()
    {
        if (Game.State is EnemyTurnState)
        {
            if (!cooldown)
            {
                cooldown = true;
                Timer.Register(2f, () =>
            {
                decidingAction = true;
            });
            }
            if (decidingAction)
            {
                TryTakeAction();
                if (!hasApplicableCard && !hasReadyToAttackEntity)
                {
                    decidingAction = false;
                    cooldown = false;
                    ActionSequence.AddAction(new EndTurnAction(enemyHero.faction));
                }
            }
        }
    }

    public void TryTakeAction()
    {
        if (Random.value > 0.5f && hasApplicableCard)
        {
            foreach (Card card in enemyHandCards.cardList)
            {
                card.AIApplicableColliders.Shuffle();
                foreach (Collider2D collider in card.AIApplicableColliders)
                {
                    if (card.IsApplicableFor(collider))
                    {
                        ActionSequence.AddAction(new ApplyCardAction(card, collider));
                        decidingAction = false;
                        cooldown = false;
                        break;
                    }
                }
                if (!decidingAction) break;
            }
            if (decidingAction)
            {
                hasApplicableCard = false;
            }
        }
        else if (hasReadyToAttackEntity)
        {
            var entities = Game.GetEntities(enemyHero.faction);
            if (entities.Count == 0 && !hasApplicableCard)
            {
                decidingAction = false;
                cooldown = false;
                GameManager.Instance.EndTurn(enemyHero.faction);
            }
            else
            {
                entities.Shuffle();
                foreach (Entity entity in entities)
                {
                    if (entity.ReadyToAttack)
                    {
                        ActionSequence.AddAction(new AttackAction(entity));
                        decidingAction = false;
                        cooldown = false;
                        break;
                    }
                }
                if (decidingAction)
                {
                    hasReadyToAttackEntity = false;
                }
            }
        }
    }
    public bool cooldown;
    public bool decidingAction;
    public bool hasApplicableCard;
    public bool hasReadyToAttackEntity;
}
