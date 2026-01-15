using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;
using BIMPlugins.ExtStorage.Methods;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace BIMPlugins.ExtStorage.Extensions
{
    public static class ModelPathExtensions
    { 
        public static Document OpenDocument(this ModelPath modelPath, string filePath, OpenOptions openOptions)
        {
            Document prDoc = null;

            try
            {
                prDoc = RevitAPI.Application.OpenDocumentFile(modelPath, openOptions);
            }
            catch (CannotOpenBothCentralAndLocalException)
            {
                MessageBox.Show("Закрой локальную копию проекта!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Autodesk.Revit.Exceptions.FileNotFoundException)
            {
                MessageBox.Show($"Файл - {filePath} не существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\n{ex.StackTrace}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return prDoc;
        }

        public static Document OpenDetachedDocument(this ModelPath modelPath, string filePath) => modelPath.OpenDocument(filePath, FileMethods.SetOpenOptions(modelPath));

        public static Document OpenLocalDocument(this ModelPath centralPath, string filePath, OpenOptions opt)
        {
            var localFilePath = GetLocalFilesFolderPath().AppendPath($"{Path.GetFileNameWithoutExtension(filePath)}_{RevitAPI.Application.Username}.rvt");
            ModelPath localPath = ModelPathUtils.ConvertUserVisiblePathToModelPath(localFilePath);

            if (File.Exists(localFilePath))
                File.Delete(localFilePath);

            WorksharingUtils.CreateNewLocal(centralPath, localPath);

            return localPath.OpenDocument(filePath, opt);
        }
        public static Document OpenLocalDocument(this ModelPath centralPath, string filePath)
        {
            var opt = new OpenOptions();
            var worksetConfig = new WorksetConfiguration(WorksetConfigurationOption.CloseAllWorksets);
            opt.SetOpenWorksetsConfiguration(worksetConfig);

            return centralPath.OpenLocalDocument(filePath, opt);
        }
        private static string GetLocalFilesFolderPath()
        {
            string path = $@"C:\Users\{Environment.UserName}\AppData\Roaming\Autodesk\Revit\Autodesk Revit {RevitAPI.Application.VersionNumber}\Revit.ini";

            string textContent = ExMethods.ReadTextFile(path);

            string[] splitString = textContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            return splitString.FirstOrDefault(l => l.StartsWith("ProjectPath=")).Substring(12);
        }
    }
}