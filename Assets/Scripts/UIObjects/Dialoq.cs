using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Dialoq : UIWindow
{
    protected Animator animator;
    [Header("Please Drag the TextBox reference in editor")] public Text dialogText;
    [NotNull, Header("Animator trigger to fire when showing this dialog")] public string animatorTriggerName = "Notice";

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public virtual void TriggerDialog(string textForDialog)
    {
        Open();
        dialogText.text = textForDialog;
        if (animator != null)
            animator.SetTrigger(animatorTriggerName);
    }
}