using UnityEngine;
using UnityEngine.UI;
using static Game;

public class CardTrackerPanel : BasePanel
{
    new void Start()
    {
        base.Start();
        CardTracker.Instance.cardTrackerPanel = this;
        foreach (var historyAction in CardTracker.Instance.historyActionList)
        {
            Add(historyAction);
        }
    }
    public override void OpenPanel<T>()
    {
        base.OpenPanel<T>();
        scrollRect.verticalNormalizedPosition = 0;
    }
    public void Add<T>(T historyAction) where T : CardTracker.HistoryAction
    {
        if (historyAction is CardTracker.EntityAttackAction entityAttackAction)
        {
            var newItem = Instantiate(entityAttackActionItem, content).GetComponent<EntityAttackActionItem>();
            newItem.SetInfo(entityAttackAction);
        }
        else if (historyAction is CardTracker.CardApplyAction cardApplyAction)
        {
            var newItem = Instantiate(cardApplyActionItem, content).GetComponent<CardApplyActionItem>();
            newItem.SetInfo(cardApplyAction);
        }
        scrollRect.verticalNormalizedPosition = 0;
    }
    public GameObject entityAttackActionItem;
    public GameObject cardApplyActionItem;
    public Transform content;
    public ScrollRect scrollRect;
}