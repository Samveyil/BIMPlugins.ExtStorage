using Autodesk.Revit.DB;

namespace BIMPlugins.ExtStorage.FailuresProcessing
{
    /// <summary>Helpers for configuring transactions.</summary>
    public static class TransactionHandler
    {
        /// <summary>Sets a failures preprocessor that handles warnings and errors when the transaction is committed.</summary>
        /// <param name="transaction">The transaction. Call this method after <see cref="Autodesk.Revit.DB.Transaction.Start()"/>.</param>
        /// <param name="preprocessor">The preprocessor, for example <see cref="WarningSkipper"/> or <see cref="OpeningsCreationSkipper"/>.</param>
        /// <example>
        /// <code>
        /// using var t = new Transaction(doc, "Create walls");
        /// t.Start();
        /// TransactionHandler.SetWarningResolver(t, new WarningSkipper());
        /// // ...
        /// t.Commit();
        /// </code>
        /// </example>
        public static void SetWarningResolver(Transaction transaction, IFailuresPreprocessor preprocessor)
        {
            FailureHandlingOptions failOptions = transaction.GetFailureHandlingOptions();
            failOptions.SetFailuresPreprocessor(preprocessor);
            transaction.SetFailureHandlingOptions(failOptions);
        }
    }
}
