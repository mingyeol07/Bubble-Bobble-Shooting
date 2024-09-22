using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Button btn_start_normal;
    [SerializeField] private Button btn_start_defence;
    [SerializeField] private Button btn_eixt;

    private void Start()
    {
        btn_start_normal.onClick.AddListener(() => { SceneManager.LoadScene("Normal"); });
        btn_start_defence.onClick.AddListener(() => { SceneManager.LoadScene("Defence"); });
        btn_eixt.onClick.AddListener(() => Application.Quit());
    }
}
