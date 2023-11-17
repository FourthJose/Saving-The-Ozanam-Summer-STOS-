using System.Collections;
using UnityEngine;

public class boxer : MonoBehaviour
{
    public GameObject teacher;
    internal bool selected = false;
    internal bool toggled = false;
    internal bool canDecide = true;
    internal bool trueAns = false;
    internal int ans;
    [SerializeField] private GameObject selection;
    [SerializeField] private Animator animator;
    private bool hitAgain;

    void Start()
    {
        hitAgain = false;
        selection.SetActive(false);
        animator.Play("selected");
    }
    void Update()
    {
        if (selected)
        {
            if (!Input.GetKey(ans.ToString()))
            {
                hitAgain = true;
            }
            selection.SetActive(true);
            questionCanvas qCanv = teacher.GetComponent<questionCanvas>();
            if (Input.GetKey(ans.ToString()) && hitAgain)
            {
                hitAgain = false;
                trueAns = true;
            }

            if (trueAns)
            {
                qCanv.canQuestion = false;
                qCanv.canLoad = false;
                if (ans == qCanv.absAns)
                {
                    StartCoroutine(Right(true));
                }
                else
                {
                    StartCoroutine(Right(false));
                }
            }
        }
        else
        {
            hitAgain = false;
            selection.SetActive(false);
        }
    }

    private IEnumerator Right(bool args)
    {
        if (canDecide)
        {
            canDecide = false;
            questionCanvas qCanv = teacher.GetComponent<questionCanvas>();
            qCanv.right += (args) ? 1 : 0;
            animator.Play((args) ? "right" : "wrong");
            yield return new WaitForSeconds(3.0f);
            selection.SetActive(false);
            animator.Play("selected");
            qCanv.questionIndex += 1;
            qCanv.canReload = true;
            qCanv.canLoad = true;
            qCanv.canRTP = true;
            selected = false;
            canDecide = true;
            trueAns = false;
        }
    }
}