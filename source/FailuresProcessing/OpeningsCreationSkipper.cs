using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace BIMPlugins.ExtStorage.FailuresProcessing
{
    /// <summary>
    /// An <see cref="Autodesk.Revit.DB.IFailuresPreprocessor"/> that resolves errors by deleting the elements that caused them,
    /// so the transaction is committed instead of being rolled back.
    /// </summary>
    /// <remarks>
    /// Useful when creating many elements, for example openings, where invalid ones should be skipped.
    /// Attach it to a transaction with <see cref="TransactionHandler.SetWarningResolver"/>.
    /// </remarks>
    public class OpeningsCreationSkipper : IFailuresPreprocessor
    {
        /// <summary>Resolves the first error that can be fixed by deleting elements.</summary>
        /// <param name="accessor">The failures of the transaction being committed.</param>
        /// <remarks>Revit calls the preprocessor again after the commit is retried, so the remaining errors are resolved one by one.</remarks>
        /// <returns>
        /// <see cref="Autodesk.Revit.DB.FailureProcessingResult.ProceedWithCommit"/> if an error was resolved;
        /// otherwise <see cref="Autodesk.Revit.DB.FailureProcessingResult.Continue"/>.
        /// </returns>
        public FailureProcessingResult PreprocessFailures(FailuresAccessor accessor)
        {
            IList<FailureMessageAccessor> failures = accessor.GetFailureMessages();
            foreach (FailureMessageAccessor failure in failures)
            {
                if (failure.GetSeverity() == FailureSeverity.Error &&
                    failure.HasResolutionOfType(FailureResolutionType.DeleteElements))
                {
                    failure.SetCurrentResolutionType(FailureResolutionType.DeleteElements);
                    accessor.ResolveFailure(failure);
                    return FailureProcessingResult.ProceedWithCommit;
                }
            }

            return FailureProcessingResult.Continue;
        }
    }
}
