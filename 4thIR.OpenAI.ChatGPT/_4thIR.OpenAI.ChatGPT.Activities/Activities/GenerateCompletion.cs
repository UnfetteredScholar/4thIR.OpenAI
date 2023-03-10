using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using _4thIR.OpenAI.ChatGPT.Activities.Properties;
using OpenAI;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using OpenAI.Models;

namespace _4thIR.OpenAI.ChatGPT.Activities
{
    [LocalizedDisplayName(nameof(Resources.GenerateCompletion_DisplayName))]
    [LocalizedDescription(nameof(Resources.GenerateCompletion_Description))]
    public class GenerateCompletion : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.Timeout_DisplayName))]
        [LocalizedDescription(nameof(Resources.Timeout_Description))]
        public InArgument<int> TimeoutMS { get; set; } = 60000;

        [LocalizedDisplayName(nameof(Resources.GenerateCompletion_APIKey_DisplayName))]
        [LocalizedDescription(nameof(Resources.GenerateCompletion_APIKey_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> APIKey { get; set; } = null;

        [LocalizedDisplayName(nameof(Resources.GenerateCompletion_Prompts_DisplayName))]
        [LocalizedDescription(nameof(Resources.GenerateCompletion_Prompts_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string[]> Prompts { get; set; }

        [LocalizedDisplayName(nameof(Resources.GenerateCompletion_Model_DisplayName))]
        [LocalizedDescription(nameof(Resources.GenerateCompletion_Model_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public Engine Engine { get; set; } = Engine.Davinci;

        [LocalizedDisplayName(nameof(Resources.GenerateCompletion_Temperature_DisplayName))]
        [LocalizedDescription(nameof(Resources.GenerateCompletion_Temperature_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<double?> Temperature { get; set; } = null;

        [LocalizedDisplayName(nameof(Resources.GenerateCompletion_MaximumTokens_DisplayName))]
        [LocalizedDescription(nameof(Resources.GenerateCompletion_MaximumTokens_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<int?> MaximumTokens { get; set; } = null;

        [LocalizedDisplayName(nameof(Resources.GenerateCompletion_StopSequences_DisplayName))]
        [LocalizedDescription(nameof(Resources.GenerateCompletion_StopSequences_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string[]> StopSequences { get; set; } = null;

        [LocalizedDisplayName(nameof(Resources.GenerateCompletion_TopP_DisplayName))]
        [LocalizedDescription(nameof(Resources.GenerateCompletion_TopP_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<double?> TopP { get; set; } = null;

        [LocalizedDisplayName(nameof(Resources.GenerateCompletion_FrequencyPenalty_DisplayName))]
        [LocalizedDescription(nameof(Resources.GenerateCompletion_FrequencyPenalty_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<double?> FrequencyPenalty { get; set; } = null;

        [LocalizedDisplayName(nameof(Resources.GenerateCompletion_PresencePenalty_DisplayName))]
        [LocalizedDescription(nameof(Resources.GenerateCompletion_PresencePenalty_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<double?> PresencePenalty { get; set; } = null;

        [LocalizedDisplayName(nameof(Resources.GenerateCompletion_NumberOfOutputs_DisplayName))]
        [LocalizedDescription(nameof(Resources.GenerateCompletion_NumberOfOutputs_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<int?> NumberOfOutputs { get; set; } = null;

        [LocalizedDisplayName(nameof(Resources.GenerateCompletion_LogProbabilities_DisplayName))]
        [LocalizedDescription(nameof(Resources.GenerateCompletion_LogProbabilities_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<int?> LogProbabilities { get; set; } = null;

        [LocalizedDisplayName(nameof(Resources.GenerateCompletion_Echo_DisplayName))]
        [LocalizedDescription(nameof(Resources.GenerateCompletion_Echo_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<bool?> Echo { get; set; } = null;

        [LocalizedDisplayName(nameof(Resources.GenerateCompletion_CompletionResult_DisplayName))]
        [LocalizedDescription(nameof(Resources.GenerateCompletion_CompletionResult_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<CompletionResult> CompletionResult { get; set; }

        #endregion

        #region Constructors

        public GenerateCompletion()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Prompts == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Prompts)));
            if (APIKey == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(APIKey)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var timeout = TimeoutMS.Get(context);


            // Set a timeout on the execution
            var task = ExecuteWithTimeout(context, cancellationToken);
            if (await Task.WhenAny(task, Task.Delay(timeout, cancellationToken)) != task) throw new TimeoutException(Resources.Timeout_Error);

            // Outputs
            return (ctx) => {
                CompletionResult.Set(ctx, task.Result);
            };
        }

        private async Task<CompletionResult> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {

            ChatGPTInterface chatGPTInterface = new ChatGPTInterface(APIKey.Get(context));

            var prompts = Prompts.Get(context);
            var temperature = Temperature.Get(context);
            var maximumLength = MaximumTokens.Get(context);
            var stopSequences = StopSequences.Get(context);
            var topP = TopP.Get(context);
            var frequencyPenalty = FrequencyPenalty.Get(context);
            var presencePenalty = PresencePenalty.Get(context);
            var numberOfOutputs = NumberOfOutputs.Get(context);
            var logProbabilities = LogProbabilities.Get(context);
            var echo = Echo.Get(context);

            CompletionRequest completionRequest = new CompletionRequest();

            completionRequest.Prompts = prompts;
            completionRequest.Temperature = temperature;
            completionRequest.MaxTokens = maximumLength;
            completionRequest.StopSequences = stopSequences;
            completionRequest.TopP = topP;
            completionRequest.FrequencyPenalty = frequencyPenalty;
            completionRequest.PresencePenalty = presencePenalty;
            completionRequest.NumChoicesPerPrompt = numberOfOutputs;
            completionRequest.LogProbabilities = logProbabilities;
            completionRequest.Echo = echo;

            switch (Engine)
            {
                case Engine.Ada:
                    completionRequest.Model = Model.Ada;
                    break;
                case Engine.Babbage:
                    completionRequest.Model = Model.Babbage;
                    break;
                case Engine.Curie:
                    completionRequest.Model = Model.Curie;
                    break;
                case Engine.Davinci:
                    completionRequest.Model = Model.Davinci;
                    break;
                default:
                    completionRequest.Model = Model.Default;
                    break;
            }

            var result = await chatGPTInterface.OpenAIClient.CompletionsEndpoint.CreateCompletionAsync(completionRequest);

            return result;
        }

        #endregion
    }
}

