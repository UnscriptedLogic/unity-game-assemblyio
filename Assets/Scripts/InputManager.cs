using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    //Touch Input
    private TouchInput touchInput;

    //Dynamic Values
    private bool primaryTapContact;
    private bool secondaryTapContact;
    private bool isOverUI;
    private Vector2 primaryTapPosition;
    private Vector2 secondaryTapPosition;
    private Vector2 primaryHoldDelta;
    private Vector2 secondaryHoldDelta;

    //Events
    public delegate void StartPrimaryTapContact(Vector2 position);
    public delegate void StartSecondaryTapContact(Vector2 position);
    public delegate void StartBuildTapContact(Vector2 position);
    public delegate void EndPrimaryTapContact();
    public delegate void EndSecondaryTapContact();
    public delegate void EndBuildTapContact();

    public event StartPrimaryTapContact OnStartPrimaryTapContact;
    public event StartSecondaryTapContact OnStartSecondaryTapContact;
    public event EndPrimaryTapContact OnEndPrimaryTapContact;
    public event EndSecondaryTapContact OnEndSecondaryTapContact;
    public event StartBuildTapContact OnStartBuildTapContact;
    public event EndBuildTapContact OnEndBuildTapContact;

    [Header("Debug")]
    [SerializeField] private TextMeshProUGUI debugText;

    public InputAction PrimaryTapPositionAction => touchInput.TouchControls.PanningContactPosition;
    public InputAction PrimaryHoldDeltaAction => touchInput.TouchControls.PrimaryHoldDelta;
    public InputAction SecondaryTapPositionAction => touchInput.TouchControls.SecondaryHoldPosition;
    public InputAction SecondaryHoldDeltaAction => touchInput.TouchControls.SecondaryHoldDelta;

    private void Awake()
    {
        touchInput = new TouchInput();
        instance = this;

        touchInput.TouchControls.PanningContact.started += PanningContactStarted;
        touchInput.TouchControls.SecondaryHoldContact.started += SecondaryHoldContactStarted;
        touchInput.TouchControls.PlaceBuild.started += PlaceBuildStarted;

        touchInput.TouchControls.PanningContact.canceled += PanningContactCancelled;
        touchInput.TouchControls.SecondaryHoldContact.canceled += SecondaryHoldContactCancelled;
        touchInput.TouchControls.PlaceBuild.canceled += PlaceBuildCancelled;
    }

    private void OnEnable()
    {
        touchInput.Enable();
    }

    private void OnDisable()
    {
        touchInput.Disable();
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
                $"SecondaryHoldDelta (mg): {secondaryHoldDelta.magnitude}\n" +
                $"Finger Distance: {Vector2.Distance(primaryTapPosition, secondaryTapPosition)}";
        }
    }

    //summary: Checks if the given screenpoint is over an UI element
    public bool IsTapOverUI(Vector2 tapPos)
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
    private void PanningContactStarted(InputAction.CallbackContext ctx)
    {
        primaryTapContact = ctx.ReadValueAsButton();
        OnStartPrimaryTapContact?.Invoke(touchInput.TouchControls.PanningContactPosition.ReadValue<Vector2>());
    }

    private void SecondaryHoldContactStarted(InputAction.CallbackContext obj)
    {
        secondaryTapContact = obj.ReadValueAsButton();
        OnStartSecondaryTapContact?.Invoke(touchInput.TouchControls.SecondaryHoldPosition.ReadValue<Vector2>());
    }

    private void PlaceBuildStarted(InputAction.CallbackContext obj)
    {
        OnStartBuildTapContact?.Invoke(touchInput.TouchControls.PlaceBuildPosition.ReadValue<Vector2>());
    }
    #endregion

    #region Cancelled
    private void PanningContactCancelled(InputAction.CallbackContext ctx)
    {
        primaryTapContact = ctx.ReadValueAsButton();
        OnEndPrimaryTapContact?.Invoke();
    }

    private void SecondaryHoldContactCancelled(InputAction.CallbackContext obj)
    {
        secondaryTapContact = obj.ReadValueAsButton();
        OnEndSecondaryTapContact?.Invoke();
    }

    private void PlaceBuildCancelled(InputAction.CallbackContext obj)
    {
        OnEndBuildTapContact?.Invoke();
    }
    #endregion
}
