//Base Source: https://www.youtube.com/watch?v=tdkdRguH_dE

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Renge.PPB.Demo {

    public class RadialMenu : MonoBehaviour {
        [SerializeField] float radius;
        [SerializeField] GameObject entryPrefab;
        [SerializeField] RawImage targetIcon;
        [SerializeField] TextMeshProUGUI targetText;
        [SerializeField] GameObject contentContainer;
        [SerializeField] List<Texture> icons;
        [SerializeField] ProceduralProgressBar bgPB;
        [SerializeField] ProceduralProgressBar selectorPB;
        [Space]
        [SerializeField] int entryCount = 10;
        [SerializeField] float animDelay = 0.01f;
        [SerializeField] float animSpeedOpen = 8f;
        [SerializeField] float animSpeedClose = 20f;

        bool isOpen = false;
        bool rotationTargetSet = false;
        float rotationTarget = 0f;

        List<RadialMenuEntry> entries;

        List<(RectTransform rectTransform, Vector3 targetPos, float scaleTarget, float delay, float animSpeed, Action completeAction)> lerpTargets;

        private void Start() {
            lerpTargets = new List<(RectTransform, Vector3, float, float, float, Action)>();
            entries = new List<RadialMenuEntry>();
        }

        //This should best be replaced with a tweening library
        private void Update() {
            for (int i = 0; i < lerpTargets.Count; i++) {
                var target = lerpTargets[i];
                bool redundant = false;
                foreach (var item in lerpTargets) {
                    if (item != target && item.rectTransform == target.rectTransform) {
                        lerpTargets.RemoveAt(i--);
                        redundant = true;
                        break;
                    }
                }
                if (redundant) {
                    continue;
                }
                if(target.delay > 0f) {
                    target.delay -= Time.deltaTime;
                    lerpTargets[i] = target;
                }
                else {
                    if (Vector3.Distance(target.rectTransform.anchoredPosition, target.targetPos) < 1f) {
                        target.completeAction?.Invoke();
                        lerpTargets.RemoveAt(i--);
                    } else {
                        target.rectTransform.anchoredPosition = Vector3.Lerp(target.rectTransform.anchoredPosition, target.targetPos, target.animSpeed * Time.deltaTime);
                        target.rectTransform.localScale = Vector3.Lerp(target.rectTransform.localScale, new Vector3(target.scaleTarget, target.scaleTarget, target.scaleTarget), target.animSpeed * Time.deltaTime);
                        lerpTargets[i] = target;
                    }
                }
            }
            if (isOpen) {
                bgPB.Radius = Mathf.Lerp(bgPB.Radius, radius, animSpeedOpen * Time.deltaTime);
                bgPB.VariableWidthCurve.MoveKey(0, new Keyframe(0, Mathf.Lerp(bgPB.VariableWidthCurve.Evaluate(0), 1.0f, animSpeedOpen * Time.deltaTime)));
                bgPB.VariableWidthCurve.MoveKey(1, new Keyframe(1, Mathf.Lerp(bgPB.VariableWidthCurve.Evaluate(1), 1.0f, (animSpeedOpen - 2) * Time.deltaTime)));
                bgPB.UpdateVariableWidthCurve();

                selectorPB.Radius = Mathf.Lerp(selectorPB.Radius, radius, animSpeedOpen * Time.deltaTime);
                if(rotationTargetSet)
                    selectorPB.transform.rotation = Quaternion.Lerp(selectorPB.transform.rotation, Quaternion.Euler(0, 0, rotationTarget), animSpeedOpen * Time.deltaTime);
            } else {
                //bgPB.Radius = Mathf.Lerp(bgPB.Radius, 0, animSpeedClose * Time.deltaTime);
                //bgPB.VariableWidthCurve.MoveKey(0, new Keyframe(0, Mathf.Lerp(bgPB.VariableWidthCurve.Evaluate(0), 0.0f, animSpeedClose * Time.deltaTime)));
                //bgPB.VariableWidthCurve.MoveKey(1, new Keyframe(1, Mathf.Lerp(bgPB.VariableWidthCurve.Evaluate(1), 0.0f, animSpeedClose * Time.deltaTime)));
                //selectorPB.Radius = Mathf.Lerp(selectorPB.Radius, 0, animSpeedOpen * Time.deltaTime);
                //bgPB.UpdateVariableWidthCurve();
            }
        }

        void AddEntry(string label, Texture icon, RadialMenuEntry.RadialMenuEntryDelegate callback) {
            GameObject entry = Instantiate(entryPrefab, contentContainer.transform);
            RadialMenuEntry radialMenuEntry = entry.GetComponent<RadialMenuEntry>();
            radialMenuEntry.Label = label;
            radialMenuEntry.Icon = icon;
            radialMenuEntry.Callback = callback;

            entries.Add(radialMenuEntry);
        }

        public void Open() {
            isOpen = true;
            bgPB.gameObject.SetActive(true);
            selectorPB.gameObject.SetActive(true);
            targetIcon.gameObject.SetActive(true);
            targetText.gameObject.SetActive(true);
            for (int i = 0; i < entryCount; i++) {
                AddEntry($"Action {i+1}", icons[i % 6], (e) => { SetTargetIcon(e); SetSelectionTarget(e); });
            }
            Invoke(nameof(SetInitialTarget), 0.1f);
            Rearrange();

            bgPB.SegmentCount = entryCount;
            bgPB.Radius = 0f;
            bgPB.VariableWidthCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));
            selectorPB.SegmentSpacing = 1f - 1f / entryCount + 0.05f / entryCount;
            selectorPB.Radius = 0;
        }

        public void Close() {
            isOpen = false;
            bgPB.gameObject.SetActive(false);
            selectorPB.gameObject.SetActive(false);
            targetIcon.gameObject.SetActive(false);
            targetText.gameObject.SetActive(false);
            for (int i = 0; i < entries.Count; i++) {
                var rect = entries[i].GetComponent<RectTransform>();
                GameObject go = entries[i].gameObject;
                //This should best be replaced with a tweening library
                lerpTargets.Add((rect, Vector3.zero, 0, 0, animSpeedClose, () => { Destroy(go); } ));
            }
            entries.Clear();
        }

        public void Toggle() {
            if (entries.Count > 0) {
                Close();
            } else {
                Open();
            }
        }

        public void Rearrange() {
            float radiansPerEntry = 2 * Mathf.PI / entries.Count;
            for (int i = 0; i < entries.Count; i++) {
                float x = Mathf.Sin(radiansPerEntry * i + radiansPerEntry/2) * radius;
                float y = Mathf.Cos(radiansPerEntry * i + radiansPerEntry/2) * radius;

                var rect = entries[i].GetComponent<RectTransform>();

                rect.localScale = Vector2.zero;
                rect.anchoredPosition = new Vector2(0, 0);
                //This should best be replaced with a tweening library
                lerpTargets.Add((rect, new Vector3(x, y, 0f), 1f, animDelay * i, animSpeedOpen, null));
            }
        }

        void SetInitialTarget() {
            SetTargetIcon(entries[0]);
            SetSelectionTarget(entries[0]);
        }

        void SetTargetIcon(RadialMenuEntry entry) {
            targetIcon.texture = entry.Icon;
            targetText.text = entry.Label;
        }
        void SetSelectionTarget(RadialMenuEntry entry) {
            rotationTargetSet = true;
            rotationTarget = GetAngle(entry.transform.position);
        }

        float GetAngle(Vector2 pos) {
            return Mathf.Atan2(pos.y - transform.position.y, pos.x - transform.position.x) * Mathf.Rad2Deg - 90;
        }
    }
}