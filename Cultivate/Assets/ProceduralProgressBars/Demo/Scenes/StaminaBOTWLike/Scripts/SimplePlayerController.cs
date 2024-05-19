using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Renge.PPB.Demo {
    public class SimplePlayerController : MonoBehaviour {
        enum StaminaState {
            Regenerating, Depleting, StoppedSprinting, StoppedSprintingExhausted, Exhausted
        }
        enum PlayerState {
            Walking, Running, SlowWalking
        }
        Camera cam;
        public GameObject staminaAnchor;
        SimpleCompoundPB staminaController;
        public GameObject cameraTarget;
        public float cameraDistance = 10;
        public float slowWalkSpeed = 3;
        public float walkSpeed = 5;
        public float runSpeed = 10;
        public float staminaDrainPerSecond = 0.1f;
        public float staminaGainPerSecond = 0.5f;
        public float staminaGainPerSecondExhausted = 1f;
        public float cooldownDelay = 0.5f;

        float currentSpeed = 5;
        float currentStamina = 1;
        StaminaState staminaState = StaminaState.Regenerating;
        PlayerState playerState = PlayerState.Walking;

        float cooldownTimer = 0;

        private void Start() {
            cam = Camera.main;
            staminaController = staminaAnchor.GetComponent<SimpleCompoundPB>();
        }

        private void Update() {
            cam.transform.localPosition = new Vector3(0, 2, -cameraDistance);
            cam.transform.LookAt(cameraTarget.transform);
            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * 5, 0));
            transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime * currentSpeed);

            switch (staminaState) {
                case StaminaState.Regenerating:
                    if (Input.GetKey(KeyCode.LeftShift)) {
                        staminaState = StaminaState.Depleting;
                        playerState = PlayerState.Running;
                        staminaAnchor.SetActive(true);
                        break;
                    }
                    if (currentStamina >= 1) {
                        currentStamina = 1;
                        if (staminaAnchor.activeSelf)
                            staminaAnchor.SetActive(false);
                    } else {
                        currentStamina += staminaGainPerSecond * Time.deltaTime;
                    }
                    break;
                case StaminaState.Depleting:
                    currentStamina -= staminaDrainPerSecond * Time.deltaTime;
                    if (!Input.GetKey(KeyCode.LeftShift)) {
                        cooldownTimer = 0;
                        staminaState = StaminaState.StoppedSprinting;
                        playerState = PlayerState.Walking;
                    } else if (currentStamina <= 0) {
                        cooldownTimer = 0;
                        staminaState = StaminaState.StoppedSprintingExhausted;
                        playerState = PlayerState.SlowWalking;
                    }
                    break;

                case StaminaState.StoppedSprinting:
                    cooldownTimer += Time.deltaTime;
                    if (cooldownTimer >= cooldownDelay) {
                        staminaState = StaminaState.Regenerating;
                    } else if (Input.GetKey(KeyCode.LeftShift)) {
                        staminaState = StaminaState.Depleting;
                        playerState = PlayerState.Running;
                    }
                    break;
                case StaminaState.StoppedSprintingExhausted:
                    cooldownTimer += Time.deltaTime;
                    if (cooldownTimer >= cooldownDelay) {
                        staminaState = StaminaState.Exhausted;
                    }
                    break;
                case StaminaState.Exhausted:
                    currentStamina += staminaGainPerSecondExhausted * Time.deltaTime;
                    if (currentStamina >= 1) {
                        staminaState = StaminaState.Regenerating;
                        playerState = PlayerState.Walking;
                    }
                    break;
            }

            switch (playerState) {
                case PlayerState.Walking:
                    currentSpeed = walkSpeed;
                    break;
                case PlayerState.Running:
                    currentSpeed = runSpeed;
                    break;
                case PlayerState.SlowWalking:
                    currentSpeed = slowWalkSpeed;
                    break;
            }

            staminaController.SetValue(currentStamina);
        }
    }
}