﻿using Microsoft.CodeAnalysis;
using Moq.Properties;

namespace Moq
{
    public static class MockDiagnostics
    {
        public static DiagnosticDescriptor Missing { get; } = new DiagnosticDescriptor(
            "MOQ001",
            new ResourceString(nameof(Resources.MissingMockAnalyzer_Title)),
            new ResourceString(nameof(Resources.MissingMockAnalyzer_Message)),
            "Build",
            DiagnosticSeverity.Error,
            true,
            new ResourceString(nameof(Resources.MissingMockAnalyzer_Description)));

        public static DiagnosticDescriptor Outdated { get; } = new DiagnosticDescriptor(
            "MOQ002",
            new ResourceString(nameof(Resources.OutdatedMockAnalyzer_Title)),
            new ResourceString(nameof(Resources.OutdatedMockAnalyzer_Message)),
            "Build",
            DiagnosticSeverity.Error,
            true,
            new ResourceString(nameof(Resources.OutdatedMockAnalyzer_Description)));

        public static DiagnosticDescriptor ObsoleteApi { get; } = new DiagnosticDescriptor(
            "MOQ003",
            new ResourceString(nameof(Resources.ObsoleteApiAnalyzer_Title)),
            new ResourceString(nameof(Resources.ObsoleteApiAnalyzer_Message)),
            "Build",
            DiagnosticSeverity.Error,
            true,
            new ResourceString(nameof(Resources.ObsoleteApiAnalyzer_Description)));
    }
}
