namespace SlackPollingApp.Model.Slack.Common
{
    public static class BlockBuildingConstants
    {
        public const string InputPlaceholder = "Enter the value...";
        
        public const string PlainText = "plain_text";
        public const string Markdown = "mrkdwn";

        public const string ViewTypeModal = "modal";
        
        public const string BlockTypeInput = "input";
        public const string BlockTypeActions = "actions";
        public const string BlockTypeSection = "section";
        public const string BlockTypeDivider = "divider";
        
        public const string ElementTypeButton = "button";
        public const string ElementTypeCheckboxes = "checkboxes";
        public const string ElementTypePlainTextInput = "plain_text_input";
        
        public const string StyleDefault = "default";
        public const string StylePrimary = "primary";
        public const string StyleDanger = "danger";

        public const int InputMaxLength = 250;
        public const int MaxOptionsCount = 10;
        public const int MinOptionsCount = 2;
        public const int ProgressBarLength = 30;

        public const string ActionIdRemoveOption = "REMOVE_LATEST_OPTION";
        public const string ActionIdAddOption = "ADD_OPTION";
        public const string ActionIdCheckboxChange = "CHECKBOX_CHANGE";
        public const string ActionIdCheckboxes = "CHECKBOXES";
        public const string ActionIdQuestion = "QUESTION";
        public const string ActionIdOptionPrefix = "OPTION-";

        public const string OptionValueMultipleOptions = "OPTION_MULTIPLE_OPTIONS";
        public const string OptionValueShowVoters = "OPTION_SHOW_VOTERS";

        public static string GetOptionActionId(int index) => $"{ActionIdOptionPrefix}{index}";
    }
}