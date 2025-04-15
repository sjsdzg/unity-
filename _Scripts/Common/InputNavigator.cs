using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputNavigator : MonoBehaviour
{
    public void Update()
    {
        Selectable next = null;

        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            }
            else
            {
                next = EventSystem.current.firstSelectedGameObject.GetComponent<Selectable>();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            }
            else
            {
                next = EventSystem.current.firstSelectedGameObject.GetComponent<Selectable>();
            }
        }

        selectGameObject(next);
    }

    private void selectGameObject(Selectable selectable)
    {
        if (selectable != null)
        {
            InputField inputfield = selectable.GetComponent<InputField>();
            if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(EventSystem.current));  //if it's an input field, also set the text caret

            EventSystem.current.SetSelectedGameObject(selectable.gameObject, new BaseEventData(EventSystem.current));
        }
    }
}
