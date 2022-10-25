using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BuildManagement;
using UnityEngine.Events;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private TextMeshProUGUI costTMP;
    private BuildingSO buildingSO;

    public void UpdateButton(BuildingSO buildingSO, UnityAction OnClick)
    {
        this.buildingSO = buildingSO;
        icon.sprite = buildingSO.Icon;
        nameTMP.text = buildingSO.name;
        costTMP.text = $"${buildingSO.Cost}";
        button.onClick.AddListener(OnClick);
    }
}
