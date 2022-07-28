using Esprima.Ast;
using JavascriptExpression = Esprima.Ast.Expression;

namespace ApiClient.PowerManagerClient;

/// <summary>
/// The JavascriptExpression Extension
/// </summary>
internal static class JavascriptExpressionExtension
{
    /// <summary>
    /// FilterNodes By its Type.
    /// </summary>
    /// <typeparam name="TStatement">The type of <see cref="Statement"/>.</typeparam>
    /// <param name="nodes">The nodes.</param>
    /// <returns>The nodes filtered.</returns>
    public static IEnumerable<TStatement> FilterNodesByType<TStatement>(this IEnumerable<Node> nodes) where TStatement : Statement
    {
        var nodeType = typeof(TStatement) switch
        {
            { } type when type == typeof(FunctionDeclaration) => Nodes.FunctionDeclaration,
            { } type when type == typeof(ExpressionStatement) => Nodes.ExpressionStatement,
            _ => throw new NotSupportedException(),
        };

        return nodes.Where(node => node.Type == nodeType).Cast<TStatement>();
    }

    /// <summary>
    /// Recursive Find the CallExpressions.
    /// </summary>
    /// <param name="expression">The Javascript Expression.</param>
    /// <param name="propertyName">The property name.</param>
    /// <returns>The CallExpressions.</returns>
    public static IEnumerable<CallExpression> RecursiveFindCallExpression(this JavascriptExpression expression, string propertyName)
    {
        if (expression is not CallExpression {Callee: MemberExpression callee} call) 
        {
            yield break;
        }

        if ((callee.Property as Identifier)?.Name == propertyName)
        {
            yield return call;
        }

        foreach (var subCall in callee.Object.RecursiveFindCallExpression(propertyName))
        {
            yield return subCall;
        }
    }

    /// <summary>
    /// SingleOrDefault ObjectExpression Properties.
    /// </summary>
    /// <typeparam name="TJavascriptExpression">The type of <see cref="JavascriptExpression"/>.</typeparam>
    /// <param name="objectExpression">The objectExpression.</param>
    /// <param name="predicate">The predicate.</param>
    /// <returns>The JavascriptExpression.</returns>
    public static TJavascriptExpression? SingleOrDefaultObjectExpressionProperties<TJavascriptExpression>(this ObjectExpression objectExpression, Func<Property, bool> predicate) where TJavascriptExpression : JavascriptExpression
    {
        return objectExpression.Properties.Cast<Property>().SingleOrDefault(predicate)?.Value as TJavascriptExpression;
    }
}