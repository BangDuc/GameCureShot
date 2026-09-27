using System.Collections.Generic;
using UnityEngine;

namespace Michsky.UI.Dark
{
    public class PanelTabManager : MonoBehaviour
    {
        [Header("PANEL LIST")]
        public List<GameObject> panels = new List<GameObject>();

        [Header("BUTTON LIST - OPTIONAL")]
        public List<GameObject> buttons = new List<GameObject>();

        [Header("SETTINGS")]
        public int currentPanelIndex;

        private const string PanelFadeIn = "Panel In";
        private const string PanelFadeOut = "Panel Out";
        private const string ButtonFadeIn = "Hover to Pressed";
        private const string ButtonFadeOut = "Pressed to Normal";

        private void Start()
        {
            if (!IsValidPanel(currentPanelIndex))
            {
                Debug.LogError(
                    $"{name}: Panel list hoặc Current Panel Index không hợp lệ.",
                    this
                );

                enabled = false;
                return;
            }

            OpenFirstTab();
        }

        public void OpenFirstTab()
        {
            PlayPanelAnimation(currentPanelIndex, PanelFadeIn);
            PlayButtonAnimation(currentPanelIndex, ButtonFadeIn);
        }

        public void PanelAnim(int newPanelIndex)
        {
            if (!IsValidPanel(newPanelIndex))
            {
                Debug.LogWarning(
                    $"{name}: Không tồn tại panel index {newPanelIndex}.",
                    this
                );

                return;
            }

            if (newPanelIndex == currentPanelIndex)
                return;

            int previousIndex = currentPanelIndex;

            PlayPanelAnimation(previousIndex, PanelFadeOut);
            PlayButtonAnimation(previousIndex, ButtonFadeOut);

            currentPanelIndex = newPanelIndex;

            PlayPanelAnimation(currentPanelIndex, PanelFadeIn);
            PlayButtonAnimation(currentPanelIndex, ButtonFadeIn);
        }

        public void NextPage()
        {
            PanelAnim(currentPanelIndex + 1);
        }

        public void PrevPage()
        {
            PanelAnim(currentPanelIndex - 1);
        }

        private bool IsValidPanel(int index)
        {
            return index >= 0 &&
                   index < panels.Count &&
                   panels[index] != null;
        }

        private void PlayPanelAnimation(
            int index,
            string animationName)
        {
            if (!IsValidPanel(index))
                return;

            Animator animator =
                panels[index].GetComponent<Animator>();

            if (animator != null)
                animator.Play(animationName);
        }

        private void PlayButtonAnimation(
            int index,
            string animationName)
        {
            // Không có button vẫn hoàn toàn hợp lệ.
            if (index < 0 ||
                index >= buttons.Count ||
                buttons[index] == null)
            {
                return;
            }

            Animator animator =
                buttons[index].GetComponent<Animator>();

            if (animator != null)
                animator.Play(animationName);
        }
    }
}
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Michsky.UI.Dark
//{
//    public class PanelTabManager : MonoBehaviour
//    {
//        [Header("PANEL LIST")]
//        public List<GameObject> panels = new List<GameObject>();

//        [Header("BUTTON LIST")]
//        public List<GameObject> buttons = new List<GameObject>();

//        private GameObject currentPanel;
//        private GameObject nextPanel;

//        private GameObject currentButton;
//        private GameObject nextButton;

//        [Header("SETTINGS")]
//        public int currentPanelIndex = 0;
//        private int currentButtonlIndex = 0;

//        private Animator currentPanelAnimator;
//        private Animator nextPanelAnimator;

//        private Animator currentButtonAnimator;
//        private Animator nextButtonAnimator;

//        string panelFadeIn = "Panel In";
//        string panelFadeOut = "Panel Out";
//        string buttonFadeIn = "Hover to Pressed";
//        string buttonFadeOut = "Pressed to Normal";

//        void Start()
//        {
//            currentButton = buttons[currentPanelIndex];
//            currentButtonAnimator = currentButton.GetComponent<Animator>();
//            currentButtonAnimator.Play(buttonFadeIn);

//            currentPanel = panels[currentPanelIndex];
//            currentPanelAnimator = currentPanel.GetComponent<Animator>();
//            currentPanelAnimator.Play(panelFadeIn);
//            Debug.Log("vua mo");
//        }

//        public void OpenFirstTab()
//        {
//            currentPanel = panels[currentPanelIndex];
//            currentPanelAnimator = currentPanel.GetComponent<Animator>();
//            currentPanelAnimator.Play(panelFadeIn);

//            currentButton = buttons[currentPanelIndex];
//            currentButtonAnimator = currentButton.GetComponent<Animator>();
//            currentButtonAnimator.Play(buttonFadeIn);
//            Debug.Log("mo tab dau");
//        }

//        public void PanelAnim(int newPanel)
//        {
//            if (newPanel != currentPanelIndex)
//            {
//                currentPanel = panels[currentPanelIndex];
//                currentPanelIndex = newPanel;
//                nextPanel = panels[currentPanelIndex];

//                currentPanelAnimator = currentPanel.GetComponent<Animator>();
//                nextPanelAnimator = nextPanel.GetComponent<Animator>();
//                currentPanelAnimator.Play(panelFadeOut);
//                nextPanelAnimator.Play(panelFadeIn);

//                currentButton = buttons[currentButtonlIndex];
//                currentButtonlIndex = newPanel;
//                nextButton = buttons[currentButtonlIndex];

//                currentButtonAnimator = currentButton.GetComponent<Animator>();
//                nextButtonAnimator = nextButton.GetComponent<Animator>();

//                currentButtonAnimator.Play(buttonFadeOut);
//                nextButtonAnimator.Play(buttonFadeIn);
//                Debug.Log("hoat anh panel");
//            }
//        }

//        public void NextPage()
//        {
//            if (currentPanelIndex <= panels.Count - 2)
//            {
//                currentPanel = panels[currentPanelIndex];
//                currentButton = buttons[currentButtonlIndex];
//                nextButton = buttons[currentButtonlIndex + 1];

//                currentPanelAnimator = currentPanel.GetComponent<Animator>();
//                currentButtonAnimator = currentButton.GetComponent<Animator>();
//                currentButtonAnimator.Play(buttonFadeOut);
//                currentPanelAnimator.Play(panelFadeOut);

//                currentPanelIndex += 1;
//                currentButtonlIndex += 1;
//                nextPanel = panels[currentPanelIndex];

//                nextPanelAnimator = nextPanel.GetComponent<Animator>();
//                nextButtonAnimator = nextButton.GetComponent<Animator>();
//                nextPanelAnimator.Play(panelFadeIn);
//                nextButtonAnimator.Play(buttonFadeIn);
//                Debug.Log("Nextpage: currentPanelIndex =" + currentPanelIndex + " currentButtonlIndex = " + currentButtonlIndex);
//            }
//        }

//        public void PrevPage()
//        {
//            if (currentPanelIndex >= 1)
//            {
//                currentPanel = panels[currentPanelIndex];
//                currentButton = buttons[currentButtonlIndex];
//                nextButton = buttons[currentButtonlIndex - 1];

//                currentPanelAnimator = currentPanel.GetComponent<Animator>();
//                currentButtonAnimator = currentButton.GetComponent<Animator>();
//                currentButtonAnimator.Play(buttonFadeOut);
//                currentPanelAnimator.Play(panelFadeOut);

//                currentPanelIndex -= 1;
//                currentButtonlIndex -= 1;
//                nextPanel = panels[currentPanelIndex];

//                nextPanelAnimator = nextPanel.GetComponent<Animator>();
//                nextButtonAnimator = nextButton.GetComponent<Animator>();
//                nextPanelAnimator.Play(panelFadeIn);
//                nextButtonAnimator.Play(buttonFadeIn);
//                Debug.Log("prevpage: currentPanelIndex =" + currentPanelIndex + " currentButtonlIndex = " + currentButtonlIndex);
//            }
//        }
//    }
//}