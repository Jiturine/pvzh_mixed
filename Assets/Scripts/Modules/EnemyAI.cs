using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameManager;

public class EnemyAI : MonoBehaviour
{
    private static EnemyAI _instance;
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
    // Start is called before the first frame update
    void Start()
    {
        hasApplicableCard = true;
        hasReadyToAttackEntity = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (turnPhase == TurnPhase.EnemyTurn)
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
                    GameManager.Instance.EnemyHeroEndTurn();
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
                Shuffle(card.AIApplicableColliders);
                foreach (Collider2D collider in card.AIApplicableColliders)
                {
                    if (card.IsApplicableFor(collider))
                    {
                        card.ApplyFor(collider);
                        if (card is EntityCard)
                        {
                            GameManager.Instance.SwitchPhase();
                        }
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
            var entities = GetEnemyEntities();
            if (entities.Count == 0 && !hasApplicableCard)
            {
                decidingAction = false;
                cooldown = false;
                GameManager.Instance.EnemyHeroEndTurn();
            }
            else
            {
                Shuffle(entities);
                foreach (Entity entity in entities)
                {
                    if (entity.ReadyToAttack)
                    {
                        entity.Attack();
                        decidingAction = false;
                        cooldown = false;
                        GameManager.Instance.SwitchPhase();
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
    public void Shuffle<T>(List<T> list)
    {
        if (list.Count == 1) return;
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[j], list[i]) = (list[i], list[j]);
        }
    }
    public List<Entity> GetEnemyEntities()
    {
        List<Entity> entities = new List<Entity>();
        foreach (Line line in lines)
        {
            if (line.enemySlot.FirstEntity != null)
            {
                entities.Add(line.enemySlot.FirstEntity);
            }
            if (line.enemySlot.SecondEntity != null)
            {
                entities.Add(line.enemySlot.SecondEntity);
            }
        }
        return entities;
    }
}
