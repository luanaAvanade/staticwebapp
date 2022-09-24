using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Gicaf.Domain
{
    public static class ReflectionExtensions
    {
        private const string ArgumentException_MustHaveExactlyOneParameter = "Must have exactly one parameter.";
        private const string ArgumentException_MustBePropertyAccessLambdaExpression = "Must be a lambda expression that expresses accessing a property.";
        private const string InvalidOperationException_NoMatchingMemberFound = "No matching member found.";
        private const string InvalidOperationException_MoreThanOneMatchingMembersFound = "More than one matching members found.";
        private const string InvalidOperationException_ExpressionIsNotLambda = "The specified expression is not a lambda expression.";
        private const string InvalidOperationException_ExpressionDoesNotHaveBody = "The specified expression does not have a body.";
        private const string InvalidOperationException_ExpressionDoesNotHaveMemberExpression = "The specified expression does not contain a member expression.";
        private const string InvalidOperationException_ExpressionDoesNotHaveMember = "The specified expression does not contain a member.";

        public static string GetPropertyName(this LambdaExpression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(ReflectionExtensions.GetArgumentName(() => expression));
            }

            MemberExpression memberExpression = null;

            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                memberExpression = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }

            if (memberExpression == null)
            {
                throw new ArgumentNullException(ArgumentException_MustBePropertyAccessLambdaExpression, ReflectionExtensions.GetArgumentName(() => expression));
            }

            return memberExpression.Member.Name;
        }


        /// <summary>
        ///     Gets the name of the specified property, which is expressed as a lambda expression.
        /// </summary>
        /// <typeparam name="TClass">The type of the class the property is a member of.</typeparam>
        /// <param name="property">The property, expressed as a lambda expression.</param>
        /// <returns>The name of the property.</returns>
        /// <remarks>
        ///     Use this method instead of using a string constant to denote a property name in
        ///     favor of refactorability.
        /// </remarks>
        public static string GetPropertyName<TClass>(Expression<Func<TClass, object>> property)
        {
            return GetPropertyName<TClass, object>(property);
        }

        /// <summary>
        ///     Gets the name of the specified property, which is expressed as a lambda expression.
        /// </summary>
        /// <typeparam name="TClass">The type of the class the property is a member of.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="property">The property, expressed as a lambda expression.</param>
        /// <returns>The name of the property.</returns>
        /// <remarks>
        ///     Use this method instead of using a string constant to denote a property name in
        ///     favor of refactorability.
        /// </remarks>
        public static string GetPropertyName<TClass, TResult>(Expression<Func<TClass, TResult>> property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(ReflectionExtensions.GetArgumentName(() => property));
            }

            return GetPropertyName(property);
        }

        /// <summary>
        ///     Gets the path of the specified property, which is expressed as a lambda expression.
        ///     The path will include the property name and all expressed parents.
        /// </summary>
        /// <typeparam name="TClass">The type of the class the property is a member of.</typeparam>
        /// <param name="property">The property, expressed as a lambda expression.</param>
        /// <param name="separator">The separator to use when composing the path.</param>
        /// <returns>The path of the property.</returns>
        /// <remarks>
        ///     Use this method instead of using a string constant to denote a property name in
        ///     favor of refactorability.
        /// </remarks>
        public static string GetPropertyPath<TClass>(Expression<Func<TClass, object>> property, string separator = ".")
        {
            return GetPropertyPath<TClass, object>(property, separator);
        }

        /// <summary>
        ///     Gets the path of the specified property, which is expressed as a lambda
        ///     expression. The path will include the property name and all expressed parents.
        /// </summary>
        /// <typeparam name="TClass">The type of the class the property is a member of.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="property">The property, expressed as a lambda expression.</param>
        /// <param name="separator">The separator to use when composing the path.</param>
        /// <returns>The path of the property.</returns>
        /// <remarks>
        ///     Use this method instead of using a string constant to denote a property name in
        ///     favor of refactorability.
        /// </remarks>
        public static string GetPropertyPath<TClass, TResult>(Expression<Func<TClass, TResult>> property, string separator = null)
        {
            if (property == null)
            {
                throw new ArgumentNullException(ReflectionExtensions.GetArgumentName(() => property));
            }

            if (separator == null)
            {
                separator = ".";
            }

            return string.Join(separator, GetPropertyInfos(property.Body).Select(n => n.Name));
        }

        /// <summary>
        ///     Gets the member info of the specified member, which is expressed as a lambda expression.
        /// </summary>
        /// <typeparam name="TClass">The type of the class the member is a member of.</typeparam>
        /// <param name="member">The member, expressed as a lambda expression.</param>
        /// <returns>The member info.</returns>
        public static MemberInfo GetMemberInfo<TClass>(Expression<Func<TClass, object>> member)
        {
            return GetMemberInfo<TClass, object>(member);
        }

        /// <summary>
        ///     Gets the member info of the specified member, which is expressed as a lambda expression.
        /// </summary>
        /// <typeparam name="TClass">The type of the class the member is a member of.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="member">The member, expressed as a lambda expression.</param>
        /// <returns>The member info.</returns>
        public static MemberInfo GetMemberInfo<TClass, TResult>(Expression<Func<TClass, TResult>> member)
        {
            if (member == null)
            {
                throw new ArgumentNullException(ReflectionExtensions.GetArgumentName(() => member));
            }

            if (member.Parameters.Count != 1)
            {
                throw new ArgumentException(ArgumentException_MustHaveExactlyOneParameter, ReflectionExtensions.GetArgumentName(() => member));
            };

            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Public;
            MemberInfo[] memberInfos = typeof(TClass).GetMember(GetPropertyName(member), bindingFlags);

            if (memberInfos.Length == 0)
            {
                throw new InvalidOperationException(InvalidOperationException_NoMatchingMemberFound);
            }
            else if (memberInfos.Length > 1)
            {
                throw new InvalidOperationException(InvalidOperationException_MoreThanOneMatchingMembersFound);
            }

            return memberInfos.First();
        }

        /// <summary>
        ///     Determines whether the specified type is a generic type of the specified generic definition.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="genericDefinition">The generic definition.</param>
        /// <returns><c>true</c> if is the type is a generic type of the specified generic definition; otherwise, <c>false</c>.</returns>
        public static bool IsGenericTypeOf(Type type, Type genericDefinition)
        {
            Type[] parameters = null;

            return IsGenericTypeOf(type, genericDefinition, out parameters);
        }

        /// <summary>
        ///     Determines whether the specified type is a generic type of the specified generic definition.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="genericDefinition">The generic definition.</param>
        /// <param name="genericParameters">The generic parameters.</param>
        /// <returns><c>true</c> if is the type is a generic type of the specified generic definition; otherwise, <c>false</c>.</returns>
        public static bool IsGenericTypeOf(Type type, Type genericDefinition, out Type[] genericParameters)
        {
            genericParameters = new Type[0];

            if (!genericDefinition.IsGenericType)
            {
                return false;
            }

            bool isMatch = type.IsGenericType && type.GetGenericTypeDefinition() == genericDefinition.GetGenericTypeDefinition();

            if (!isMatch && type.BaseType != null)
            {
                isMatch = IsGenericTypeOf(type.BaseType, genericDefinition, out genericParameters);
            }

            if (!isMatch && genericDefinition.IsInterface && type.GetInterfaces().Any())
            {
                foreach (Type i in type.GetInterfaces())
                {
                    if (ReflectionExtensions.IsGenericTypeOf(i, genericDefinition, out genericParameters))
                    {
                        isMatch = true;
                        break;
                    }
                }
            }

            if (isMatch && !genericParameters.Any())
            {
                genericParameters = type.GetGenericArguments();
            }

            return isMatch;
        }

        /// <summary>
        ///     Gets the property details for the properties that make up the specified expression.</summary>
        /// <param name="expression">The expression.</param>
        /// <returns>A collection of <see cref="PropertyInfo"/> instances.</returns>
        private static IEnumerable<PropertyInfo> GetPropertyInfos(Expression expression)
        {
            MemberExpression memberExpression = expression as MemberExpression;
            UnaryExpression unaryExpression = expression as UnaryExpression;

            if (unaryExpression != null && unaryExpression.Operand.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = unaryExpression.Operand as MemberExpression;
            }

            if (memberExpression == null)
            {
                yield break;
            }

            PropertyInfo property = memberExpression.Member as PropertyInfo;

            if (property == null)
            {
                throw new ArgumentException(ArgumentException_MustBePropertyAccessLambdaExpression, ReflectionExtensions.GetArgumentName(() => expression));
            }

            foreach (PropertyInfo propertyInfo in GetPropertyInfos(memberExpression.Expression))
            {
                yield return propertyInfo;
            }

            yield return property;
        }

        /// <summary>
        ///     Gets the name of the argument as represented by the specified expression.
        /// </summary>
        /// <param name="parameter">An expression that represents the argument to get the name of.</param>
        /// <returns>The name of the argument.</returns>
        public static string GetArgumentName(Expression<Func<object>> parameter)
        {
            LambdaExpression lambda = parameter as LambdaExpression;

            if (lambda == null)
            {
                throw new InvalidOperationException(InvalidOperationException_ExpressionIsNotLambda);
            }

            MemberExpression member = lambda.Body as MemberExpression;

            if (member == null)
            {
                UnaryExpression unary = lambda.Body as UnaryExpression;

                if (unary == null)
                {
                    throw new InvalidOperationException(InvalidOperationException_ExpressionDoesNotHaveBody);
                }

                member = unary.Operand as MemberExpression;

                if (member == null)
                {
                    throw new InvalidOperationException(InvalidOperationException_ExpressionDoesNotHaveMemberExpression);
                }
            }

            if (member.Member == null)
            {
                throw new InvalidOperationException(InvalidOperationException_ExpressionDoesNotHaveMember);
            }

            return member.Member.Name;
        }
    }
}
