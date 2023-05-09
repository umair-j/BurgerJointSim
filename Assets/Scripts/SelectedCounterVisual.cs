using UnityEngine;
public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter counter;
    [SerializeField]private GameObject counterVisual;
    [SerializeField] private GameObject selectedCounterVisual;

    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Instance_OnSelectedCounterChanged;
    }
    private void Instance_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == counter)
        {
            ChangeToSelectedCounterVisual();
        }
        else
        {
            ChangeToUnSelectedCounterVisual();
        }
    }
    private void ChangeToSelectedCounterVisual()
    {
        selectedCounterVisual.SetActive(true);
        counterVisual.SetActive(false);
    }
    private void ChangeToUnSelectedCounterVisual()
    {
        selectedCounterVisual.SetActive(false);
        counterVisual.SetActive(true);
    }
}