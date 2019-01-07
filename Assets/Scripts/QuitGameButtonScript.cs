using UnityEngine;
using UnityEngine.EventSystems;

public class QuitGameButtonScript : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        print("Quitting game");
        Application.Quit();
    }
}