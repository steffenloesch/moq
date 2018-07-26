using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Moq
{
    /// <summary>
    /// Generates diagnostics for using unsupported Moq APIs.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
    public class ApiPortAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => throw new NotImplementedException();
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(MockDiagnostics.ObsoleteApi);

        public override void Initialize(AnalysisContext context)
        {
            throw new NotImplementedException();
        }
    }
}
