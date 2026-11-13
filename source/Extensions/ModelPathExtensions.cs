using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.UI;
using BIMPlugins.ExtStorage.Methods;
using System;
using System.IO;

namespace BIMPlugins.ExtStorage.Extensions
{
    /// <summary>Extension methods for <see cref="Autodesk.Revit.DB.ModelPath"/>: opening documents in background or in the UI.</summary>
    public static class ModelPathExtensions
    {
        /// <summary>Opens a document from disk or cloud.</summary>
        /// <param name="modelPath">The path of the model.</param>
        /// <param name="openOptions">Options for opening the file.</param>
        /// <remarks>
        /// This method opens the document into memory but does not make it visible to the user in any way.
        /// If the document cannot be opened, <see langword="null"/> is returned instead of throwing an exception;
        /// the overload with an <c>error</c> parameter tells why.
        /// </remarks>
        /// <returns>The opened document, or <see langword="null"/> if it cannot be opened.</returns>
        public static Document OpenDocument(this ModelPath modelPath, OpenOptions openOptions) =>
            modelPath.OpenDocument(openOptions, out _);

        /// <summary>Opens a document from disk or cloud.</summary>
        /// <param name="modelPath">The path of the model.</param>
        /// <param name="openOptions">Options for opening the file.</param>
        /// <param name="error">Why the document cannot be opened, ready to show to the user; <see langword="null"/> if it is opened.</param>
        /// <remarks>
        /// This method opens the document into memory but does not make it visible to the user in any way.
        /// If the document cannot be opened, <see langword="null"/> is returned instead of throwing an exception.
        /// </remarks>
        /// <returns>The opened document, or <see langword="null"/> if it cannot be opened.</returns>
        public static Document OpenDocument(this ModelPath modelPath, OpenOptions openOptions, out string error)
        {
            error = null;
            try
            {
                return RevitAPI.Application.OpenDocumentFile(modelPath, openOptions);
            }
            catch (Exception ex)
            {
                error = ErrorMessage(modelPath, ex);
                return null;
            }
        }

        /// <summary>Opens a document detached from its central file from disk or cloud.</summary>
        /// <param name="modelPath">The path of the model.</param>
        /// <param name="closeAllWorksets"><see langword="true"/> to close all user-created worksets; otherwise, <see langword="false"/>.</param>
        /// <inheritdoc cref="OpenDocument(Autodesk.Revit.DB.ModelPath, Autodesk.Revit.DB.OpenOptions)" path="/remarks"/>
        /// <returns>The opened document, or <see langword="null"/> if it cannot be opened.</returns>
        public static Document OpenDetachedDocument(this ModelPath modelPath, bool closeAllWorksets = true) =>
            modelPath.OpenDetachedDocument(out _, closeAllWorksets);

        /// <summary>Opens a document detached from its central file from disk or cloud.</summary>
        /// <param name="modelPath">The path of the model.</param>
        /// <param name="error">Why the document cannot be opened, ready to show to the user; <see langword="null"/> if it is opened.</param>
        /// <param name="closeAllWorksets"><see langword="true"/> to close all user-created worksets; otherwise, <see langword="false"/>.</param>
        /// <inheritdoc cref="OpenDocument(Autodesk.Revit.DB.ModelPath, Autodesk.Revit.DB.OpenOptions, out string)" path="/remarks"/>
        /// <returns>The opened document, or <see langword="null"/> if it cannot be opened.</returns>
        public static Document OpenDetachedDocument(this ModelPath modelPath, out string error, bool closeAllWorksets = true) =>
            modelPath.OpenDocument(FileMethods.SetOpenOptions(modelPath, DetachFromCentralOption.DetachAndPreserveWorksets, closeAllWorksets), out error);

        /// <summary>Opens a new local file for the current user from disk or cloud.</summary>
        /// <param name="centralPath">The path of the central model.</param>
        /// <param name="closeAllWorksets"><see langword="true"/> to close all user-created worksets; otherwise, <see langword="false"/>.</param>
        /// <remarks>
        /// This method opens the document into memory but does not make it visible to the user in any way.
        /// The local file is created in the default folder for user files from the Revit options (<c>ProjectPath</c> in Revit.ini,
        /// or the Documents folder if it is not set) as <c>&lt;file name&gt;_&lt;user name&gt;.rvt</c>; an existing local file with this name is deleted.
        /// If the local file cannot be created or opened, <see langword="null"/> is returned instead of throwing an exception.
        /// </remarks>
        /// <returns>The opened document, or <see langword="null"/> if it cannot be opened.</returns>
        public static Document OpenLocalDocument(this ModelPath centralPath, bool closeAllWorksets = true) =>
            centralPath.OpenLocalDocument(out _, closeAllWorksets);

        /// <summary>Opens a new local file for the current user from disk or cloud.</summary>
        /// <param name="centralPath">The path of the central model.</param>
        /// <param name="error">Why the document cannot be opened, ready to show to the user; <see langword="null"/> if it is opened.</param>
        /// <param name="closeAllWorksets"><see langword="true"/> to close all user-created worksets; otherwise, <see langword="false"/>.</param>
        /// <inheritdoc cref="OpenLocalDocument(Autodesk.Revit.DB.ModelPath, bool)" path="/remarks"/>
        /// <returns>The opened document, or <see langword="null"/> if it cannot be opened.</returns>
        public static Document OpenLocalDocument(this ModelPath centralPath, out string error, bool closeAllWorksets = true)
        {
            ModelPath localPath;
            try
            {
                localPath = CreateNewLocal(centralPath);
            }
            catch (Exception ex)
            {
                error = ErrorMessage(centralPath, ex);
                return null;
            }

            return localPath.OpenDocument(FileMethods.SetOpenOptions(localPath, DetachFromCentralOption.DoNotDetach, closeAllWorksets), out error);
        }

        /// <summary>Opens and activates a Revit document, from disk or cloud.</summary>
        /// <param name="modelPath">The path of the model.</param>
        /// <param name="openOptions">Options for opening the file, for example from <see cref="Methods.FileMethods.SetOpenOptions"/>.</param>
        /// <remarks>This method, if successful, changes the active document.</remarks>
        /// <returns>The opened document.</returns>
        public static UIDocument OpenAndActivateDocument(this ModelPath modelPath, OpenOptions openOptions) => RevitAPI.UIApplication.OpenAndActivateDocument(modelPath, openOptions, false);

        /// <summary>Opens and activates a Revit document detached from its central file, from disk or cloud.</summary>
        /// <param name="modelPath">The path of the model.</param>
        /// <param name="closeAllWorksets"><see langword="true"/> to close all user-created worksets; otherwise, <see langword="false"/>.</param>
        /// <remarks>This method, if successful, changes the active document.</remarks>
        /// <returns>The opened document.</returns>
        public static UIDocument OpenAndActivateDetachedDocument(this ModelPath modelPath, bool closeAllWorksets = true) =>
            modelPath.OpenAndActivateDocument(FileMethods.SetOpenOptions(modelPath, DetachFromCentralOption.DetachAndPreserveWorksets, closeAllWorksets));

        /// <summary>Opens and activates a new local file for the current user, from disk or cloud.</summary>
        /// <param name="centralPath">The path of the central model.</param>
        /// <param name="closeAllWorksets"><see langword="true"/> to close all user-created worksets; otherwise, <see langword="false"/>.</param>
        /// <remarks>
        /// This method, if successful, changes the active document. The local file is created in the default folder for user files
        /// from the Revit options (<c>ProjectPath</c> in Revit.ini, or the Documents folder if it is not set) as
        /// <c>&lt;file name&gt;_&lt;user name&gt;.rvt</c>; an existing local file with this name is deleted.
        /// </remarks>
        /// <returns>The opened document.</returns>
        public static UIDocument OpenAndActivateLocalDocument(this ModelPath centralPath, bool closeAllWorksets = true)
        {
            var localPath = CreateNewLocal(centralPath);
            return localPath.OpenAndActivateDocument(FileMethods.SetOpenOptions(localPath, DetachFromCentralOption.DoNotDetach, closeAllWorksets));
        }


        // A message for the user: the cases that need an action from them are explained, other errors keep Revit's text.
        private static string ErrorMessage(ModelPath modelPath, Exception exception)
        {
            var path = ModelPathUtils.ConvertModelPathToUserVisiblePath(modelPath);

            switch (exception)
            {
                case CannotOpenBothCentralAndLocalException _:
                    return $"Close the local copy of {path} and try again: the central model and its local copy cannot be open at the same time.";
                case Autodesk.Revit.Exceptions.FileNotFoundException _:
                case System.IO.FileNotFoundException _:
                    return $"The file {path} does not exist.";
                default:
                    return $"{path} cannot be opened: {exception.Message}";
            }
        }

        // Creates <file name>_<user name>.rvt in the folder for local files, replacing an existing one.
        private static ModelPath CreateNewLocal(ModelPath centralPath)
        {
            var fileName = Path.GetFileNameWithoutExtension(ModelPathUtils.ConvertModelPathToUserVisiblePath(centralPath));
            var localFilePath = Path.Combine(GetLocalFilesFolderPath(), $"{fileName}_{RevitAPI.Application.Username}.rvt");
            var localPath = ModelPathUtils.ConvertUserVisiblePathToModelPath(localFilePath);

            if (File.Exists(localFilePath))
                File.Delete(localFilePath);

            WorksharingUtils.CreateNewLocal(centralPath, localPath);
            return localPath;
        }

        // The "Default path for user files" from Revit's options: ProjectPath in the [Directories] section of Revit.ini
        // in the user's roaming profile. Without it, or if that folder is gone, Revit's own default, the Documents folder.
        private static string GetLocalFilesFolderPath()
        {
            var ini = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Autodesk", "Revit", "Autodesk Revit " + RevitAPI.Application.VersionNumber, "Revit.ini");

            const string key = "ProjectPath=";
            if (File.Exists(ini))
            {
                // Revit.ini is UTF-16 with a byte order mark, which ReadLines detects.
                foreach (var line in File.ReadLines(ini))
                {
                    if (!line.StartsWith(key, StringComparison.OrdinalIgnoreCase))
                        continue;

                    var path = Environment.ExpandEnvironmentVariables(line.Substring(key.Length).Trim());
                    if (path.Length > 0 && Directory.Exists(path))
                        return path;
                    break;
                }
            }

            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
    }
}
