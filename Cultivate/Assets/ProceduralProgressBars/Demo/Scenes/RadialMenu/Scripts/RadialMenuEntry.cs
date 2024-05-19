//Base Source: https://www.youtube.com/watch?v=tdkdRguH_dE

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Renge.PPB.Demo {

    public class RadialMenuEntry : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
        public delegate void RadialMenuEntryDelegate(RadialMenuEntry entry);

        [SerializeField] string label;
        [SerializeField] RawImage icon;

        RectTransform rectTransform;
        bool isHovering = false;

        public RadialMenuEntryDelegate Callback { get; set; }
        public Texture Icon { get => icon.texture; set => icon.texture = value; }
        public string Label { get => label; set => label = value; }

        private void Start() {
            rectTransform = icon.GetComponent<RectTransform>();
        }

        private void Update() {
            //this should best be replaced with a tweening library
            if (isHovering) {
                rectTransform.localScale = Vector2.Lerp(rectTransform.localScale, Vector2.one * 1.5f, 30f * Time.deltaTime);
            }
            else {
                rectTransform.localScale = Vector2.Lerp(rectTransform.localScale, Vector2.one, 30f * Time.deltaTime);
            }
        }

        public void OnPointerClick(PointerEventData eventData) {
            Callback?.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            isHovering = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            isHovering = false;
        }
    }

}