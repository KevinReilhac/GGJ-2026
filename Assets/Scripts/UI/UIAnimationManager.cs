using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class UIAnimationManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI playText;
    [SerializeField] private TextMeshProUGUI quitText;
    
    // Start is called before the first frame update
    void Start()
    {
        playText.text = "Jouer";
        quitText.text = "Quitter";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer entered: ");
        if (eventData.pointerEnter.name == "PlayButton")
        {
            playText.text = "> Jouer";
        }
        else if (eventData.pointerEnter.name == "QuitButton")
        {
            quitText.text = "> Quitter";
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer exited: ");
        if (eventData.pointerEnter.name == "PlayButton")
        {
            playText.text = "Jouer";
        }
        else if (eventData.pointerEnter.name == "QuitButton")
        {
            quitText.text = "Quitter";
        }
    }

}