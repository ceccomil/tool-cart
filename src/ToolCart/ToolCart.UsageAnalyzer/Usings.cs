global using Microsoft.CodeAnalysis;
global using Microsoft.CodeAnalysis.CSharp;
global using Microsoft.CodeAnalysis.CSharp.Syntax;
global using Microsoft.CodeAnalysis.Diagnostics;
global using System.Collections.Immutable;
global using System.Globalization;
global using System.Text;
global using System.Text.RegularExpressions;
global using ToolCart.UsageAnalyzer.Helpers;
global using static Shared.Helpers.Tags;
using System.Diagnostics.CodeAnalysis;


[assembly: SuppressMessage(
  "MicrosoftCodeAnalysisReleaseTracking",
  "RS2008",
  Justification = "Internal analyzer not versioned")]