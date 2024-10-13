using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using static GameManager;

public class SumoWrestler : Entity
{
    new void Start()
    {
        base.Start();
        if (slot != null)
        {
            BattlecryEvent += MoveEntity;
            if (!abilities.Any(ability => ability is Gravestone))
            {
                abilities.Add(new Gravestone(this));
            }
            AIApplicableColliders = ColliderManager.colliders.Where(kvp => kvp.Key / 100 == 1).Select(kvp => kvp.Value).ToList();
        }
    }
    public void MoveEntity()
    {
        bool hasEntity = false;
        foreach (Line line in lines)
        {
            if (!line.GetOpponentSlot(faction).Empty)
            {
                hasEntity = true;
                break;
            }
        }
        if (hasEntity)
        {
            Time.timeScale = 0;
            GameManager.OnMoveEntityCompleteEvent += ResponseTimeScale;
            if (faction == myHero.faction)
            {
                Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
                MessageBox.ShowMessage("移动一株植物", pos);
                OnUpdate += UpdateDetect;
            }
            else
            {
                if (gameMode == GameMode.Offline)
                {
                    Timer.Register(1f, () =>
                {
                    EnemyAI.Instance.Shuffle(AIApplicableColliders);
                    foreach (Collider2D collider in AIApplicableColliders)
                    {
                        Slot slot = collider.GetComponentInParent<Slot>();
                        if (slot.GetEntity(collider) != null)
                        {
                            selectedEntity = slot.GetEntity(collider);
                            break;
                        }
                    }
                    EnemyAI.Instance.Shuffle(AIApplicableColliders);
                    foreach (Collider2D collider in AIApplicableColliders)
                    {
                        if (IaAbleToMoveTo(collider))
                        {
                            GameManager.Instance.MoveEntity(selectedEntity, collider);
                            selectedEntity = null;
                            break;
                        }
                    }
                    BattlecryManager.Instance.isBattlecrying = false;
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
                if (collider != null && collider.CompareTag("Slot"))
                {
                    Slot slot = collider.GetComponentInParent<Slot>();
                    if (slot.faction == enemyHero.faction)
                    {
                        if (selectedEntity == null)
                        {
                            if (slot.GetEntity(collider) != null)
                            {
                                selectedEntity = slot.GetEntity(collider);
                                selectedEntity.spriteRenderer.color *= 0.5f;
                                Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
                                MessageBox.ShowMessage("选择目标位置", pos);
                            }
                        }
                        else
                        {
                            if (IaAbleToMoveTo(collider))
                            {
                                int posID = (selectedEntity.slot.FirstEntity == selectedEntity) ? 0 : 1;
                                if (gameMode == GameMode.Online)
                                {
                                    GameManager.Instance.MoveEntityServerRpc(enemyHero.faction, selectedEntity.slot.lineIndex, posID, ColliderManager.colliderID[collider]);
                                }
                                else
                                {
                                    GameManager.Instance.MoveEntity(selectedEntity, collider);
                                }
                                BattlecryManager.Instance.isBattlecrying = false;
                                OnUpdate -= UpdateDetect;
                                selectedEntity.spriteRenderer.color *= 2f;
                                selectedEntity = null;
                                MessageBox.HideMessage();
                            }
                        }
                    }
                }
            }
        }
    }
    private void ResponseTimeScale()
    {
        Time.timeScale = 1;
        GameManager.OnMoveEntityCompleteEvent -= ResponseTimeScale;
    }
    private bool IaAbleToMoveTo(Collider2D collider)
    {
        Slot targetSlot = collider.GetComponentInParent<Slot>();
        if (lines[targetSlot.lineIndex].terrain == Line.LineTerrain.Water && !selectedEntity.abilities.Any(ability => ability is Amphibious))
        {
            return false;
        }
        if (targetSlot == selectedEntity.slot)
        {
            return targetSlot.GetEntity(collider) != selectedEntity && targetSlot.GetEntity(collider) != null;
        }
        if (selectedEntity.abilities.Any(ability => ability is TeamUp))
        {
            if (targetSlot.Empty)
            {
                if (collider == targetSlot.firstCollider) return true;
                else return false;
            }
            else if (targetSlot.FirstEntity != null && targetSlot.SecondEntity != null) return false;
            else return true;
        }
        else
        {
            if (targetSlot.Empty)
            {
                if (collider == targetSlot.firstCollider) return true;
                else return false;
            }
            else if (targetSlot.FirstEntity != null && targetSlot.SecondEntity != null) return false;
            else if (targetSlot.FirstEntity != null)
            {
                if (targetSlot.FirstEntity.abilities.Any(ability => ability is TeamUp)) return true;
                else return false;
            }
            else
            {
                if (targetSlot.SecondEntity.abilities.Any(ability => ability is TeamUp)) return true;
                else return false;
            }
        }
    }
    public Entity selectedEntity;
    public List<Collider2D> AIApplicableColliders;
    public bool moveComplete;
}
