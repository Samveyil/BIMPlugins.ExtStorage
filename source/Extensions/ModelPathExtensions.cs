using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.UI;
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
        /// <summary>Opens a document from disk or cloud.</summary>
        /// <param name="filePath">The file to be opened.</param>
        /// <param name="openOptions">Options for opening the file.</param>
        /// <remarks>This method opens the document into memory but does not make it visible to the user in any way.</remarks>
        /// <returns>The opened document.</returns>
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

        /// <summary>Opens a document detached from its central file from disk or cloud.</summary>
        /// <param name="filePath">The file to be opened.</param>
        /// <param name="closeAllWorksets"><see langword="true"/> if to close all user-created worksets; otherwise, false.</param>
        /// <remarks>This method opens the document into memory but does not make it visible to the user in any way.</remarks>
        /// <returns>The opened document.</returns>
        public static Document OpenDetachedDocument(this ModelPath modelPath, string filePath, bool closeAllWorksets = true) =>
            modelPath.OpenDocument(filePath, FileMethods.SetOpenOptions(modelPath, DetachFromCentralOption.DetachAndPreserveWorksets, closeAllWorksets));

        /// <summary>Opens a new local file for the current user from disk or cloud.</summary>
        /// <param name="filePath">The file to be opened.</param>
        /// <param name="closeAllWorksets"><see langword="true"/> if to close all user-created worksets; otherwise, false.</param>
        /// <remarks>This method opens the document into memory but does not make it visible to the user in any way.</remarks>
        /// <returns>The opened document.</returns>
        public static Document OpenLocalDocument(this ModelPath centralPath, string filePath, bool closeAllWorksets = true)
        {
            var localFilePath = GetLocalFilesFolderPath().AppendPath($"{Path.GetFileNameWithoutExtension(filePath)}_{RevitAPI.Application.Username}.rvt");
            ModelPath localPath = ModelPathUtils.ConvertUserVisiblePathToModelPath(localFilePath);

            if (File.Exists(localFilePath))
                File.Delete(localFilePath);

            WorksharingUtils.CreateNewLocal(centralPath, localPath);

            return localPath.OpenDocument(filePath, FileMethods.SetOpenOptions(localPath, DetachFromCentralOption.DetachAndPreserveWorksets, closeAllWorksets));
        }

        /// <summary>Opens and activates a Revit document, include both local document or cloud document.</summary>
        /// <remarks>This method, if successful, changes the active document.</remarks>
        /// <returns>The opened document.</returns>
        public static UIDocument OpenAndActivateDocument(this ModelPath modelPath, OpenOptions openOptions) => RevitAPI.UIApplication.OpenAndActivateDocument(modelPath, openOptions, false);

        /// <summary>Opens and activates a Revit document detached from its central file, include both local document or cloud document.</summary>
        /// <param name="closeAllWorksets"><see langword="true"/> if to close all user-created worksets; otherwise, false.</param>
        /// <remarks>This method, if successful, changes the active document.</remarks>
        /// <returns>The opened document.</returns>
        public static UIDocument OpenAndActivateDetachedDocument(this ModelPath modelPath, bool closeAllWorksets = true) => 
            modelPath.OpenAndActivateDocument(FileMethods.SetOpenOptions(modelPath, DetachFromCentralOption.DetachAndPreserveWorksets, closeAllWorksets));

        /// <summary>Opens and activates a new local file for the current user, include both local document or cloud document.</summary>
        /// <param name="filePath">The file to be opened.</param>
        /// <param name="closeAllWorksets"><see langword="true"/> if to close all user-created worksets; otherwise, false.</param>
        /// <remarks>This method, if successful, changes the active document.</remarks>
        /// <returns>The opened document.</returns>
        public static UIDocument OpenAndActivateLocalDocument(this ModelPath centralPath, string filePath, bool closeAllWorksets = true)
        {
            var localFilePath = GetLocalFilesFolderPath().AppendPath($"{Path.GetFileNameWithoutExtension(filePath)}_{RevitAPI.Application.Username}.rvt");
            ModelPath localPath = ModelPathUtils.ConvertUserVisiblePathToModelPath(localFilePath);

            if (File.Exists(localFilePath))
                File.Delete(localFilePath);

            WorksharingUtils.CreateNewLocal(centralPath, localPath);

            return localPath.OpenAndActivateDocument(FileMethods.SetOpenOptions(localPath, DetachFromCentralOption.DoNotDetach, closeAllWorksets));
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