using System;
using System.Linq.Expressions;

namespace SixPack.Reflection
{
  partial class ExpressionExtensions
  {
    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Action> RemoveFirstParameter<T0>(this Expression<Action<T0>> expression, Expression replacement)
    {
      return Expression.Lambda<Action>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
        }
      );
    }

    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Func<TResult>> RemoveFirstParameter<T0, TResult>(this Expression<Func<T0, TResult>> expression, Expression replacement)
    {
      return Expression.Lambda<Func<TResult>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
        }
      );
    }
    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Func<T1, TResult>> RemoveFirstParameter<T0, T1, TResult>(this Expression<Func<T0, T1, TResult>> expression, Expression replacement)
    {
      return Expression.Lambda<Func<T1, TResult>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[1],
        }
      );
    }
    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Func<T1, T2, TResult>> RemoveFirstParameter<T0, T1, T2, TResult>(this Expression<Func<T0, T1, T2, TResult>> expression, Expression replacement)
    {
      return Expression.Lambda<Func<T1, T2, TResult>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[1],
                expression.Parameters[2],
        }
      );
    }
    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Func<T1, T2, T3, TResult>> RemoveFirstParameter<T0, T1, T2, T3, TResult>(this Expression<Func<T0, T1, T2, T3, TResult>> expression, Expression replacement)
    {
      return Expression.Lambda<Func<T1, T2, T3, TResult>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
        }
      );
    }
    
#if NET_40
    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Func<T1, T2, T3, T4, TResult>> RemoveFirstParameter<T0, T1, T2, T3, T4, TResult>(this Expression<Func<T0, T1, T2, T3, T4, TResult>> expression, Expression replacement)
    {
      return Expression.Lambda<Func<T1, T2, T3, T4, TResult>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
        }
      );
    }
    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Func<T1, T2, T3, T4, T5, TResult>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5, TResult>(this Expression<Func<T0, T1, T2, T3, T4, T5, TResult>> expression, Expression replacement)
    {
      return Expression.Lambda<Func<T1, T2, T3, T4, T5, TResult>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
        }
      );
    }
    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5, T6, TResult>(this Expression<Func<T0, T1, T2, T3, T4, T5, T6, TResult>> expression, Expression replacement)
    {
      return Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, TResult>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
                expression.Parameters[6],
        }
      );
    }
    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5, T6, T7, TResult>(this Expression<Func<T0, T1, T2, T3, T4, T5, T6, T7, TResult>> expression, Expression replacement)
    {
      return Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, TResult>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
                expression.Parameters[6],
                expression.Parameters[7],
        }
      );
    }
    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Expression<Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, TResult>> expression, Expression replacement)
    {
      return Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
                expression.Parameters[6],
                expression.Parameters[7],
                expression.Parameters[8],
        }
      );
    }
    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Expression<Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> expression, Expression replacement)
    {
      return Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
                expression.Parameters[6],
                expression.Parameters[7],
                expression.Parameters[8],
                expression.Parameters[9],
        }
      );
    }
    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Expression<Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> expression, Expression replacement)
    {
      return Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
                expression.Parameters[6],
                expression.Parameters[7],
                expression.Parameters[8],
                expression.Parameters[9],
                expression.Parameters[10],
        }
      );
    }
    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Expression<Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>> expression, Expression replacement)
    {
      return Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
                expression.Parameters[6],
                expression.Parameters[7],
                expression.Parameters[8],
                expression.Parameters[9],
                expression.Parameters[10],
                expression.Parameters[11],
        }
      );
    }
    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Expression<Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>> expression, Expression replacement)
    {
      return Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
                expression.Parameters[6],
                expression.Parameters[7],
                expression.Parameters[8],
                expression.Parameters[9],
                expression.Parameters[10],
                expression.Parameters[11],
                expression.Parameters[12],
        }
      );
    }
    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Expression<Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>> expression, Expression replacement)
    {
      return Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
                expression.Parameters[6],
                expression.Parameters[7],
                expression.Parameters[8],
                expression.Parameters[9],
                expression.Parameters[10],
                expression.Parameters[11],
                expression.Parameters[12],
                expression.Parameters[13],
        }
      );
    }
    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Expression<Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>> expression, Expression replacement)
    {
      return Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
                expression.Parameters[6],
                expression.Parameters[7],
                expression.Parameters[8],
                expression.Parameters[9],
                expression.Parameters[10],
                expression.Parameters[11],
                expression.Parameters[12],
                expression.Parameters[13],
                expression.Parameters[14],
        }
      );
    }
    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Expression<Func<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>> expression, Expression replacement)
    {
      return Expression.Lambda<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
                expression.Parameters[6],
                expression.Parameters[7],
                expression.Parameters[8],
                expression.Parameters[9],
                expression.Parameters[10],
                expression.Parameters[11],
                expression.Parameters[12],
                expression.Parameters[13],
                expression.Parameters[14],
                expression.Parameters[15],
        }
      );
    }
    #endif
    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Action<T1>> RemoveFirstParameter<T0, T1>(this Expression<Action<T0, T1>> expression, Expression replacement)
    {
      return Expression.Lambda<Action<T1>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[0],
                expression.Parameters[1],
        }
      );
    }

    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Action<T1, T2>> RemoveFirstParameter<T0, T1, T2>(this Expression<Action<T0, T1, T2>> expression, Expression replacement)
    {
      return Expression.Lambda<Action<T1, T2>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[0],
                expression.Parameters[1],
                expression.Parameters[2],
        }
      );
    }

    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Action<T1, T2, T3>> RemoveFirstParameter<T0, T1, T2, T3>(this Expression<Action<T0, T1, T2, T3>> expression, Expression replacement)
    {
      return Expression.Lambda<Action<T1, T2, T3>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[0],
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
        }
      );
    }

    
#if NET_40
    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Action<T1, T2, T3, T4>> RemoveFirstParameter<T0, T1, T2, T3, T4>(this Expression<Action<T0, T1, T2, T3, T4>> expression, Expression replacement)
    {
      return Expression.Lambda<Action<T1, T2, T3, T4>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[0],
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
        }
      );
    }

    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Action<T1, T2, T3, T4, T5>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5>(this Expression<Action<T0, T1, T2, T3, T4, T5>> expression, Expression replacement)
    {
      return Expression.Lambda<Action<T1, T2, T3, T4, T5>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[0],
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
        }
      );
    }

    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Action<T1, T2, T3, T4, T5, T6>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5, T6>(this Expression<Action<T0, T1, T2, T3, T4, T5, T6>> expression, Expression replacement)
    {
      return Expression.Lambda<Action<T1, T2, T3, T4, T5, T6>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[0],
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
                expression.Parameters[6],
        }
      );
    }

    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Action<T1, T2, T3, T4, T5, T6, T7>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5, T6, T7>(this Expression<Action<T0, T1, T2, T3, T4, T5, T6, T7>> expression, Expression replacement)
    {
      return Expression.Lambda<Action<T1, T2, T3, T4, T5, T6, T7>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[0],
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
                expression.Parameters[6],
                expression.Parameters[7],
        }
      );
    }

    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5, T6, T7, T8>(this Expression<Action<T0, T1, T2, T3, T4, T5, T6, T7, T8>> expression, Expression replacement)
    {
      return Expression.Lambda<Action<T1, T2, T3, T4, T5, T6, T7, T8>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[0],
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
                expression.Parameters[6],
                expression.Parameters[7],
                expression.Parameters[8],
        }
      );
    }

    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Expression<Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>> expression, Expression replacement)
    {
      return Expression.Lambda<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[0],
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
                expression.Parameters[6],
                expression.Parameters[7],
                expression.Parameters[8],
                expression.Parameters[9],
        }
      );
    }

    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Expression<Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> expression, Expression replacement)
    {
      return Expression.Lambda<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[0],
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
                expression.Parameters[6],
                expression.Parameters[7],
                expression.Parameters[8],
                expression.Parameters[9],
                expression.Parameters[10],
        }
      );
    }

    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Expression<Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> expression, Expression replacement)
    {
      return Expression.Lambda<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[0],
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
                expression.Parameters[6],
                expression.Parameters[7],
                expression.Parameters[8],
                expression.Parameters[9],
                expression.Parameters[10],
                expression.Parameters[11],
        }
      );
    }

    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Expression<Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> expression, Expression replacement)
    {
      return Expression.Lambda<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[0],
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
                expression.Parameters[6],
                expression.Parameters[7],
                expression.Parameters[8],
                expression.Parameters[9],
                expression.Parameters[10],
                expression.Parameters[11],
                expression.Parameters[12],
        }
      );
    }

    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Expression<Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> expression, Expression replacement)
    {
      return Expression.Lambda<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[0],
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
                expression.Parameters[6],
                expression.Parameters[7],
                expression.Parameters[8],
                expression.Parameters[9],
                expression.Parameters[10],
                expression.Parameters[11],
                expression.Parameters[12],
                expression.Parameters[13],
        }
      );
    }

    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Expression<Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> expression, Expression replacement)
    {
      return Expression.Lambda<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[0],
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
                expression.Parameters[6],
                expression.Parameters[7],
                expression.Parameters[8],
                expression.Parameters[9],
                expression.Parameters[10],
                expression.Parameters[11],
                expression.Parameters[12],
                expression.Parameters[13],
                expression.Parameters[14],
        }
      );
    }

    

    /// <summary>
    /// Replaces the first parameter of the specified lambda expression with another expression.
    /// </summary>
    public static Expression<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> RemoveFirstParameter<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Expression<Action<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> expression, Expression replacement)
    {
      return Expression.Lambda<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>(
        expression.Body.ReplaceParameter(expression.Parameters[0], replacement),
        new ParameterExpression[] {
                expression.Parameters[0],
                expression.Parameters[1],
                expression.Parameters[2],
                expression.Parameters[3],
                expression.Parameters[4],
                expression.Parameters[5],
                expression.Parameters[6],
                expression.Parameters[7],
                expression.Parameters[8],
                expression.Parameters[9],
                expression.Parameters[10],
                expression.Parameters[11],
                expression.Parameters[12],
                expression.Parameters[13],
                expression.Parameters[14],
                expression.Parameters[15],
        }
      );
    }

    #endif

  }
}