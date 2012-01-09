using System;
using SixPack.ComponentModel;

namespace SixPack
{
	/// <summary>
	/// Associates a value with an enum field.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public sealed class EnumValueAttribute : Attribute
	{
		private readonly object value;

		/// <summary>
		/// Gets the value.
		/// </summary>
		public object Value { get { return value; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumValueAttribute"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public EnumValueAttribute(string value)
		{
			this.value = value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumValueAttribute"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public EnumValueAttribute(char value)
		{
			this.value = value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumValueAttribute"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public EnumValueAttribute(byte value)
		{
			this.value = value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumValueAttribute"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public EnumValueAttribute(short value)
		{
			this.value = value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumValueAttribute"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public EnumValueAttribute(int value)
		{
			this.value = value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumValueAttribute"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public EnumValueAttribute(long value)
		{
			this.value = value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumValueAttribute"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public EnumValueAttribute(float value)
		{
			this.value = value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumValueAttribute"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public EnumValueAttribute(double value)
		{
			this.value = value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumValueAttribute"/> class.
		/// </summary>
		/// <param name="value">if set to <c>true</c> [value].</param>
		public EnumValueAttribute(bool value)
		{
			this.value = value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumValueAttribute"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public EnumValueAttribute(object value)
		{
			this.value = value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumValueAttribute"/> class.
		/// </summary>
		/// <param name="type">The type of the value.</param>
		/// <param name="value">The string representation of the value.</param>
		public EnumValueAttribute(Type type, string value)
		{
			this.value = TypeConverter.ChangeType(value, type);
		}
	}
}