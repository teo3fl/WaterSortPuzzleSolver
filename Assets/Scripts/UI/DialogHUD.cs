using UnityEngine;

public class DialogHUD : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI txt_message;
    [SerializeField]
    private TMPro.TextMeshProUGUI txt_button;

    public delegate void ClickHandler();
    private ClickHandler onDialogButtonClicked;

    public void Display(string message, string buttonText)
    {
        txt_message.text = message;
        txt_button.text = buttonText;
        onDialogButtonClicked = null;

        SetActive(true);
    }

    public void Display(string message, string buttonText, ClickHandler handler)
    {
        Display(message, buttonText);
        onDialogButtonClicked += handler;
    }

    private void SetActive(bool isActive)
    {
        for(int i=0; i<transform.childCount;++i)
        {
            transform.GetChild(i).gameObject.SetActive(isActive);
        }
    }

    public void OnButtonClicked()
    {
        onDialogButtonClicked?.Invoke();
        SetActive(false);
    }
}
