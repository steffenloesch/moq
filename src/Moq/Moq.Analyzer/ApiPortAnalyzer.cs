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
        private const string mockMetadataName = "Mock`1";

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(MockDiagnostics.ObsoleteApi);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeObjectCreation, Microsoft.CodeAnalysis.CSharp.SyntaxKind.ObjectCreationExpression);
            context.RegisterSyntaxNodeAction(AnalyzeObjectCreation, Microsoft.CodeAnalysis.VisualBasic.SyntaxKind.ObjectCreationExpression);
        }

        void AnalyzeObjectCreation(SyntaxNodeAnalysisContext context)
        {
            // Get the type symbol for Mock<T>.
            var genericMockType = context.Compilation.GetTypeByMetadataName(mockMetadataName);
            if (genericMockType == null)
                return;

            //TODO: continue implementation
        }
    }
}
