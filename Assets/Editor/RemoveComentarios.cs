using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;


public class RemoveComentarios : EditorWindow
{
    private string targetFolder = "Assets/Scripts"; // Carpeta predeterminada
    private bool includeSubfolders = true;

    [MenuItem("Tools/Remove Comments")]
    public static void ShowWindow()
    {
        GetWindow<RemoveComentarios>("Remove Comments");
    }

    private void OnGUI()
    {
        GUILayout.Label("Remove Comments from Scripts", EditorStyles.boldLabel);

        targetFolder = EditorGUILayout.TextField("Target Folder", targetFolder);
        includeSubfolders = EditorGUILayout.Toggle("Include Subfolders", includeSubfolders);

        if (GUILayout.Button("Remove Comments"))
        {
            RemoveCommentsInFolder(targetFolder, includeSubfolders);
        }
    }

    private void RemoveCommentsInFolder(string folderPath, bool includeSubfolders)
    {
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError($"The folder '{folderPath}' does not exist.");
            return;
        }

        // Obtén todos los archivos .cs en la carpeta seleccionada
        string[] files = Directory.GetFiles(folderPath, "*.cs",
            includeSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

        foreach (string file in files)
        {
            RemoveCommentsInFile(file);
        }

        Debug.Log($"Removed comments from {files.Length} script(s) in folder: {folderPath}");
        AssetDatabase.Refresh();
    }

    private void RemoveCommentsInFile(string filePath)
    {
        string fileContent = File.ReadAllText(filePath);

        // Expresión regular para eliminar comentarios
        string noComments = Regex.Replace(fileContent,
            @"(\/\/.*?$|\/\*[\s\S]*?\*\/)", // Coincide comentarios de línea y bloque
            string.Empty,
            RegexOptions.Multiline);

        // Escribe el archivo sin comentarios
        File.WriteAllText(filePath, noComments);
    }
}
