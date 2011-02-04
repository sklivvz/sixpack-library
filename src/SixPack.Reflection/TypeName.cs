using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using SixPack.Text;

namespace SixPack.Reflection
{
	/// <summary>
	/// Parses names of .NET types.
	/// </summary>
	public sealed class TypeName
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TypeName"/> class.
		/// </summary>
		/// <param name="assemblyQualifiedName">Assembly qualified name of the type.</param>
		public TypeName(string assemblyQualifiedName)
		{
			new Parser().Parse(assemblyQualifiedName, this);
		}

		/// <summary>
		/// Gets or sets the name of the type, e.g. "IEnumerable"
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets or sets the namespace as a list, e.g. ["System", "Collections", "Generic"].
		/// </summary>
		public IList<string> Namespace { get; private set; }

		/// <summary>
		/// Gets or sets the container types.
		/// </summary>
		public IList<string> Nesting { get; private set; }

		/// <summary>
		/// Gets or sets the list of type arguments, e.g. [ "System.String" ].
		/// </summary>
		public IList<TypeName> TypeArguments { get; private set; }

		/// <summary>
		/// Gets or sets the name of the assembly.
		/// </summary>
		public AssemblyName AssemblyName { get; private set; }

		/// <summary>
		/// Gets the full name (with namespaces) in a human-readable format.
		/// </summary>
		public string FullName
		{
			get
			{
				return FormatAsString(t => t.Namespace.DelimitWith("", "{0}.") + t.Nesting.DelimitWith("", "{0}+") + t.Name);
			}
		}

		/// <summary>
		/// Gets the short name (without namespaces) in a human-readable format.
		/// </summary>
		public string ShortName
		{
			get
			{
				return FormatAsString(t => t.Name);
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return ShortName;
		}

		private string FormatAsString(Func<TypeName, string> display)
		{
			var args = TypeArguments
				.Select(display)
				.DelimitWith(", ", prefix: "<", suffix: ">");

			return display(this) + args;
		}

		private TypeName()
		{
		}

		#region Parser
		private sealed class Parser
		{
			private TextReader _reader;
			private char? _nextChar;

			private char Read()
			{
				var result = _nextChar ?? '\0';

				ReadNext();
				if(_nextChar == '\\')
				{
					ReadNext();
				}

				return result;
			}

			private void ReadNext()
			{
				var next = _reader.Read();
				_nextChar = next >= 0 ? (char)next : (char?)null;
			}

			private void Read(char expected)
			{
				if (Read() != expected)
				{
					throw new FormatException(string.Format("Expected '{0}'.", expected));
				}
			}

			private void IgnoreSpaces()
			{
				do
				{
					Read();
				} while (_nextChar == ' ');
			}

			private void ReadUntil(StringBuilder buffer, string delimiters)
			{
				while (_nextChar != null && delimiters.IndexOf(_nextChar.Value) < 0)
				{
					buffer.Append(Read());
				}
			}

			private string ReadUntil(string delimiters)
			{
				var buffer = new StringBuilder();
				ReadUntil(buffer, delimiters);
				return buffer.ToString();
			}

			public void Parse(string assemblyQualifiedName, TypeName typeName)
			{
				_reader = new StringReader(assemblyQualifiedName);
				Read();
				TypeSpec(typeName);

				if (_nextChar != null)
				{
					throw new FormatException("There are remaining unparsed characters.");
				}
			}

			private void TypeSpec(TypeName typeName)
			{
				var @namespace = new List<string>();
				while (true)
				{
					@namespace.Add(ReadUntil(".,+`["));
					if (_nextChar != '.')
					{
						break;
					}
					Read('.');
				}

				var typeNameList = @namespace;
				var nesting = new List<string>();
				if (_nextChar == '+')
				{
					while (true)
					{
						nesting.Add(ReadUntil(",+`"));
						if (_nextChar != '+')
						{
							break;
						}
						Read('+');
					}
					typeNameList = nesting;
				}

				typeName.Name = typeNameList[typeNameList.Count - 1];
				typeNameList.RemoveAt(typeNameList.Count - 1);

				typeName.Namespace = new ReadOnlyCollection<string>(@namespace);
				typeName.Nesting = new ReadOnlyCollection<string>(nesting);

				while (_nextChar == '[')
				{
					typeName.Name += ReadUntil("]") + ']';
					Read(']');
				}

				if (_nextChar == '`')
				{
					Read('`');
					var argCount = int.Parse(ReadUntil("["), CultureInfo.InvariantCulture);

					var typeArgs = new TypeName[argCount];
					Read('[');

					for (var i = 0; i < argCount; ++i)
					{
						Read('[');

						typeArgs[i] = new TypeName();
						TypeSpec(typeArgs[i]);

						if (i < argCount - 1)
						{
							ReadUntil("[");
						}
						else
						{
							Read(']');
						}
					}

					typeName.TypeArguments = new ReadOnlyCollection<TypeName>(typeArgs);

					Read(']');
				}
				else
				{
					typeName.TypeArguments = _emptyTypes;
				}

				if (_nextChar == ',')
				{
					IgnoreSpaces();
					var assemblyName = ReadUntil("]");
					typeName.AssemblyName = new AssemblyName(assemblyName);
				}
			}

			private static readonly IList<TypeName> _emptyTypes = new TypeName[0];
		}
		#endregion
	}
}
