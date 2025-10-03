using Autodesk.Revit.DB;

namespace BIMPlugins.ExtStorage.FailuresProcessing
{
    public static class TransactionHandler
    {
        public static void SetWarningResolver(Transaction transaction, IFailuresPreprocessor preprocessor)
        {
            FailureHandlingOptions failOptions = transaction.GetFailureHandlingOptions();
            failOptions.SetFailuresPreprocessor(preprocessor);
            transaction.SetFailureHandlingOptions(failOptions);
        }
    }
}
