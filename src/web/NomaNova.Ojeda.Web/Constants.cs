using Blazored.Modal;

namespace NomaNova.Ojeda.Web
{
    public static class Constants
    {
        public const string EnvProduction = "Production";
        
        public const int SearchMinInputChars = 1;
        public const int SearchDebounceMs = 400;

        public const int DefaultTruncateSmall = 40;
        public const int DefaultTruncateLarge = 100;

        public const int DefaultPageNumber = 1;
        public const int DefaultPageSize = 25;
        
        public static readonly ModalOptions DefaultModalOptions = new()
        { 
            Animation = ModalAnimation.FadeInOut(0.15)
        };
        
        public const string StorageKeyPageSize = "key-pager-page-size";
        public const string StorageKeyViewMode = "key-view-selector-mode";
        
        public const long MaxImageSizeBytes = 2_097_152; // 2MB
    }
}