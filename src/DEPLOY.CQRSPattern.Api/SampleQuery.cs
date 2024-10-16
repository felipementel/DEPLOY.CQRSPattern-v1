using DEPLOY.CQRSPattern.Domain.Queries;
using System.Diagnostics.CodeAnalysis;

namespace DEPLOY.CQRSPattern.Api
{
    public class SampleQuery : IQuery<SampleQueryResult>, IParsable<SampleQuery>
    {
        public static SampleQuery Parse(string s, IFormatProvider? provider)
        {
            //help me
            return default;
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out SampleQuery result)
        {
            //help me 
            result = default;
            return false;


        }
    }
}