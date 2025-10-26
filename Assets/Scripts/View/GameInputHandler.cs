using UnityEngine;
using UnityEngine.UI;
using ColorSorter.GameSystem;
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
            if (blueButton)
                blueButton.onClick.AddListener(() => controller.HandleInput(ColorType.Blue));

            if (redButton)
                redButton.onClick.AddListener(() => controller.HandleInput(ColorType.Red));

            if (restartButton)
                restartButton.onClick.AddListener(() => controller.RestartGame());
        }


        // TODO: 키보드 입력 시 작성
        void Update()
        {
            if (controller == null)
                return;
        }

        public void SetEnabled(bool on)
        {
            if (blueButton) blueButton.interactable = on;
            if (redButton) redButton.interactable = on;
            if (restartButton) restartButton.interactable = on;
        }
    }
}