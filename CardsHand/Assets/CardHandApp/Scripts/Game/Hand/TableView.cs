using CardsHand.Card;
using CardsHand.View;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CardsHand.Hand
{
    public class TableView : BaseUIView, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;

            var d = eventData.pointerDrag.GetComponent<ICardView>();

            if (d != null)
                d.DropOnTable(transform as RectTransform);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;

            var d = eventData.pointerDrag.GetComponent<ICardView>();

            if (d != null)
                d.DropOnTable(transform as RectTransform);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;

            var d = eventData.pointerDrag.GetComponent<ICardView>();

            if (d != null)
                d.DropOnTable(null);
        }
    }
}