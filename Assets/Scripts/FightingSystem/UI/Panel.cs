using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    public void Show(bool animate = true)
    {
        OnShowPanel();
        if (animate)
            ShowAnimation();
        else
            gameObject.SetActive(true);
    }

    public void Hide(bool animate = true)
    {
        OnHidePanel();
        if (animate)
            HideAnimation();
        else
            gameObject.SetActive(false);
    }
    public virtual void OnShowPanel() {}
    public virtual void OnHidePanel() {}
    public virtual void ShowAnimation()
    {
        gameObject.SetActive(true);
    }

    public virtual void HideAnimation()
    {
        gameObject.SetActive(false);
    }
}
