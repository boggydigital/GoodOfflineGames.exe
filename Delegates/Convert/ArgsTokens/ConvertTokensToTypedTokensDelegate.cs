using System.Collections.Generic;
using System.Threading.Tasks;

using Interfaces.Delegates.Convert;


using Attributes;

using Models.ArgsTokens;

namespace Delegates.Convert.ArgsTokens
{
    public class ConvertTokensToTypedTokensDelegate : 
        IConvertAsyncDelegate<
            IEnumerable<string>, 
            IAsyncEnumerable<(string, Tokens)>>
    {
        private IConvertAsyncDelegate<IEnumerable<string>, IAsyncEnumerable<(string, Tokens)>> convertTokensToLikelyTypedTokensDelegate;
        private IConvertAsyncDelegate<IAsyncEnumerable<(string, Tokens)>, IAsyncEnumerable<(string, Tokens)>> convertLikelyTypedToTypedTokensDelegate;
        private IConvertAsyncDelegate<IAsyncEnumerable<(string, Tokens)>, IAsyncEnumerable<(string, Tokens)>> convertMethodsSetTokensToMethodTitleTokensDelegate;

        [Dependencies(
            "Delegates.Convert.ArgsTokens.ConvertTokensToLikelyTypedTokensDelegate,Delegates",
            "Delegates.Convert.ArgsTokens.ConvertLikelyTypedToTypedTokensDelegate,Delegates",
            "Delegates.Convert.ArgsTokens.ConvertMethodsSetTokensToMethodTitleTokensDelegate,Delegates")]
        public ConvertTokensToTypedTokensDelegate(
            IConvertAsyncDelegate<IEnumerable<string>, IAsyncEnumerable<(string, Tokens)>> convertTokensToLikelyTypedTokensDelegate,
            IConvertAsyncDelegate<IAsyncEnumerable<(string, Tokens)>, IAsyncEnumerable<(string, Tokens)>> convertLikelyTypedToTypedTokensDelegate,
            IConvertAsyncDelegate<IAsyncEnumerable<(string, Tokens)>, IAsyncEnumerable<(string, Tokens)>> convertMethodsSetTokensToMethodTitleTokensDelegate)
        {
            this.convertTokensToLikelyTypedTokensDelegate = convertTokensToLikelyTypedTokensDelegate;
            this.convertLikelyTypedToTypedTokensDelegate = convertLikelyTypedToTypedTokensDelegate;
            this.convertMethodsSetTokensToMethodTitleTokensDelegate = convertMethodsSetTokensToMethodTitleTokensDelegate;
        }
        public IAsyncEnumerable<(string, Tokens)> ConvertAsync(IEnumerable<string> tokens)
        {
            // 1. Convert untyped tokens into a likely-typed tokens
            var likelyTypedTokens = 
                convertTokensToLikelyTypedTokensDelegate.ConvertAsync(
                    tokens);
            // 2. Resolve likely types into specific types
            var typedTokens = 
                convertLikelyTypedToTypedTokensDelegate.ConvertAsync(
                    likelyTypedTokens);
            // 3. Expand methods sets (if present)
            var expandedTypedTokens = 
                convertMethodsSetTokensToMethodTitleTokensDelegate.ConvertAsync(
                    typedTokens);

            return expandedTypedTokens;
        }
    }
}