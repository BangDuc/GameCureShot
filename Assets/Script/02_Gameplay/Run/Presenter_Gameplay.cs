using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Presenter_Gameplay : MonoBehaviour
{
    [SerializeField]List<Transform> childs;

    private void Start()
    {
        foreach(Transform child in this.transform)
        {
            childs.Add(child);
        }
    }

    public PausePresenter Get_PausePresenter()
    {
        Transform pausePresenter = null;

        pausePresenter = Work_with_GameObject.FindDeepChild(this.transform, "PausePresenter");

        if (pausePresenter == null) return null;

        return pausePresenter.GetComponent<PausePresenter>();
    }

    public EmployeeCardPresenter Get_EmployeeCardPresenter()
    {
        Transform Presenter = null;

        Presenter = Work_with_GameObject.FindDeepChild(this.transform, "EmployeeCardPresenter");

        if (Presenter == null) return null;

        return Presenter.GetComponent<EmployeeCardPresenter>();
    }

    public CameraPresenter Get_CameraPresenter()
    {
        Transform Presenter = null;

        Presenter = Work_with_GameObject.FindDeepChild(this.transform, "CameraPresenter");

        if (Presenter == null) return null;

        return Presenter.GetComponent<CameraPresenter>();
    }

    public EndRunPresenter Get_EndRunPresenter()
    {
        Transform Presenter = null;

        Presenter = Work_with_GameObject.FindDeepChild(this.transform, "EndRunPresenter");

        if (Presenter == null) return null;

        return Presenter.GetComponent<EndRunPresenter>();
    }




}
