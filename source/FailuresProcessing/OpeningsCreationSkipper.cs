using Autodesk.Revit.DB;

namespace BIMPlugins.ExtStorage.FailuresProcessing
{
    public class OpeningsCreationSkipper : IFailuresPreprocessor
    {
        public FailureProcessingResult PreprocessFailures(FailuresAccessor accessor)
        {
            IList<FailureMessageAccessor> failures = accessor.GetFailureMessages();
            foreach (FailureMessageAccessor failureMessageAccessor in failures)
            {
                if (failureMessageAccessor.HasResolutionOfType(FailureResolutionType.DeleteElements))
                {
                    failureMessageAccessor.SetCurrentResolutionType(FailureResolutionType.DeleteElements);

                    var id = failureMessageAccessor.GetFailureDefinitionId();
                    var failureSeverity = accessor.GetSeverity();
                    if (failureSeverity == FailureSeverity.Error)
                    {
                        accessor.ResolveFailure(failureMessageAccessor);
                        return FailureProcessingResult.ProceedWithCommit;
                    }
                    else
                    {
                        return FailureProcessingResult.Continue;
                    }
                }
            }
            return FailureProcessingResult.Continue;
        }
    }
}
