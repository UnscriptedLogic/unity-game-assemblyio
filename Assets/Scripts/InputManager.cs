using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //Touch Input
    private TouchInput touchInput;
    private TouchInput.TouchControlsActions touchControls;

    //Dynamic Values
    private bool primaryTapContact;
    private bool secondaryTapContact;
    private bool isOverUI;
    private Vector2 primaryTapPosition;
    private Vector2 secondaryTapPosition;
    private Vector2 primaryHoldDelta;
    private Vector2 secondaryHoldDelta;

    //Events
    public delegate void StartPrimaryTapContact();
    public delegate void StartPrimaryTapPosition(Vector2 position);
    public delegate void StartPrimaryHoldDelta(Vector2 delta);
    public delegate void StartSecondaryTapContact();
    public delegate void StartSecondaryTapPosition(Vector2 position);
    public delegate void StartSecondaryHoldDelta(Vector2 delta);

    public delegate void EndPrimaryTapContact();
    public delegate void EndPrimaryTapPosition(Vector2 position);
    public delegate void EndPrimaryHoldDelta(Vector2 delta);
    public delegate void EndSecondaryTapContact();
    public delegate void EndSecondaryTapPosition(Vector2 position);
    public delegate void EndSecondaryHoldDelta(Vector2 delta);

    public event StartPrimaryTapContact OnStartPrimaryTapContact;
    public event StartPrimaryTapPosition OnStartPrimaryTapPosition;
    public event StartPrimaryHoldDelta OnStartPrimaryHoldDelta;
    public event StartSecondaryTapContact OnStartSecondaryTapContact;
    public event StartSecondaryTapPosition OnStartSecondaryTapPosition;
    public event StartSecondaryHoldDelta OnStartSecondaryHoldDelta;

    public event EndPrimaryTapContact OnEndPrimaryTapContact;
    public event EndPrimaryTapPosition OnEndPrimaryTapPosition;
    public event EndPrimaryHoldDelta OnEndPrimaryHoldDelta;
    public event EndSecondaryTapContact OnEndSecondaryTapContact;
    public event EndSecondaryTapPosition OnEndSecondaryTapPosition;
    public event EndSecondaryHoldDelta OnEndSecondaryHoldDelta;

    [Header("Debug")]
    [SerializeField] private TextMeshProUGUI debugText;

    private void Awake()
    {
        touchInput = new TouchInput();
        touchControls = touchInput.TouchControls;
    }

    private void OnEnable()
    {
        touchInput.Enable();
    }

    private void OnDisable()
    {
        touchInput.Disable();
    }

    private void Start()
    {
        touchControls.PrimaryTapContact.started += PrimaryTapContactStarted;
        touchControls.PrimaryTapPosition.performed += PrimaryTapPositionStarted;
        touchControls.PrimaryHoldDelta.started += PrimaryHoldDeltaStarted;
        touchControls.SecondaryHoldContact.started += SecondaryHoldContactStarted;
        touchControls.SecondaryHoldPosition.performed += SecondaryHoldPositionStarted;
        touchControls.SecondaryHoldDelta.started += SecondaryHoldDeltaStarted;

        touchControls.PrimaryTapContact.canceled += PrimaryTapContactCancelled;
        touchControls.PrimaryTapPosition.canceled += PrimaryTapPositionCancelled;
        touchControls.PrimaryHoldDelta.canceled += PrimaryHoldDeltaCancelled;
        touchControls.SecondaryHoldContact.canceled += SecondaryHoldContactCancelled;
        touchControls.SecondaryHoldPosition.canceled += SecondaryHoldPositionCancelled;
        touchControls.SecondaryHoldDelta.canceled += SecondaryHoldDeltaCancelled; ;
    }

    private void Update()
    {
        if (debugText.gameObject.activeInHierarchy)
        {
            debugText.text =
                $"PrimaryTapContact: {primaryTapContact}\n" +
                $"PrimaryTapPosition: {primaryTapPosition}\n" +
                $"PrimaryHoldDelta: {primaryHoldDelta}\n" +
                $"PrimaryHoldDelta (mg): {primaryHoldDelta.magnitude}\n" +
                $"IsOverUI: {isOverUI}\n" +
                $"SecondaryTapContact: {secondaryTapContact}\n" +
                $"SecondaryTapPosition: {secondaryTapPosition}\n" +
                $"SecondaryHoldDelta: {secondaryHoldDelta}\n" +
                $"SecondaryHoldDelta (mg): {secondaryHoldDelta.magnitude}\n";
        }
    }

    private bool IsTapOverUI(Vector2 tapPos)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = tapPos;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                return true;
            }
        }

        return false;
    }

    #region Started Or Performed
    private void PrimaryTapContactStarted(InputAction.CallbackContext ctx)
    {
        primaryTapContact = ctx.ReadValueAsButton();
        OnStartPrimaryTapContact?.Invoke();
    }

    private void PrimaryTapPositionStarted(InputAction.CallbackContext ctx)
    {
        primaryTapPosition = ctx.ReadValue<Vector2>();
        isOverUI = IsTapOverUI(primaryTapPosition);
        if (!isOverUI)
        {
            OnStartPrimaryTapPosition?.Invoke(primaryTapPosition);
        }
    }

    private void PrimaryHoldDeltaStarted(InputAction.CallbackContext context)
    {
        primaryHoldDelta = context.ReadValue<Vector2>();
        OnStartPrimaryHoldDelta?.Invoke(primaryHoldDelta);
    }

    private void SecondaryHoldContactStarted(InputAction.CallbackContext obj)
    {
        secondaryTapContact = obj.ReadValueAsButton();
        OnStartSecondaryTapContact?.Invoke();
    }

    private void SecondaryHoldPositionStarted(InputAction.CallbackContext obj)
    {
        secondaryTapPosition = obj.ReadValue<Vector2>();
    }

    private void SecondaryHoldDeltaStarted(InputAction.CallbackContext obj)
    {
        secondaryHoldDelta = obj.ReadValue<Vector2>();
    }
    #endregion

    #region Cancelled
    private void PrimaryTapContactCancelled(InputAction.CallbackContext ctx)
    {
        primaryTapContact = ctx.ReadValueAsButton();
        OnEndPrimaryTapContact?.Invoke();
    }

    private void PrimaryTapPositionCancelled(InputAction.CallbackContext obj)
    {
        primaryTapPosition = obj.ReadValue<Vector2>();
        isOverUI = IsTapOverUI(primaryTapPosition);
        if (!isOverUI)
        {
            OnEndPrimaryTapPosition?.Invoke(primaryTapPosition);
        }
    }

    private void PrimaryHoldDeltaCancelled(InputAction.CallbackContext obj)
    {
        primaryHoldDelta = obj.ReadValue<Vector2>();
        OnEndPrimaryHoldDelta?.Invoke(primaryHoldDelta);
    } 

    private void SecondaryHoldPositionCancelled(InputAction.CallbackContext obj)
    {
        secondaryTapContact = obj.ReadValueAsButton();
    }

    private void SecondaryHoldContactCancelled(InputAction.CallbackContext obj)
    {
        secondaryTapPosition = obj.ReadValue<Vector2>();
    }

    private void SecondaryHoldDeltaCancelled(InputAction.CallbackContext obj)
    {
        secondaryHoldDelta = obj.ReadValue<Vector2>();
    }
    #endregion
}
