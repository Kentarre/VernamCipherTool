using System;
using System.ComponentModel;
using System.Reflection;
using VernamCipherTool.Enums;

namespace VernamCipherTool.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Operation op)
        {
            FieldInfo field = op.GetType().GetField(op.ToString());

            return !(Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                is DescriptionAttribute attribute)
                    ? op.ToString()
                    : attribute.Description;
        }
    }
}
