using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }


    [SerializeField] TMP_Text popUpTitle;
    [SerializeField] TMP_Text popUpContent;
    [SerializeField] RectTransform popUpContainer;
    [SerializeField] AnimationCurve popUpCurve;
    [SerializeField] float popUpSpeed;
    [SerializeField] float popUpDisplayTime = 8f;
    [Range(0, 100f)][SerializeField] float popUpHeightOffset = 0f;
    [SerializeField] RectTransform canvas;
    [SerializeField] TMP_Text floorText;
    Vector3 hidePosition;
    [SerializeField] GameObject CrossHair;
    [SerializeField] TMP_Text t_Interact;
    [SerializeField] TMP_Text t_Camera;
    [SerializeField] TMP_Text t_Sphere;
    [SerializeField] GameObject tutorialGroup;
    [SerializeField] RoomName_UI RN_UI;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        hidePosition = popUpContainer.position;
    }

    void Start()
    {
        GameManager.setTutorialEnabled(true);
    }

    public void ActivateInfoPopUp(string title, string content)
    {
        popUpContent.SetText(content);
        popUpTitle.SetText(title);
        StopAllCoroutines();
        StartCoroutine(MovePopUp());
    }


    IEnumerator MovePopUp()
    {
        popUpContainer.position = new Vector3(popUpContainer.position.x, 0f, popUpContainer.position.z);
        float curveTime = 0;
        bool isMoving = true;

        while (isMoving)
        {
            yield return 0; //Wait one frame to allow OnGUI to redraw the text container with the new content
            float newHeight = ((popUpContainer.sizeDelta.y * popUpContainer.localScale.y) * canvas.localScale.y) + popUpHeightOffset;

            while (Mathf.Abs(popUpContainer.position.y - newHeight) > 0.1f)
            {
                curveTime += Time.deltaTime;
                popUpContainer.position = Vector3.Lerp(popUpContainer.position, new Vector3(popUpContainer.position.x, newHeight, popUpContainer.position.z), popUpCurve.Evaluate(curveTime) * popUpSpeed);
                yield return null;
            }

            yield return new WaitForSeconds(popUpDisplayTime);

            while (popUpContainer.position.y > 0.1f)
            {
                curveTime += Time.deltaTime;
                popUpContainer.position = Vector3.Lerp(popUpContainer.position, new Vector3(popUpContainer.position.x, 0, popUpContainer.position.z), popUpCurve.Evaluate(curveTime) * popUpSpeed);
                yield return null;
            }
            isMoving = false;
        }


    }

    void FloorUI(int floorNum)
    {
        floorText.SetText("Current Floor: " + (floorNum + 1));
    }

    void OnEnable()
    {
        InputManager.onControlsChanged += setInputTutorials;
        GameManager.OnTutorialToggle += ToggleTutorial;
    }

    void OnDisable()
    {
        InputManager.onControlsChanged -= setInputTutorials;
        GameManager.OnTutorialToggle -= ToggleTutorial;
    }

    void setInputTutorials(PlayerInput input)
    {
        string currentControlScheme = input.currentControlScheme;
        if (currentControlScheme != "GamePad")
        {
            t_Sphere.SetText("Click the mouse wheel to see through walls");
            t_Camera.SetText("Right click moves the camera around, you can also zoom in with the scroll wheel");
            t_Interact.SetText("Left click on certain objects to bring up more info");
            CrossHair.SetActive(false);
        }
        else
        {
            t_Sphere.SetText("Use Left Trigger to see through walls");
            t_Camera.SetText("Left Stick moves the camera, while the Right Stick zooms in or out");
            t_Interact.SetText("Right Trigger while looking at certain items will bring up more info");
            CrossHair.SetActive(true);
        }



    }

    public void ToggleTutorial(bool isEnabled)
    {
        StopAllCoroutines();
        StartCoroutine(FadeUI(tutorialGroup));
    }

    public void RoomNameDisplay(string name)
    {
        if (RN_UI.GetCurrentName() != name)
        {
            RN_UI.Display(name);
        }
    }

    IEnumerator FadeUI(GameObject ui)
    {
        CanvasGroup cg = ui.GetComponent<CanvasGroup>();
        if (cg.alpha == 0)
        {
            ui.SetActive(true);
            while (cg.alpha != 1)
            {
                cg.alpha = Mathf.Lerp(cg.alpha, 1, Time.deltaTime * 0.8f);
                if (cg.alpha > 0.98f) cg.alpha = 1;
                yield return null;
            }
        }
        else if (cg.alpha != 0)
        {
            while (cg.alpha != 0)
            {
                cg.alpha = Mathf.Lerp(cg.alpha, 0, Time.deltaTime * 2f);
                if (cg.alpha < 0.02f) cg.alpha = 0;
                yield return null;
            }
            ui.SetActive(false);
        }

    }




}
