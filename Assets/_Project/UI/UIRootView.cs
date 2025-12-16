using UnityEngine;

public sealed class UIRootView : MonoBehaviour
{
    [SerializeField] private GameObject _loadingScreenPrefab;
    [SerializeField] private Transform _uiSceneContainer;

    private void Awake()
    {
        HideLoadingScreen();
    }

    public void ShowLoadingScreen()
    {
        _loadingScreenPrefab.SetActive(true);
    }

    public void HideLoadingScreen()
    {
        _loadingScreenPrefab.SetActive(false);
    }

    public void AttachSceneUI(GameObject sceneUI)
    {
        ClearSceneUI();

        sceneUI.transform.SetParent(_uiSceneContainer.transform);
    }

    public void AttachAdditiveSceneUI(GameObject sceneUI)
    {
        sceneUI.transform.SetParent(_uiSceneContainer, false);
    }

    private void ClearSceneUI()
    {
        for (int i = 0; i < _uiSceneContainer.transform.childCount; i++)
        {
            Destroy(_uiSceneContainer.GetChild(i).gameObject);
        }
    }
}
