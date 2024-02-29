using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace nanoFramework.SourceGenerators.Generators
{
    internal sealed class ResourceIdEnumGenerator : IResourceIdEnumGenerator
    {
        public string GenerateSource()
        {
            var enumDeclaration = EnumDeclaration(
                attributeLists: SingletonList(
                    AttributeList(
                        SingletonSeparatedList(
                            Attribute(
                                IdentifierName("System.Serializable")
                            )
                        )
                    )
                ),
                modifiers: TokenList(
                    Token(SyntaxKind.InternalKeyword)
                ),
                identifier: Identifier(Constants.ResourceId.EnumIdentifier),
                baseList: BaseList(
                    SingletonSeparatedList<BaseTypeSyntax>(
                        SimpleBaseType(
                            PredefinedType(
                                Token(SyntaxKind.ShortKeyword)
                            )
                        )
                    )
                ),
                members: SeparatedList<EnumMemberDeclarationSyntax>());

            var namespaceDeclaration = NamespaceDeclaration(ParseName(Constants.ResourceMetadataNamespace))
                .AddMembers(enumDeclaration);

            var compilationUnit = CompilationUnit()
                .AddMembers(namespaceDeclaration);

            return SyntaxTree(compilationUnit, encoding: Encoding.UTF8)
                .GetRoot().NormalizeWhitespace().ToString();
        }
    }
}
