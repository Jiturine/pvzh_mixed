using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static Game;
using UnityEngine;
using UnityEngine.EventSystems;
using static GameManager;

public class SumoWrestler : Entity
{
    public override bool HasBattlecry => true;
    override public void Battlecry()
    {
        if (Game.GetOpponentEntities(faction).Any())
        {
            if (faction == myHero.faction)
            {
                Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
                var messageBoxPanel = UIManager.Instance.TryOpenPanel<MessageBoxPanel>();
                messageBoxPanel.ShowMessage("移动一株植物", pos);
                OnUpdate += UpdateDetect;
            }
            else
            {
                if (gameMode == GameMode.Offline)
                {
                    Timer.Register(1f, () =>
                {
                    AIApplicableColliders.Shuffle();
                    foreach (Collider2D collider in AIApplicableColliders)
                    {
                        Slot slot = collider.GetComponentInParent<Slot>();
                        if (slot.GetEntity(collider) != null)
                        {
                            selectedEntity = slot.GetEntity(collider);
                            break;
                        }
                    }
                    AIApplicableColliders.Shuffle();
                    foreach (Collider2D collider in AIApplicableColliders)
                    {
                        if (selectedEntity.IsAbleToMoveTo(collider))
                        {
                            ActionSequence.actionSequence.AddFirst(new MoveAction(selectedEntity, collider));
                            selectedEntity = null;
                            break;
                        }
                    }
                }, useRealTime: true);
                }
            }
        }
    }
    public void UpdateDetect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] hitColliders = Physics2D.OverlapPointAll(mousePosition);
            foreach (var collider in hitColliders)
            {
                if (collider != null && collider.CompareTag("Pos"))
                {
                    Position pos = collider.GetComponent<Position>();
                    if (pos.faction == enemyHero.faction)
                    {
                        if (selectedEntity == null)
                        {
                            if (pos.entity != null)
                            {
                                selectedEntity = pos.entity;
                                selectedEntity.spriteRenderer.color *= 0.5f;
                                Vector3 targetPos = Camera.main.WorldToScreenPoint(transform.position);
                                var messageBoxPanel = UIManager.Instance.TryOpenPanel<MessageBoxPanel>();
                                messageBoxPanel.ShowMessage("选择目标位置", targetPos);
                            }
                        }
                        else
                        {
                            if (selectedEntity.IsAbleToMoveTo(collider))
                            {
                                ActionSequence.actionSequence.AddFirst(new MoveAction(selectedEntity, collider));
                                OnUpdate -= UpdateDetect;
                                selectedEntity.spriteRenderer.color *= 2f;
                                selectedEntity = null;
                                UIManager.Instance.ClosePanel<MessageBoxPanel>();
                            }
                        }
                    }
                }
            }
        }
    }
    public Entity selectedEntity;
    static public List<Collider2D> AIApplicableColliders => ColliderManager.MyColliders;
    public bool moveComplete;
}
