using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;

namespace SixPack.Reflection.Tests
{
	[TestClass]
	public class ExpressionExtensionTests
	{
		public TestContext TestContext { get; set; }

		private interface IHasIdentifier
		{
			int Id { get; }
			int OtherId { get; }
		}

		private class Entity : IHasIdentifier
		{
			public int Id { get; set; }
			int IHasIdentifier.OtherId { get { return Id; } }
		}

		private Expression<Func<THasIdentifier, int>> MakeIdAccessExpression<THasIdentifier>()
			where THasIdentifier : IHasIdentifier
		{
			return hasId => hasId.Id;
		}

		private Expression<Func<THasIdentifier, int>> MakeOtherIdAccessExpression<THasIdentifier>()
			where THasIdentifier : IHasIdentifier
		{
			return hasId => hasId.OtherId;
		}

		private class ProhibitConvertVisitor : ExpressionVisitor
		{
			protected override Expression VisitUnary(UnaryExpression node)
			{
				Assert.AreNotEqual(ExpressionType.Convert, node.NodeType);

				return base.VisitUnary(node);
			}
		}

		private class CountConvertVisitor : ExpressionVisitor
		{
			public int Count { get; set; }

			protected override Expression VisitUnary(UnaryExpression node)
			{
				if (ExpressionType.Convert == node.NodeType)
					++Count;

				return base.VisitUnary(node);
			}
		}

		[TestMethod]
		public void RemoveConversionToSelf_RemovesCast()
		{
			var baseExpression = MakeIdAccessExpression<Entity>();
			TestContext.WriteLine(baseExpression.ToString());

			var convertedExpression = baseExpression.RemoveConversionToSelf();
			TestContext.WriteLine(convertedExpression.ToString());

			new ProhibitConvertVisitor().Visit(convertedExpression);
		}

		[TestMethod]
		public void RemoveConversionToSelf_DoesNotRemoveUnrelatedCasts()
		{
			Expression<Func<Entity, object>> baseExpression = e => (object)e.Id;
			TestContext.WriteLine(baseExpression.ToString());

			var convertedExpression = baseExpression.RemoveConversionToSelf();
			TestContext.WriteLine(convertedExpression.ToString());

			var visitor = new CountConvertVisitor();
			visitor.Visit(convertedExpression);
			Assert.AreEqual(1, visitor.Count);
		}

		[TestMethod]
		public void RemoveConversionToSelf_DoesNotRemoveRequiredCasts()
		{
			var baseExpression = MakeOtherIdAccessExpression<Entity>();
			TestContext.WriteLine(baseExpression.ToString());

			var convertedExpression = baseExpression.RemoveConversionToSelf();
			TestContext.WriteLine(convertedExpression.ToString());

			var visitor = new CountConvertVisitor();
			visitor.Visit(convertedExpression);
			Assert.AreEqual(1, visitor.Count);
		}
	}
}
