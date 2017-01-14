using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIWindow : BroadcasterAndReceiver
{
    [Header("Use for SFX etc")] public UnityEvent onOpen;
    [Header("Use for SFX etc")] public UnityEvent onClose;
    [Header("Optional textbox for name of Window")] public Text windowNameTextBox;
    [SerializeField] [Header("Write the default Name of the Window here")] private string _windowName = "New Dialog";

    /// <summary>
    /// use this property to change the display name of the window from code
    /// </summary>
    public string WindowName
    {
        get { return _windowName; }
        set
        {
            _windowName = value;
            if (windowNameTextBox != null)
                windowNameTextBox.text = value;
        }
    }

    protected override void Start()
    {
        base.Start();
        if (windowNameTextBox != null)
            windowNameTextBox.text = _windowName;
    }

    public virtual void Open()
    {
        onOpen.Invoke();
    }

    public virtual void Close()
    {
        onClose.Invoke();
    }
}