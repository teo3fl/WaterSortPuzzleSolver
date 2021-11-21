using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private readonly string savePath = "/Save";
    private readonly string extension = "wsp";

    [SerializeField]
    private GameObject go_dialogBox;
    [SerializeField]
    private GameObject go_saveSpecifficElements;
    [SerializeField]
    private GameObject go_loadSpecifficElements;
    [SerializeField]
    private GameObject go_deleteSpecifficElements;

    [SerializeField]
    private FileContainer fileNamesContainer;
    [SerializeField]
    private GameObject go_fileNameInputField;

    [SerializeField]
    private DialogHUD dialogHUD;


    private void Start()
    {
        fileNamesContainer.onElementSelected += ClearInputField;
    }

    // display

    public void DisplaySaveDialog()
    {
        // display UI
        go_dialogBox.SetActive(true);
        go_saveSpecifficElements.SetActive(true);
        StartCoroutine(DisplayFileList());
    }

    public void DisplayLoadDialog()
    {
        // display UI
        go_dialogBox.SetActive(true);
        go_loadSpecifficElements.SetActive(true);
        StartCoroutine(DisplayFileList());
    }

    public void DisplayDeleteDialog()
    {
        // display UI
        go_dialogBox.SetActive(true);
        go_deleteSpecifficElements.SetActive(true);
        StartCoroutine(DisplayFileList());
    }

    private IEnumerator DisplayFileList()
    {
        yield return new WaitForSeconds(0.1f);
        PopulateFileList();
    }

    private void PopulateFileList()
    {
        fileNamesContainer.ResetContents();
        var files = GetSaveFileNames();

        if (files.Count > 0)
        {
            fileNamesContainer.Display(files);
        }
    }

    private List<string> GetSaveFileNames()
    {
        var fileNames = new List<string>();
        string searchPattern = $"*.{extension}";
        try
        {
            var files = new DirectoryInfo(Application.persistentDataPath + savePath).GetFiles(searchPattern, SearchOption.AllDirectories);

            foreach (var file in files)
            {
                fileNames.Add(file.Name.TrimEnd(('.' + extension).ToCharArray()));
            }
        }
        catch
        {
            // directory doesn't exist
        }

        return fileNames;
    }


    // buttons

    public void OnSavePressed()
    {
        // create file
        string fileName = GetSelectedFileName();
        if (fileName == string.Empty)
        {
            dialogHUD.Display("You must provide a name for the file. Either by picking a file that already exists or by typing one.", "Close");
            return;
        }

        var data = Tuple.Create(ColorContainer.Instance.GetData(), BeakerContainer.Instance.GetData(), BeakerUI.MaxCapacity);
        SaveData(data, fileName);
        dialogHUD.Display("Saved.", "Close");

        ClearInputField();

        // hide UI
        go_dialogBox.SetActive(false);
        go_saveSpecifficElements.SetActive(false);
    }

    public void OnLoadPressed()
    {
        StartCoroutine(LoadData());
    }

    public void OnDeletePressed()
    {
        var fileName = fileNamesContainer.SelectedItem;
        if (fileName != string.Empty)
        {
            File.Delete(GetFullPath(fileName));
            fileNamesContainer.DeleteSelectedElement();
        }
        else
        {
            dialogHUD.Display("No file was selected", "Close");
        }
    }

    public void OnCancelPressed()
    {
        ClearInputField();
        go_saveSpecifficElements.SetActive(false);
        go_loadSpecifficElements.SetActive(false);
        go_dialogBox.SetActive(false);
    }


    // functionality

    private IEnumerator LoadData()
    {
        // gather data
        var (colorData, beakerData, maxCapacity) = LoadData(fileNamesContainer.SelectedItem);

        // load fill the containers
        try
        {
            ColorContainer.Instance.LoadData(colorData);
        }
        catch // the data wasn't loaded correctly
        {
            // reset the container
            BeakerContainer.Instance.ResetContents();

            dialogHUD.Display("Couldn't load the configuration from the given file.", "Close");
            yield break;
        }

        yield return new WaitForSeconds(0.1f);

        try
        {
            BeakerContainer.Instance.LoadData(beakerData, maxCapacity);

            // the data was loaded correctly
            dialogHUD.Display("Success.", "Close");
        }
        catch // the data wasn't loaded correctly
        {
            // reset the containers
            BeakerContainer.Instance.ResetContents();
            ColorContainer.Instance.ResetContents();

            dialogHUD.Display("Couldn't load the configuration from the given file.", "Close");
            yield break;
        }

        ClearInputField();
        // hide UI
        go_dialogBox.SetActive(false);
        go_loadSpecifficElements.SetActive(false);
    }

    private void SaveData(Tuple<List<ColorSampleData>, List<Beaker>, int> data, string fileName)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = GetFullPath(fileName);
        EnsureFolder(Application.persistentDataPath + savePath);
        FileStream stream = new FileStream(path, FileMode.Create);

        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    private Tuple<List<ColorSampleData>, List<Beaker>, int> LoadData(string fileName)
    {
        string path = GetFullPath(fileName);

        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            var data = binaryFormatter.Deserialize(stream) as Tuple<List<ColorSampleData>, List<Beaker>, int>;
            stream.Close();

            return data;
        }

        throw new FileNotFoundException();
    }

    private void EnsureFolder(string path)
    {
        if (!Directory.Exists(path))
        {
            string directoryName = Path.GetDirectoryName(path);
            Directory.CreateDirectory(directoryName);
        }
    }

    private string GetFullPath(string fileName)
    {
        return Application.persistentDataPath + savePath + '/' + fileName + '.' + extension;
    }

    private string GetSelectedFileName()
    {
        var textInput = go_fileNameInputField.GetComponent<TMPro.TMP_InputField>().text;

        if (textInput != string.Empty)
        {
            return textInput;
        }

        return fileNamesContainer.SelectedItem;
    }

    private void ClearInputField()
    {
        go_fileNameInputField.GetComponent<TMPro.TMP_InputField>().text = string.Empty;
    }
}
