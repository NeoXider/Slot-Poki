using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject[] pages;

    void Start()
    {
        SetPage(0);
    }

    public void SetPage(int v)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == v);
        }
    }

    public void SetOveridePage(int v)
    {
        pages[v].SetActive(true);
    }
}
