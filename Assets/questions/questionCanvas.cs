using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class questionCanvas : MonoBehaviour
{
    internal bool canQuestion = false;
    internal bool playerInside = false;
    internal bool canEnableQuestions = true;
    internal bool canCloseQuestions = true;
    internal bool canReload = true;
    internal bool canLoad = true;
    internal bool canLoadScene = true;
    internal bool canRTP = true;
    internal int absAns;
    internal int right = 0;
    internal int questionIndex = 1;

    private float lastOpened;
    private bool canInteract = false;
    private bool inDialog = false;
    private bool canIntro = true;
    private bool closePressed = true;
    private bool canCongratulate = false;
    private bool inOption = false;
    private bool activeCanvas = false;
    private bool canPressAgain = false;
    private bool activeQuestion = false;
    private float initPos;
    private float initScale;

    [SerializeField] private int dialogueCount = 0;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private int sceneToLoad = 1;
    [SerializeField] private bool doesNotCongratulate = false;
    [SerializeField] private bool canContinue = false;

    [Header("Dialogues")]
    [SerializeField] private string character1;
    [SerializeField] private string character2;
    [SerializeField] private GameObject char1;
    [SerializeField] private GameObject char2;
    [SerializeField] private GameObject interactKey;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private bool canScene;
    [TextArea(1, 4)][SerializeField] private string[] dialogues;
    [SerializeField] private string[] emitter;
    [SerializeField] private bool[] optional;
    [SerializeField] private int startPos;
    [SerializeField] private int continuation;
    [SerializeField] private int congrats;

    [Header("Questionnaire")]
    [SerializeField] private TMP_Text rightQuestions;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject question;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject questionField;
    [SerializeField] private GameObject introBox;
    [SerializeField] private GameObject question1Box;
    [SerializeField] private GameObject question2Box;
    [SerializeField] private GameObject question3Box;
    [SerializeField] private TMP_Text introText;
    [SerializeField] private TMP_Text question1Text;
    [SerializeField] private TMP_Text question2Text;
    [SerializeField] private TMP_Text question3Text;


    [Header("Question 1")]
    [TextArea(3, 10)][SerializeField] private string questionIntroQ1;
    [TextArea(1, 3)][SerializeField] private string option1_Q1;
    [TextArea(1, 3)][SerializeField] private string option2_Q1;
    [TextArea(1, 3)][SerializeField] private string option3_Q1;
    enum rightAns_Q1 { Option1 = 0, Option2 = 1, Option3 = 2 }
    [SerializeField] rightAns_Q1 rightAnswer_Q1;

    [Header("Question 2")]
    [TextArea(3, 10)][SerializeField] private string questionIntroQ2;
    [TextArea(1, 3)][SerializeField] private string option1_Q2;
    [TextArea(1, 3)][SerializeField] private string option2_Q2;
    [TextArea(1, 3)][SerializeField] private string option3_Q2;
    enum rightAns_Q2 { Option1 = 0, Option2 = 1, Option3 = 2 }
    [SerializeField] rightAns_Q2 rightAnswer_Q2;

    [Header("Question 3")]
    [TextArea(3, 10)][SerializeField] private string questionIntroQ3;
    [TextArea(1, 3)][SerializeField] private string option1_Q3;
    [TextArea(1, 3)][SerializeField] private string option2_Q3;
    [TextArea(1, 3)][SerializeField] private string option3_Q3;
    enum rightAns_Q3 { Option1 = 0, Option2 = 1, Option3 = 2 }
    [SerializeField] rightAns_Q3 rightAnswer_Q3;

    private void Start()
    {
        playerMovement.canMove = true;
        LeanTween.init(8000);
        initPos = dialogText.rectTransform.anchoredPosition.y;
        initScale = dialogText.rectTransform.sizeDelta.y;
        blackScreen.SetActive(false);
        LeanTween.alpha(blackScreen.GetComponent<RectTransform>(), 0f, 0f);
        StartCoroutine(ReloadQuestions(questionIntroQ1, option1_Q1, option2_Q1, option3_Q1));
        options.gameObject.SetActive(false);
        canvas.SetActive(false);
        char1.SetActive(false);
        char2.SetActive(false);
        LeanTween.alpha(interactKey.GetComponent<RectTransform>(), 0f, 0f);
        LeanTween.alpha(question.GetComponent<RectTransform>(), 0f, 0f);
        LeanTween.alpha(questionField.GetComponent<RectTransform>(), 0f, 0f);
        LeanTween.alpha(introBox.GetComponent<RectTransform>(), 0f, 0f);
        LeanTween.alpha(question1Box.GetComponent<RectTransform>(), 0f, 0f);
        LeanTween.alpha(question2Box.GetComponent<RectTransform>(), 0f, 0f);
        LeanTween.alpha(question3Box.GetComponent<RectTransform>(), 0f, 0f);
    }
    private void FixedUpdate()
    {
        Debug.Log(canQuestion);

        if (Input.GetKey("f") && canInteract && playerInside)
        {
            dialogText.text = dialogues[dialogueCount];
            activeCanvas = true;
            canInteract = false;
            playerMovement.canMove = false;
            StartCoroutine(Interaction(true));
            Dialogues();
        }

        if (activeQuestion)
        {
            rightQuestions.text = right + "/3";
            question.SetActive(true);
            StartCoroutine(EnableQuestions());
            InitializeQuestions();
            if (canQuestion)
            {
                QuestionDynamics();
            }

            if (Input.GetKey("f") && !closePressed)
            {
                StartCoroutine(CloseQuestionnaire());
            }

            if (!Input.GetKey("f") && Time.time - lastOpened > 2.5f)
            {
                closePressed = false;
            }
        }
        else
        {
            question.SetActive(false);
        }

        if (activeCanvas)
        {
            canvas.SetActive(true);
            inDialog = true;
            if (inDialog)
            {
                if (Input.GetKey("return") && canIntro)
                {
                    if (dialogueCount == continuation - 1)
                    {
                        dialogueCount = startPos;
                        StopUI();
                    }
                    else if (dialogueCount == congrats - 1 && canContinue)
                    {
                        dialogueCount = continuation;
                        StopUI();
                    }
                    else if (dialogueCount == dialogues.Length - 1 && canCongratulate)
                    {
                        dialogueCount = continuation;
                        canCongratulate = false;
                        canContinue = true;
                        if (canScene)
                        {
                            StartCoroutine(LoadScene());
                        }
                        StopUI();
                    }
                    else if (dialogueCount == congrats - 1 && doesNotCongratulate)
                    {
                        dialogueCount = continuation;
                        canCongratulate = false;
                        doesNotCongratulate = false;
                        canContinue = true;
                        if (canScene)
                        {
                            StartCoroutine(LoadScene());
                        }
                        StopUI();
                    }
                    else if (dialogueCount == dialogues.Length - 1 && canContinue)
                    {
                        dialogueCount = continuation;
                        StopUI();
                    }
                    else
                    {
                        dialogueCount += 1;
                        Dialogues();
                    }
                }

                if (inOption)
                {
                    if (Input.GetKey("g") && canPressAgain)
                    {
                        dialogueCount += 2;
                        Dialogues();
                    }
                    else if (Input.GetKey("f") && canPressAgain)
                    {
                        dialogueCount += 1;
                        activeQuestion = true;
                        activeCanvas = false;
                    }
                }
            }
            if (Input.GetKey("escape"))
            {
                if (dialogueCount >= startPos && dialogueCount < continuation)
                {
                    dialogueCount = startPos;
                    StopUI();
                }
                else if (dialogueCount > startPos && dialogueCount <= congrats - 1 && canContinue)
                {
                    dialogueCount = continuation;
                    StopUI();
                }
                else if (dialogueCount > continuation && dialogueCount <= dialogues.Length - 1 && canCongratulate)
                {
                    dialogueCount = continuation;
                    canContinue = true;
                    canCongratulate = false;
                    StopUI();
                }
                else
                {
                    StopUI();
                }
            }
        }
        else
        {
            canvas.SetActive(false);
        }
    }
    private void StopUI()
    {
        inDialog = false;
        activeCanvas = false;
        playerMovement.canMove = true;
        StartCoroutine(Interaction(false));
        canPressAgain = false;
        canInteract = true;
    }

    private IEnumerator EnableQuestions()
    {
        if (canEnableQuestions)
        {
            canEnableQuestions = false;
            float customDelay = gi.uiOpeningTime + gi.uiOpeningDelay + gi.frameOpeningDelay;
            LeanTween.alpha(question.GetComponent<RectTransform>(), 1f, gi.uiOpeningTime).setDelay(gi.uiOpeningDelay);
            yield return new WaitForSeconds(customDelay);
            LeanTween.alpha(questionField.GetComponent<RectTransform>(), 1f, gi.frameOpeningTime).setDelay(customDelay);
            yield return new WaitForSeconds(gi.uiOpeningDelay + gi.uiOpeningTime + gi.frameOpeningTime + customDelay);
        }
    }

    private IEnumerator LoadScene()
    {
        if (canLoadScene)
        {
            canLoadScene = false;
            blackScreen.SetActive(true);
            LeanTween.alpha(blackScreen.GetComponent<RectTransform>(), 1f, gi.sceneChangerTime);
            yield return new WaitForSeconds(gi.sceneChangerTime);
            SceneManager.LoadScene(sceneToLoad);
            LeanTween.alpha(blackScreen.GetComponent<RectTransform>(), 0f, gi.sceneChangerTime);
            yield return new WaitForSeconds(gi.sceneChangerTime);
            blackScreen.SetActive(false);
        }
    }

    private IEnumerator CloseQuestionnaire()
    {
        closePressed = true;
        if (questionIndex > 3)
        {
            canCongratulate = true;
            dialogueCount = congrats;
            Dialogues();
            activeCanvas = true;
        }
        else
        {
            canCongratulate = false;
            dialogueCount = startPos;
            Dialogues();
            StopUI();
        }
        canRTP = false;
        canReload = false;
        canLoad = false;
        canQuestion = false;
        introBox.SetActive(false);
        question1Box.SetActive(false);
        question2Box.SetActive(false);
        question3Box.SetActive(false);
        introText.gameObject.SetActive(false);
        question1Text.gameObject.SetActive(false);
        question2Text.gameObject.SetActive(false);
        question3Text.gameObject.SetActive(false);
        LeanTween.alpha(interactKey.GetComponent<RectTransform>(), 0f, 0f).setDelay(0f);
        LeanTween.alpha(question.GetComponent<RectTransform>(), 0f, 0f).setDelay(0f);
        LeanTween.alpha(questionField.GetComponent<RectTransform>(), 0f, 0f).setDelay(0f);
        LeanTween.alpha(introBox.GetComponent<RectTransform>(), 0f, 0f);
        LeanTween.alpha(question1Box.GetComponent<RectTransform>(), 0f, 0f);
        LeanTween.alpha(question2Box.GetComponent<RectTransform>(), 0f, 0f);
        LeanTween.alpha(question3Box.GetComponent<RectTransform>(), 0f, 0f);
        yield return new WaitForSeconds(0.01f);
        activeQuestion = false;
        canCloseQuestions = true;
        canEnableQuestions = true;
        canRTP = true;
        canReload = true;
        canLoad = true;
    }
    private IEnumerator EndQuestionnaire()
    {
        if (canCloseQuestions) {
            canCloseQuestions = false;
            canRTP = false;
            canReload = false;
            canLoad = false;
            canQuestion = false;
            canCongratulate = true;
            activeCanvas = true;
            dialogueCount = congrats;
            Dialogues();
            introBox.SetActive(false);
            question1Box.SetActive(false);
            question2Box.SetActive(false);
            question3Box.SetActive(false);
            introText.gameObject.SetActive(false);
            question1Text.gameObject.SetActive(false);
            question2Text.gameObject.SetActive(false);
            question3Text.gameObject.SetActive(false);
            rightQuestions.gameObject.SetActive(false);
            float custDelay = gi.uiClosingTime + gi.uiClosingDelay + gi.frameClosingDelay;
            LeanTween.alpha(question.GetComponent<RectTransform>(), 0f, gi.uiClosingTime).setDelay(gi.uiOpeningDelay);
            yield return new WaitForSeconds(custDelay);
            LeanTween.alpha(questionField.GetComponent<RectTransform>(), 0f, gi.frameClosingTime).setDelay(custDelay);
            yield return new WaitForSeconds(gi.uiClosingDelay + gi.uiClosingTime + gi.frameClosingTime + custDelay);
            activeQuestion = false;
            canCloseQuestions = true;
            canEnableQuestions = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInside = true;
            StopUI();
            StartCoroutine(Alpha(true));
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (activeCanvas)
            {
                collider.GetComponent<Rigidbody2D>().velocity = new Vector2(0, collider.GetComponent<Rigidbody2D>().velocity.y);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInside = false;
            StartCoroutine(Alpha(false));
        }
    }

    private IEnumerator Alpha(bool inside)
    {
        if (inside)
        {
            LeanTween.alpha(interactKey.GetComponent<RectTransform>(), 1f, gi.keyOpeningTime).setDelay(gi.keyOpeningDelay);
        }
        
        if (!inside)
        {
            LeanTween.alpha(interactKey.GetComponent<RectTransform>(), 0f, gi.keyOpeningTime).setDelay(gi.keyOpeningDelay);
            canInteract = false;
        }

        yield return new WaitForSeconds(gi.keyOpeningTime + gi.keyOpeningDelay);
        if (inside) {
            canInteract = true;
        }
    }

    private IEnumerator Interaction(bool interacted)
    {
        if (interacted)
        {
            LeanTween.alpha(interactKey.GetComponent<RectTransform>(), 0f, gi.keyInteractionTime).setDelay(gi.keyInteractionDelay);
        }
        else if (!interacted)
        {
            LeanTween.alpha(interactKey.GetComponent<RectTransform>(), 1f, gi.keyInteractionTime).setDelay(gi.keyInteractionDelay);
        }

        yield return new WaitForSeconds(gi.keyInteractionTime + gi.keyInteractionDelay);

        interactKey.SetActive(!interacted);
    }

    private void Dialogues()
    {
        canIntro = false;
        if (dialogueCount < continuation)
        {
            StartCoroutine(DialogExecution());
        }
        else if (dialogueCount >= continuation && canContinue)
        {
            StartCoroutine(DialogExecution());
        }
        else if (dialogueCount >= congrats && canCongratulate)
        {
            StartCoroutine(DialogExecution());
        }
        else
        {
            StopUI();
        }
    }

    private IEnumerator DialogExecution()
    {
        dialogText.text = dialogues[dialogueCount];
        typewriterUI_v2 typewriter = dialogText.GetComponent<typewriterUI_v2>();
        typewriter.writer = dialogues[dialogueCount];
        dialogText.gameObject.SetActive(false);
        dialogText.gameObject.SetActive(true);

        if (emitter[dialogueCount] == character1)
        {
            char1.SetActive(true);
            char2.SetActive(false);
        }
        else if (emitter[dialogueCount] == character2)
        {
            char1.SetActive(false);
            char2.SetActive(true);
        }
        else
        {
            char1.SetActive(false);
            char2.SetActive(false);
        }
        if (optional[dialogueCount])
        {
            inOption = true;
            canIntro = false;
            dialogText.rectTransform.anchoredPosition = new Vector2(dialogText.rectTransform.anchoredPosition.x, 60);
            dialogText.rectTransform.sizeDelta = new Vector2(dialogText.rectTransform.sizeDelta.x, 120);
            yield return new WaitForSeconds(gi.optionOpeningDelay);
            canPressAgain = true;
            options.gameObject.SetActive(true);
        }
        else
        {
            inOption = false;
            dialogText.rectTransform.anchoredPosition = new Vector2(dialogText.rectTransform.anchoredPosition.x, initPos);
            dialogText.rectTransform.sizeDelta = new Vector2(dialogText.rectTransform.sizeDelta.x, initScale);
            options.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            canIntro = true;
        }
    }
    private void InitializeQuestions()
    {
        if (questionIndex == 1)
        {
            StartCoroutine(ReloadQuestions(questionIntroQ1, option1_Q1, option2_Q1, option3_Q1));
            absAns = (int)rightAnswer_Q1 + 1;
        }
        else if (questionIndex == 2)
        {
            StartCoroutine(ReloadQuestions(questionIntroQ2, option1_Q2, option2_Q2, option3_Q2));
            absAns = (int)rightAnswer_Q2 + 1;
        }
        else if (questionIndex == 3)
        {
            StartCoroutine(ReloadQuestions(questionIntroQ3, option1_Q3, option2_Q3, option3_Q3));
            absAns = (int)rightAnswer_Q3 + 1;
        }
        else
        {
            StartCoroutine(EndQuestionnaire());
        }
        StartCoroutine(LoadQuestions());
    }

    private IEnumerator LoadQuestions()
    {
        if (canLoad)
        {
            canLoad = false;
            lastOpened = Time.time;
            StartCoroutine(ReloadTypewriters());
            yield return new WaitForSeconds(1.0f);

            introText.gameObject.SetActive(true);
            introBox.SetActive(true);
            LeanTween.alpha(introBox.GetComponent<RectTransform>(), 1f, 0.5f);
            yield return new WaitForSeconds(gi.questionOpeningDelay);
            question1Text.gameObject.SetActive(true);
            question1Box.SetActive(true);
            LeanTween.alpha(question1Box.GetComponent<RectTransform>(), 1f, 0.5f);
            yield return new WaitForSeconds(gi.questionOpeningDelay);
            question2Text.gameObject.SetActive(true);
            question2Box.SetActive(true);
            LeanTween.alpha(question2Box.GetComponent<RectTransform>(), 1f, 0.5f);
            yield return new WaitForSeconds(gi.questionOpeningDelay);
            question3Text.gameObject.SetActive(true);
            question3Box.SetActive(true);
            LeanTween.alpha(question3Box.GetComponent<RectTransform>(), 1f, 0.5f);
            canQuestion = true;
        }
    }

    private void QuestionDynamics()
    {
        boxer q1box = question1Box.GetComponent<boxer>();
        boxer q2box = question2Box.GetComponent<boxer>();
        boxer q3box = question3Box.GetComponent<boxer>();
        if (canQuestion)
        {
            if (Input.GetKey("1") && !Input.GetKey("2") && !Input.GetKey("3"))
            {
                q1box.ans = 1;
                q1box.selected = true;
                q2box.selected = false;
                q3box.selected = false;
            } 
            
            if (!Input.GetKey("1") && Input.GetKey("2") && !Input.GetKey("3"))
            {
                q2box.ans = 2;
                q1box.selected = false;
                q2box.selected = true;
                q3box.selected = false;
            }
            
            if (!Input.GetKey("1") && !Input.GetKey("2") && Input.GetKey("3"))
            {
                q3box.ans = 3;
                q1box.selected = false;
                q2box.selected = false;
                q3box.selected = true;
            }
        }
    }

    private IEnumerator ReloadQuestions(string intro, string option1, string option2, string option3)
    {
        if (canReload)
        {
            introText.text = intro;
            question1Text.text = option1;
            question2Text.text = option2;
            question3Text.text = option3;

            introBox.SetActive(false);
            question1Box.SetActive(false);
            question2Box.SetActive(false);
            question3Box.SetActive(false);
            introText.gameObject.SetActive(false);
            question1Text.gameObject.SetActive(false);
            question2Text.gameObject.SetActive(false);
            question3Text.gameObject.SetActive(false);
            canReload = false;
            yield break;
        }
    }

    private IEnumerator ReloadTypewriters()
    {
        if (canRTP)
        {
            typewriterUI_v2 introTypewriter = introText.GetComponent<typewriterUI_v2>();
            typewriterUI_v2 q1Typewriter = question1Text.GetComponent<typewriterUI_v2>();
            typewriterUI_v2 q2Typewriter = question2Text.GetComponent<typewriterUI_v2>();
            typewriterUI_v2 q3Typewriter = question3Text.GetComponent<typewriterUI_v2>();

            introTypewriter.writer = introText.text;
            q1Typewriter.writer = question1Text.text;
            q2Typewriter.writer = question2Text.text;
            q3Typewriter.writer = question3Text.text;
            canRTP = false;
            yield break;
        }

    }
}