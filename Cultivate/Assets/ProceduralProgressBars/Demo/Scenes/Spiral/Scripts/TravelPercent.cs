using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Renge.PPB.Demo {

    public class TravelPercent : MonoBehaviour {
        public GameObject traveler;
        public ProceduralProgressBar progressBar;
        public float offset = 0.1f;

        // Update is called once per frame
        void Update() {
            progressBar.Value = 1 - (traveler.transform.position.y - transform.position.y) / 5.0f + offset;
        }
    } }
