using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static Game;
using UnityEngine;
using UnityEngine.EventSystems;
using static GameManager;
using System.Security.Cryptography;

public class SumoWrestler : Entity
{
    public override bool HasBattlecry => true;
    override public void Battlecry()
    {
        if (Game.GetOpponentEntities(faction).Any())
        {
            if (gameMode == GameMode.Online)
            {
                ActionSequence.Lock();
            }
            if (faction == myHero.faction)
            {
                Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
                var messageBoxPanel = UIManager.Instance.TryOpenPanel<MessageBoxPanel>();
                messageBoxPanel.ShowMessage("移动一株植物", pos);
                foreach (var position in Game.GetOpponentPositions(faction))
                {
                    if (position.entity != null)
                    {
                        position.entity.ShowApplicableEntity();
                    }
                }
                OnUpdate += MyUpdateDetect;
            }
            else if (gameMode == GameMode.Offline) //AI逻辑
            {
                Timer.Register(1f, () =>
            {
                AIApplicableColliders.Shuffle();
                foreach (Collider2D collider in AIApplicableColliders)
                {
                    Position pos = collider.GetComponent<Position>();
                    if (pos.entity != null)
                    {
                        selectedEntity = pos.entity;
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
            });
            }
        }
    }
    public void MyUpdateDetect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Input.mousePosition.TranslateScreenToWorld();
            Collider2D[] hitColliders = Physics2D.OverlapPointAll(mousePosition);
            if (hitColliders == null) return;
            foreach (var collider in hitColliders)
            {
                if (collider.CompareTag("Pos"))
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
                                foreach (var position in Game.GetOpponentPositions(faction))
                                {
                                    if (position.entity != null)
                                    {
                                        position.entity.HideApplicableEntity();
                                    }
                                    if (selectedEntity.IsAbleToMoveTo(position.collider))
                                    {
                                        position.ShowApplicablePositon();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (selectedEntity.IsAbleToMoveTo(collider))
                            {
                                ActionSequence.AddInstantAction(new MoveAction(selectedEntity, collider));
                                OnUpdate -= MyUpdateDetect;
                                foreach (var position in Game.GetOpponentPositions(faction))
                                {
                                    position.HideApplicablePosition();
                                }
                                selectedEntity.spriteRenderer.color *= 2f;
                                selectedEntity = null;
                                UIManager.Instance.ClosePanel<MessageBoxPanel>();
                                GameManager.Instance.ActionSequenceUnlockServerRpc();
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
