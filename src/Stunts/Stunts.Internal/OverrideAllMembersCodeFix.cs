﻿using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeGeneration;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Shared.Extensions;

namespace Stunts
{
    class OverrideAllMembersCodeFix : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get => ImmutableArray.Create(OverridableMembersAnalyzer.DiagnosticId);
        }

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var diagnostic = context.Diagnostics.FirstOrDefault(d => FixableDiagnosticIds.Contains(d.Id));
            if (diagnostic == null)
                return;

            var sourceToken = root.FindToken(diagnostic.Location.SourceSpan.Start);

            // Find the invocation identified by the diagnostic.
            var type =
                (SyntaxNode)sourceToken.Parent.AncestorsAndSelf().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax>().FirstOrDefault() ??
                (SyntaxNode)sourceToken.Parent.AncestorsAndSelf().OfType<Microsoft.CodeAnalysis.VisualBasic.Syntax.ClassBlockSyntax>().FirstOrDefault();

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: "Override All Members",
                    createChangedSolution: c => OverrideAllMembersAsync(context.Document, type, c),
                    equivalenceKey: nameof(OverrideAllMembersCodeFix)),
                diagnostic);
        }

        async Task<Solution> OverrideAllMembersAsync(Document document, SyntaxNode type, CancellationToken cancellationToken)
        {
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
            var symbol = semanticModel.GetDeclaredSymbol(type) as INamedTypeSymbol;
            if (symbol == null)
                return document.Project.Solution;

            var overridables = symbol.GetOverridableMembers(cancellationToken);
            if (type.Language == LanguageNames.VisualBasic)
                overridables = overridables.Where(x => x.MetadataName != "Finalize")
                    // VB doesn't support overriding events (yet). See https://github.com/dotnet/vblang/issues/63
                    .Where(x => x.Kind != SymbolKind.Event)
                    .ToImmutableArray();            

            var generator = SyntaxGenerator.GetGenerator(document);
            var memberTasks = overridables.SelectAsArray(
                m => generator.OverrideAsync(m, symbol, document, cancellationToken: cancellationToken));

            var members = await Task.WhenAll(memberTasks).ConfigureAwait(false);
            var newDoc = await CodeGenerator.AddMemberDeclarationsAsync(document.Project.Solution, symbol, members, cancellationToken: cancellationToken).ConfigureAwait(false);
            
            return newDoc.Project.Solution;
        }
    }
}
