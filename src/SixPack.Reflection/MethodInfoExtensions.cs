using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace SixPack.Reflection
{
	/// <summary>
	/// Extension methods for <see cref="MethodInfo"/>.
	/// </summary>
	public static class MethodInfoExtensions
	{
#if NET_40
		/// <summary/>
		public interface IMethodInvoker
		{
			/// <summary/>
			object Invoke(object obj, object[] arguments);
		}

		private static readonly ConditionalWeakTable<MethodInfo, IMethodInvoker> methodInvokers = new ConditionalWeakTable<MethodInfo, IMethodInvoker>();
		private static readonly Lazy<Tuple<AssemblyBuilder, ModuleBuilder>> methodInvokerModule = new Lazy<Tuple<AssemblyBuilder, ModuleBuilder>>(() =>
		{
			var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(
				new AssemblyName("MethodInvokers"),
				AssemblyBuilderAccess.Run
			);
			return Tuple.Create(assembly, assembly.DefineDynamicModule("MethodInvokers"));
		});

		/// <summary>
		/// Invokes the method. If the method throws an exception, that exception is NOT wrapped into a <see cref="TargetInvocationException"/>.
		/// </summary>
		/// <param name="method">The method to invoke.</param>
		/// <param name="obj">The object on which to invoke the method or constructor. If a method is static,
		///     this argument is ignored. If a constructor is static, this argument must
		///     be null or an instance of the class that defines the constructor.</param>
		/// <param name="arguments">An argument list for the invoked method or constructor. This is an array
		///     of objects with the same number, order, and type as the parameters of the
		///     method or constructor to be invoked. If there are no parameters, parameters
		///     should be null.If the method or constructor represented by this instance
		///     takes a ref parameter (ByRef in Visual Basic), no special attribute is required
		///     for that parameter in order to invoke the method or constructor using this
		///     function. Any object in this array that is not explicitly initialized with
		///     a value will contain the default value for that object type. For reference-type
		///     elements, this value is null. For value-type elements, this value is 0, 0.0,
		///     or false, depending on the specific element type.</param>
		/// <returns>An object containing the return value of the invoked method, or null in the
		///     case of a constructor.</returns>
		/// <remarks>
		/// This method only works on public methods.
		/// </remarks>
		public static object InvokeUnwrapped(this MethodInfo method, object obj, params object[] arguments)
		{
			if (method == null)
			{
				throw new NullReferenceException();
			}

			var invoker = methodInvokers.GetValue(method, CreateMethodInvoker);
			return invoker.Invoke(obj, arguments);
		}

		private static IMethodInvoker CreateMethodInvoker(MethodInfo method)
		{
			var assemblyAndModule = methodInvokerModule.Value;

			var module = assemblyAndModule.Item2;

			var parameters = method.GetParameters();
			var typeName = string.Format("{0}_{1}_{2}_{3}", method.DeclaringType.FullName.Replace('.', '_'), method.Name, parameters.Length, Guid.NewGuid());

			var invokerBuilder = module.DefineType(typeName, TypeAttributes.Class | TypeAttributes.Public, typeof(object), new[] { typeof(IMethodInvoker) });

			var invokerMethod = invokerBuilder.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.Virtual, typeof(object), new[] { typeof(object), typeof(object[]) });
			invokerBuilder.DefineMethodOverride(invokerMethod, typeof(IMethodInvoker).GetMethod("Invoke"));

			var il = invokerMethod.GetILGenerator();

			if (!method.IsStatic)
			{
				// "this" parameter
				il.Emit(OpCodes.Ldarg_1);

				// Cast to method's declaring type
				EmitCast(il, method.DeclaringType);
			}

			for (int i = 0; i < parameters.Length; ++i)
			{
				// "arguments" parameter
				il.Emit(OpCodes.Ldarg_2);

				// Array index
				il.Emit(OpCodes.Ldc_I4, i);

				// Read "arguments" array at index
				il.Emit(OpCodes.Ldelem_Ref);

				// Cast to parameter's type
				EmitCast(il, parameters[i].ParameterType);
			}

			// Invoke method
			il.EmitCall(OpCodes.Call, method, null);

			if (method.ReturnType == typeof(void))
			{
				// return null
				il.Emit(OpCodes.Ldnull);
			}
			else if (method.ReturnType.IsValueType)
			{
				// Box return value
				il.Emit(OpCodes.Box, method.ReturnType);
			}

			// Return
			il.Emit(OpCodes.Ret);

			var invokerType = invokerBuilder.CreateType();
			return (IMethodInvoker)Activator.CreateInstance(invokerType);
		}

		private static void EmitCast(ILGenerator il, Type type)
		{
			il.Emit(type.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, type);
		}

		//private class A
		//{
		//	public string Reference(int a, string b)
		//	{
		//		return null;
		//	}
		//	public int Value(int a, string b)
		//	{
		//		return 0;
		//	}
		//}

		//private struct B
		//{
		//	public string Reference(int a, string b)
		//	{
		//		return null;
		//	}
		//	public int Value(int a, string b)
		//	{
		//		return 0;
		//	}
		//}

		//private class MethodInvoker
		//{
		//	public object Invoke_Reference_Reference(object obj, object[] arguments)
		//	{
		//		return ((A)obj).Reference((int)arguments[0], (string)arguments[1]);
		//	}

		//	public object Invoke_Reference_Value(object obj, object[] arguments)
		//	{
		//		return ((A)obj).Value((int)arguments[0], (string)arguments[1]);
		//	}

		//	public object Invoke_Value_Reference(object obj, object[] arguments)
		//	{
		//		return ((B)obj).Reference((int)arguments[0], (string)arguments[1]);
		//	}

		//	public object Invoke_Value_Value(object obj, object[] arguments)
		//	{
		//		return ((B)obj).Value((int)arguments[0], (string)arguments[1]);
		//	}
		//}
#endif
	}
}