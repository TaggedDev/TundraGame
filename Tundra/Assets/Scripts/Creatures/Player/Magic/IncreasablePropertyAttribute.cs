using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Creatures.Player.Magic
{
    /// <summary>
    /// An attribute for a <see langword="double"/> typed property with automatic increasing with given elements
    /// </summary>
    /// <example>
    /// For example, if you have a damage modifier which should depend on reagents with special element type,
    /// you can give them this attribute to increase them automatically.
    /// </example>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class IncreasablePropertyAttribute : Attribute
    {
        private readonly double _defaultAdditionStep;
        /// <summary>
        /// Mode of property increasion.
        /// </summary>
        public enum IncreaseMode
        {
            /// <summary>
            /// Adds a value with every reagent.
            /// </summary>
            Addition,
            /// <summary>
            /// Subtracts a value with every reagent.
            /// </summary>
            Subtraction,
            /// <summary>
            /// Multiples a property with every reagent.
            /// </summary>
            Multiplication,
            /// <summary>
            /// Divides a value with every reagent.
            /// </summary>
            Division,
        }
        /// <summary>
        /// Current mode of increasion.
        /// </summary>
        public IncreaseMode Mode { get; set; }
        /// <summary>
        /// Value to step on every reagent.
        /// </summary>
        public double AdditionStep { get; set; }
        /// <summary>
        /// Coefficient to multiply a <see cref="AdditionStep"/> on with every step.
        /// </summary>
        public double AccelerationCoefficient { get; set; }
        /// <summary>
        /// Element to trigger reagent.
        /// </summary>
        public MagicElement Element { get; set; }
        /// <summary>
        /// Enables an automatic increasion for this property.
        /// </summary>
        /// <param name="step">Step to increase with an every reagent.</param>
        /// <param name="acceleration">Coefficient to increase the step.</param>
        /// <param name="element">Element(s) to filter triggers to increasion.</param>
        /// <param name="mode">Mode of increasing.</param>
        public IncreasablePropertyAttribute(double step, double acceleration, MagicElement element, IncreaseMode mode = IncreaseMode.Addition)
        {
            _defaultAdditionStep = AdditionStep = step;
            AccelerationCoefficient = acceleration;
            Element = element;
            Mode = mode;
        }
        /// <summary>
        /// Enables an automatic increasion for this property.
        /// </summary>
        /// <param name="step">Step to increase with an every reagent.</param>
        /// <param name="element">Element(s) to filter triggers to increasion.</param>
        /// <param name="mode">Mode of increasing.</param>
        public IncreasablePropertyAttribute(double step, MagicElement element, IncreaseMode mode = IncreaseMode.Addition) : this(step, 1, element, mode)
        {
        }
        /// <summary>
        /// Increases given property value with rules given by this attribute.
        /// </summary>
        /// <param name="spell">A spell instance.</param>
        /// <param name="prop">A property to increase (attributes don't contains their owners).</param>
        /// <param name="reagent">A reagent to filter if increasion is needed.</param>
        /// <returns><see langword="true"/> if the increasion was successful, otherwise <see langword="false"/>.</returns>
        public bool IncreaseValue(object spell, PropertyInfo prop, MagicElement reagent)
        {
            if (!Element.HasFlag(reagent)) return false;
            if (prop.PropertyType == typeof(double))
            {
                var value = (double)prop.GetValue(spell);
                switch (Mode)
                {
                    case IncreaseMode.Addition:
                        value += AdditionStep;
                        break;
                    case IncreaseMode.Subtraction:
                        value -= AdditionStep;
                        break;
                    case IncreaseMode.Multiplication:
                        value *= AdditionStep;
                        break;
                    case IncreaseMode.Division:
                        value /= AdditionStep;
                        break;
                }
                AdditionStep *= AccelerationCoefficient;
                prop.SetValue(spell, value);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Resets <see cref="AdditionStep"/> to defaults.
        /// </summary>
        public void ResetAttribute()
        {
            AdditionStep = _defaultAdditionStep;
        }

        public override string ToString()
        {
            return $"Element: {Element}, Step: {AdditionStep}, Acc: {AccelerationCoefficient}, Mode: {Mode}";
        }
    }
}
