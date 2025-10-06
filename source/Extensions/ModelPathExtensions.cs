using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;
using BIMPlugins.ExtStorage.Methods;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace BIMPlugins.ExtStorage.Extensions
{
    public static class ModelPathExtensions
    {
        public static Document OpenDocument(this ModelPath modelPath, string file)
        {
            Document prDoc = null;

            try
            {
                prDoc = RevitAPI.Application.OpenDocumentFile(modelPath, FileMethods.SetOpenOptions(modelPath));
            }
            catch (CannotOpenBothCentralAndLocalException)
            {
                MessageBox.Show("Закрой локальную копию проекта!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show($"Файл - {file} не существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\n{ex.StackTrace}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return prDoc;
        }
        public static Document OpenDocument(this ModelPath modelPath, string file, OpenOptions openOptions)
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
            catch (FileNotFoundException)
            {
                MessageBox.Show($"Файл - {file} не существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\n{ex.StackTrace}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return prDoc;
        }
    }
}