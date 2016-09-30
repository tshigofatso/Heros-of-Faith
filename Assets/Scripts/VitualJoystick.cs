using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VitualJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{

    private static VitualJoystick instance;

    public static VitualJoystick Instance
    {
        get
        {
            return instance;
        }


    }

    private Image backgroundImage;
    private Image JoyStickImage;
    private Vector3 InputVector;

    private void Start() {
        backgroundImage = GetComponent<Image>();
        JoyStickImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(backgroundImage.rectTransform,ped.position,ped.pressEventCamera,out pos))
        {
            pos.x = (pos.x / backgroundImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / backgroundImage.rectTransform.sizeDelta.y);

            InputVector = new Vector3(pos.x *2,0,pos.y * 2 -1);
            InputVector = (InputVector.magnitude > 1.0f)? InputVector.normalized : InputVector;

            JoyStickImage.rectTransform.anchoredPosition = new Vector3(InputVector.x * (backgroundImage.rectTransform.sizeDelta.x / 2), InputVector.z * (backgroundImage.rectTransform.sizeDelta.y / 2));

            HandleMovement();
        }
    }

    public void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public void OnPointerUp(PointerEventData ped)
    {
        InputVector = Vector3.zero;
        JoyStickImage.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float Horizontal()
    {
        if (InputVector.x != 0)
        {
            return InputVector.x;
        }
        else
        {
            return Input.GetAxis("Horizontal");
        }
    }

    public float Vertical()
    {
        if (InputVector.z != 0)
        {
            return InputVector.z;
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }


    public void HandleMovement()
    {
        if ( Player.Instance.MyRigidBody.velocity.y < 0)
        {
            Player.Instance.MyAnimator.SetBool("land", true);
        }

        if (!Player.Instance.Attack && !Player.Instance.Slide && (Player.Instance.OnGround || Player.Instance.aircontrol))
        {
            Player.Instance.MyRigidBody.velocity = new Vector2(Horizontal() * 10.0f, Player.Instance.MyRigidBody.velocity.y);
        }
        if (Player.Instance.Jump && Player.Instance.MyRigidBody.velocity.y == 0)
        {
            Player.Instance.MyRigidBody.AddForce(new Vector2(0, Player.Instance.jumpForce));
        }

        Player.Instance.MyAnimator.SetFloat("speed", Mathf.Abs(Horizontal()));

    }
}
