using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.UI;
using BIMPlugins.ExtStorage.Methods;
using System;
using System.IO;
using System.Linq;

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
        /// <param name="existingFile">What to do with an existing local file of the same name, as Revit asks when it creates a local file.</param>
        /// <remarks>
        /// This method opens the document into memory but does not make it visible to the user in any way.
        /// The local file is created in the default folder for user files from the Revit options (<c>ProjectPath</c> in Revit.ini,
        /// or the Documents folder if it is not set) as <c>&lt;file name&gt;_&lt;user name&gt;.rvt</c>; an existing local file with this name
        /// is overwritten or renamed with a timestamp, as <paramref name="existingFile"/> says.
        /// If the local file cannot be created or opened, <see langword="null"/> is returned instead of throwing an exception.
        /// </remarks>
        /// <returns>The opened document, or <see langword="null"/> if it cannot be opened.</returns>
        public static Document OpenLocalDocument(this ModelPath centralPath, bool closeAllWorksets = true,
            ExistingLocalFile existingFile = ExistingLocalFile.Overwrite) =>
            centralPath.OpenLocalDocument(out _, closeAllWorksets, existingFile);

        /// <summary>Opens a new local file for the current user from disk or cloud.</summary>
        /// <param name="centralPath">The path of the central model.</param>
        /// <param name="error">Why the document cannot be opened, ready to show to the user; <see langword="null"/> if it is opened.</param>
        /// <param name="closeAllWorksets"><see langword="true"/> to close all user-created worksets; otherwise, <see langword="false"/>.</param>
        /// <param name="existingFile">What to do with an existing local file of the same name, as Revit asks when it creates a local file.</param>
        /// <inheritdoc cref="OpenLocalDocument(Autodesk.Revit.DB.ModelPath, bool, ExistingLocalFile)" path="/remarks"/>
        /// <returns>The opened document, or <see langword="null"/> if it cannot be opened.</returns>
        public static Document OpenLocalDocument(this ModelPath centralPath, out string error, bool closeAllWorksets = true,
            ExistingLocalFile existingFile = ExistingLocalFile.Overwrite)
        {
            ModelPath localPath;
            try
            {
                localPath = CreateNewLocal(centralPath, existingFile);
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
        /// <remarks>
        /// This method, if successful, changes the active document.
        /// If the document cannot be opened, <see langword="null"/> is returned instead of throwing an exception.
        /// </remarks>
        /// <returns>The opened document, or <see langword="null"/> if it cannot be opened.</returns>
        public static UIDocument OpenAndActivateDocument(this ModelPath modelPath, OpenOptions openOptions) =>
            modelPath.OpenAndActivateDocument(openOptions, out _);

        /// <summary>Opens and activates a Revit document, from disk or cloud.</summary>
        /// <param name="modelPath">The path of the model.</param>
        /// <param name="openOptions">Options for opening the file, for example from <see cref="Methods.FileMethods.SetOpenOptions"/>.</param>
        /// <param name="error">Why the document cannot be opened, ready to show to the user; <see langword="null"/> if it is opened.</param>
        /// <inheritdoc cref="OpenAndActivateDocument(Autodesk.Revit.DB.ModelPath, Autodesk.Revit.DB.OpenOptions)" path="/remarks"/>
        /// <returns>The opened document, or <see langword="null"/> if it cannot be opened.</returns>
        public static UIDocument OpenAndActivateDocument(this ModelPath modelPath, OpenOptions openOptions, out string error)
        {
            error = null;
            try
            {
                return RevitAPI.UIApplication.OpenAndActivateDocument(modelPath, openOptions, false);
            }
            catch (Exception ex)
            {
                error = ErrorMessage(modelPath, ex);
                return null;
            }
        }

        /// <summary>Opens and activates a Revit document detached from its central file, from disk or cloud.</summary>
        /// <param name="modelPath">The path of the model.</param>
        /// <param name="closeAllWorksets"><see langword="true"/> to close all user-created worksets; otherwise, <see langword="false"/>.</param>
        /// <inheritdoc cref="OpenAndActivateDocument(Autodesk.Revit.DB.ModelPath, Autodesk.Revit.DB.OpenOptions)" path="/remarks"/>
        /// <returns>The opened document, or <see langword="null"/> if it cannot be opened.</returns>
        public static UIDocument OpenAndActivateDetachedDocument(this ModelPath modelPath, bool closeAllWorksets = true) =>
            modelPath.OpenAndActivateDetachedDocument(out _, closeAllWorksets);

        /// <summary>Opens and activates a Revit document detached from its central file, from disk or cloud.</summary>
        /// <param name="modelPath">The path of the model.</param>
        /// <param name="error">Why the document cannot be opened, ready to show to the user; <see langword="null"/> if it is opened.</param>
        /// <param name="closeAllWorksets"><see langword="true"/> to close all user-created worksets; otherwise, <see langword="false"/>.</param>
        /// <inheritdoc cref="OpenAndActivateDocument(Autodesk.Revit.DB.ModelPath, Autodesk.Revit.DB.OpenOptions)" path="/remarks"/>
        /// <returns>The opened document, or <see langword="null"/> if it cannot be opened.</returns>
        public static UIDocument OpenAndActivateDetachedDocument(this ModelPath modelPath, out string error, bool closeAllWorksets = true) =>
            modelPath.OpenAndActivateDocument(FileMethods.SetOpenOptions(modelPath, DetachFromCentralOption.DetachAndPreserveWorksets, closeAllWorksets), out error);

        /// <summary>Opens and activates a new local file for the current user, from disk or cloud.</summary>
        /// <param name="centralPath">The path of the central model.</param>
        /// <param name="closeAllWorksets"><see langword="true"/> to close all user-created worksets; otherwise, <see langword="false"/>.</param>
        /// <param name="existingFile">What to do with an existing local file of the same name, as Revit asks when it creates a local file.</param>
        /// <remarks>
        /// This method, if successful, changes the active document. The local file is created in the default folder for user files
        /// from the Revit options (<c>ProjectPath</c> in Revit.ini, or the Documents folder if it is not set) as
        /// <c>&lt;file name&gt;_&lt;user name&gt;.rvt</c>; an existing local file with this name is overwritten or renamed with
        /// a timestamp, as <paramref name="existingFile"/> says.
        /// If the local file cannot be created or opened, <see langword="null"/> is returned instead of throwing an exception.
        /// </remarks>
        /// <returns>The opened document, or <see langword="null"/> if it cannot be opened.</returns>
        public static UIDocument OpenAndActivateLocalDocument(this ModelPath centralPath, bool closeAllWorksets = true,
            ExistingLocalFile existingFile = ExistingLocalFile.Overwrite) =>
            centralPath.OpenAndActivateLocalDocument(out _, closeAllWorksets, existingFile);

        /// <summary>Opens and activates a new local file for the current user, from disk or cloud.</summary>
        /// <param name="centralPath">The path of the central model.</param>
        /// <param name="error">Why the document cannot be opened, ready to show to the user; <see langword="null"/> if it is opened.</param>
        /// <param name="closeAllWorksets"><see langword="true"/> to close all user-created worksets; otherwise, <see langword="false"/>.</param>
        /// <param name="existingFile">What to do with an existing local file of the same name, as Revit asks when it creates a local file.</param>
        /// <inheritdoc cref="OpenAndActivateLocalDocument(Autodesk.Revit.DB.ModelPath, bool, ExistingLocalFile)" path="/remarks"/>
        /// <returns>The opened document, or <see langword="null"/> if it cannot be opened.</returns>
        public static UIDocument OpenAndActivateLocalDocument(this ModelPath centralPath, out string error, bool closeAllWorksets = true,
            ExistingLocalFile existingFile = ExistingLocalFile.Overwrite)
        {
            ModelPath localPath;
            try
            {
                localPath = CreateNewLocal(centralPath, existingFile);
            }
            catch (Exception ex)
            {
                error = ErrorMessage(centralPath, ex);
                return null;
            }

            return localPath.OpenAndActivateDocument(FileMethods.SetOpenOptions(localPath, DetachFromCentralOption.DoNotDetach, closeAllWorksets), out error);
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

        // Creates <file name>_<user name>.rvt in the folder for local files. An existing local file is renamed with a timestamp
        // or deleted, as chosen. A file open in Revit can be neither: then the local copy is not created, and the caller gets the error.
        private static ModelPath CreateNewLocal(ModelPath centralPath, ExistingLocalFile existingFile)
        {
            var folder = GetLocalFilesFolderPath();
            var fileName = Path.GetFileNameWithoutExtension(ModelPathUtils.ConvertModelPathToUserVisiblePath(centralPath));

            // A user name such as DOMAIN\user would otherwise make the path invalid.
            var invalidChars = Path.GetInvalidFileNameChars();
            var userName = new string(RevitAPI.Application.Username.Select(c => invalidChars.Contains(c) ? '_' : c).ToArray());

            var localFilePath = Path.Combine(folder, $"{fileName}_{userName}.rvt");
            var localPath = ModelPathUtils.ConvertUserVisiblePathToModelPath(localFilePath);

            if (File.Exists(localFilePath))
            {
                if (existingFile == ExistingLocalFile.Overwrite)
                    File.Delete(localFilePath);
                else
                    File.Move(localFilePath, Path.Combine(folder, $"{fileName}_{userName}_{DateTime.Now:yyyyMMdd_HHmmss}.rvt"));
            }

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

    /// <summary>
    /// What to do with an existing local file of the same name when a new local file is created: the two choices
    /// Revit gives in its dialog.
    /// </summary>
    public enum ExistingLocalFile
    {
        /// <summary>Delete the existing file and its changes that are not synchronized. The default.</summary>
        Overwrite,

        /// <summary>
        /// Keep the existing file: it is renamed <c>&lt;file name&gt;_&lt;user name&gt;_yyyyMMdd_HHmmss.rvt</c>,
        /// so that changes not yet synchronized in it are not lost.
        /// </summary>
        AppendTimestamp
    }
}
