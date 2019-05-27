//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.6.6
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from D:\Files\Programming\GitHub\stork\stork\stork.g4 by ANTLR 4.6.6

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace stork {
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="storkParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.6.6")]
[System.CLSCompliant(false)]
public interface IstorkVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="storkParser.compileUnit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCompileUnit([NotNull] storkParser.CompileUnitContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="storkParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBlock([NotNull] storkParser.BlockContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="storkParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatement([NotNull] storkParser.StatementContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="storkParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpr([NotNull] storkParser.ExprContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="storkParser.stat_define"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStat_define([NotNull] storkParser.Stat_defineContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="storkParser.stat_assign"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStat_assign([NotNull] storkParser.Stat_assignContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="storkParser.stat_functionCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStat_functionCall([NotNull] storkParser.Stat_functionCallContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="storkParser.stat_functionDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStat_functionDef([NotNull] storkParser.Stat_functionDefContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="storkParser.object_reference"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitObject_reference([NotNull] storkParser.Object_referenceContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="storkParser.value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitValue([NotNull] storkParser.ValueContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="storkParser.params"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParams([NotNull] storkParser.ParamsContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="storkParser.funcdefparams"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFuncdefparams([NotNull] storkParser.FuncdefparamsContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="storkParser.operator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOperator([NotNull] storkParser.OperatorContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="storkParser.postfix_op"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPostfix_op([NotNull] storkParser.Postfix_opContext context);
}
} // namespace stork
