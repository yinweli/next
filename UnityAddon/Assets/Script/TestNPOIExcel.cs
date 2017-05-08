using FouridStudio;
using System;
using UnityEngine;

public class TestNPOIExcel : MonoBehaviour
{
    public string path = "";

    private void Start()
    {
        testCreateExcel();
        testLoadExcel();
    }

    private void testCreateExcel()
    {
        NPOIExcel excel = new NPOIExcel();

        excel["測試表單1"][1, 1].setValue("測試字串1");
        excel["測試表單1"][3, 1].setValue("測試字串2");
        excel["測試表單1"][5, 1].setValue("測試字串3");
        excel["測試表單1"][1, 3].setValue(10001);
        excel["測試表單1"][3, 3].setValue(10002);
        excel["測試表單1"][5, 3].setValue(10003);
        excel["測試表單1"][2, 2].setValue(99.001);
        excel["測試表單1"][4, 2].setValue(99.002);
        excel["測試表單1"][6, 2].setValue(99.003);
        excel["測試表單1"][2, 4].setValue(DateTime.Now.ToString());
        excel["測試表單1"][4, 4].setValue(DateTime.Now.ToString());
        excel["測試表單1"][6, 4].setValue(DateTime.Now.ToString());

        excel.save(path);
    }

    private void testLoadExcel()
    {
        NPOIExcel excel = new NPOIExcel(path);

        excel["測試表單2"][1, 1].setValue("測試字串1");
        excel["測試表單2"][3, 1].setValue("測試字串2");
        excel["測試表單2"][5, 1].setValue("測試字串3");
        excel["測試表單2"][1, 3].setValue(10001);
        excel["測試表單2"][3, 3].setValue(10002);
        excel["測試表單2"][5, 3].setValue(10003);
        excel["測試表單2"][2, 2].setValue(99.001);
        excel["測試表單2"][4, 2].setValue(99.002);
        excel["測試表單2"][6, 2].setValue(99.003);
        excel["測試表單2"][2, 4].setValue(DateTime.Now.ToString());
        excel["測試表單2"][4, 4].setValue(DateTime.Now.ToString());
        excel["測試表單2"][6, 4].setValue(DateTime.Now.ToString());

        excel.save(path);
    }
}