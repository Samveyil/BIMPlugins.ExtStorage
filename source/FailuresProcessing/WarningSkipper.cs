using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace BIMPlugins.ExtStorage.FailuresProcessing
{
    /// <summary>
    /// An <see cref="Autodesk.Revit.DB.IFailuresPreprocessor"/> that deletes warnings so that they are not shown to the user.
    /// </summary>
    /// <remarks>Attach it to a transaction with <see cref="TransactionHandler.SetWarningResolver"/>.</remarks>
    public class WarningSkipper : IFailuresPreprocessor
    {
        /// <summary>Deletes all warnings of the transaction.</summary>
        /// <param name="accessor">The failures of the transaction being committed.</param>
        /// <remarks>If the transaction also has errors, nothing is deleted and the failures are processed by Revit as usual.</remarks>
        /// <returns>Always <see cref="Autodesk.Revit.DB.FailureProcessingResult.Continue"/>.</returns>
        public FailureProcessingResult PreprocessFailures(FailuresAccessor accessor)
        {
            IList<FailureMessageAccessor> failures = accessor.GetFailureMessages();
            foreach (FailureMessageAccessor failureMessageAccessor in failures)
            {
                var id = failureMessageAccessor.GetFailureDefinitionId();
                var failureSeverity = accessor.GetSeverity();
                if (failureSeverity == FailureSeverity.Warning)
                {
                    accessor.DeleteWarning(failureMessageAccessor);
                }
                else
                {
                    return FailureProcessingResult.Continue;
                }
            }
            return FailureProcessingResult.Continue;
        }
    }
}
