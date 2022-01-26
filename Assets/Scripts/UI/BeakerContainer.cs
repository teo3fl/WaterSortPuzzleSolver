using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeakerContainer : ContainerManager
{
    public static BeakerContainer Instance { get; private set; }

    [SerializeField]
    private GameObject go_beaker;
    [SerializeField]
    private ColorSample defaultColorSample;
    public ColorSample DefaultColorSample { get { return defaultColorSample; } }

    [SerializeField]
    private Slider sl_beakerCapacity;


    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            //UpdateBaseContainerHeight();
        }
    }

    protected override void InstantiateElement()
    {
        var beaker = Instantiate(go_beaker, t_container).GetComponent<BeakerUI>();
        beaker.container = this;
        beaker.Initialize();
    }

    private void InstantiateElement(Beaker data)
    {
        var beaker = Instantiate(go_beaker, t_container).GetComponent<BeakerUI>();
        beaker.container = this;
        beaker.Initialize();
        beaker.SetData(data);
    }

    public List<Beaker> GetData()
    {
        var list = new List<Beaker>();
        for (int i = 0; i < t_container.childCount - 1; ++i)
        {
            list.Add(t_container.GetChild(i).GetComponent<BeakerUI>().GetData());
        }

        return list;
    }

    public void LoadData(List<Beaker> beakers, int maxCapacity)
    {
        ResetContents();
        sl_beakerCapacity.value = maxCapacity;

        foreach (var beakerData in beakers)
        {
            InstantiateElement(beakerData);
        }

       SnapElements();
    }

    public override void ResetContents()
    {
        for (int i = 0; i < t_container.childCount - 1; ++i)
        {
            Destroy(t_container.GetChild(i).gameObject);
        }
    }
}
