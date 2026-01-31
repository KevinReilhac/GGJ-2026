using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    public void Show(bool animate = true)
    {
        BeforeHidePanel();
        gameObject.SetActive(true);
        AfterShowPanel();
    }

    public void Hide(bool animate = true)
    {
        BeforeHidePanel();
        gameObject.SetActive(false);
        AfterHidePanel();
    }
    public virtual void AfterShowPanel() {}
    public virtual void AfterHidePanel() {}
    public virtual void BeforeShowPanel() {}
    public virtual void BeforeHidePanel() {}
}
