using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Button btn_start;
    [SerializeField] private Button btn_credit;
    [SerializeField] private Button btn_creditExit;
    [SerializeField] private GameObject pnl_credit;

    private void Start()
    {
        btn_start.onClick.AddListener(() => { SceneManager.LoadScene("InGame"); });
        btn_credit.onClick.AddListener(() => { pnl_credit.SetActive(!pnl_credit.activeSelf); });
        btn_creditExit.onClick.AddListener(() => { pnl_credit.SetActive(!pnl_credit.activeSelf); });
    }
}
