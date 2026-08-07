using UnityEngine;
using UnityEngine.UI;
using ColorSorter.Game;
using ColorSorter.Controller;

namespace ColorSorter.View
{
    public class GameInputHandler : MonoBehaviour
    {
        [Header("Controller")]
        [SerializeField] GameController controller;

        [Header("Game Buttons")]
        [SerializeField] private Button blueButton;
        [SerializeField] private Button redButton;

        [Header("GameOver Buttons")]
        [SerializeField] private Button restartButton;

        void Awake()
        {
            if (controller == null)
                Debug.LogError($"{nameof(GameInputHandler)}: {nameof(controller)} is not assigned.", this);

            if (blueButton && controller)
                blueButton.onClick.AddListener(() => controller.HandleInput(ColorType.Blue));

            if (redButton && controller)
                redButton.onClick.AddListener(() => controller.HandleInput(ColorType.Red));

            if (restartButton && controller)
                restartButton.onClick.AddListener(() => controller.RestartGame());
        }

        public void SetEnabled(bool on)
        {
            if (blueButton) blueButton.interactable = on;
            if (redButton) redButton.interactable = on;
            if (restartButton) restartButton.interactable = on;
        }
    }
}